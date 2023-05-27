using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class SpawnEnemyOnPlane : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public GameObject enemyPrefab;
    private ARPlaneManager aRPlaneManager;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();

    private void Awake() {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable() {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown;
    }

    private void OnDisable() {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown;
    }
    
    private void FingerDown(EnhancedTouch.Finger finger) {
        if(finger.index != 0) return;

        if(aRRaycastManager.Raycast(finger.currentTouch.screenPosition, hits, TrackableType.PlaneWithinPolygon)) {
            foreach(ARRaycastHit hit in hits) {
                Pose pose = hit.pose;
                GameObject enemy = Instantiate(enemyPrefab, pose.position, pose.rotation);

                if(aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp) {
                    //move enemy toward camera
                    StartCoroutine(MoveTowardCamera(enemy));
                }
            }
        }
    }

    IEnumerator MoveTowardCamera(GameObject enemy)
    {
        Vector3 position = enemy.transform.position;
        position.y = 0f;
        Vector3 arCamera = Camera.main.transform.position;
        arCamera.y = 0f;
        Vector3 direction = arCamera - position;
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 1f);


        while(Vector3.Distance(enemy.transform.position, arCamera) > 0.1f) {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, arCamera, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }
}
