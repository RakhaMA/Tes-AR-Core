using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using EnhancedTouch = UnityEngine.InputSystem.EnhancedTouch;
using TMPro;

[RequireComponent(typeof(ARRaycastManager), typeof(ARPlaneManager))]
public class SpawnEnemyOnPlane : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    public GameObject enemyPrefab;
    public float spawnInterval = 3f; // Time interval between enemy spawns
    private ARPlaneManager aRPlaneManager;
    private ARRaycastManager aRRaycastManager;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    public TextMeshProUGUI debugText;
    public TextMeshProUGUI debugText2;

    private void Awake()
    {
        aRRaycastManager = GetComponent<ARRaycastManager>();
        aRPlaneManager = GetComponent<ARPlaneManager>();
    }

    private void OnEnable()
    {
        EnhancedTouch.TouchSimulation.Enable();
        EnhancedTouch.EnhancedTouchSupport.Enable();
        EnhancedTouch.Touch.onFingerDown += FingerDown; // Register the FingerDown event handler
    }

    private void OnDisable()
    {
        EnhancedTouch.TouchSimulation.Disable();
        EnhancedTouch.EnhancedTouchSupport.Disable();
        EnhancedTouch.Touch.onFingerDown -= FingerDown; // Unregister the FingerDown event handler
    }

    private void Start()
    {
        StartCoroutine(SpawnEnemiesRoutine());
    }

    private IEnumerator SpawnEnemiesRoutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(spawnInterval);

            if (aRRaycastManager.Raycast(new Vector2(Screen.width / 2f, Screen.height / 2f), hits, TrackableType.PlaneWithinPolygon))
            {
                Debug.Log("Plane detected!");
                debugText.text = "Plane detected! " + aRPlaneManager.GetPlane(hits[0].trackableId).alignment.ToString();

                foreach (ARRaycastHit hit in hits)
                {
                    Pose pose = hit.pose;
                    Quaternion rotation = pose.rotation * Quaternion.Euler(0f, 180f, 0f);
                    GameObject enemy = Instantiate(enemyPrefab, pose.position, rotation);
                    debugText2.text = "Enemy spawned!";

                    if (aRPlaneManager.GetPlane(hit.trackableId).alignment == PlaneAlignment.HorizontalUp)
                    {
                        StartCoroutine(MoveTowardCamera(enemy));
                    }
                }
            }
            else
            {
                Debug.Log("No plane detected.");
                debugText.text = "No plane detected.";
            }
        }
    }

    IEnumerator MoveTowardCamera(GameObject enemy)
    {
        Vector3 position = enemy.transform.position;
        position.y = 0f;
        Vector3 arCamera = Camera.main.transform.position;
        arCamera.y = 0f;
        Vector3 direction = position - arCamera; // Reverse the direction calculation
        Quaternion targetRotation = Quaternion.LookRotation(direction);
        enemy.transform.rotation = Quaternion.Slerp(enemy.transform.rotation, targetRotation, 1f);

        while (Vector3.Distance(enemy.transform.position, arCamera) > 0.1f)
        {
            enemy.transform.position = Vector3.MoveTowards(enemy.transform.position, arCamera, moveSpeed * Time.deltaTime);
            yield return null;
        }
    }


    private void FingerDown(EnhancedTouch.Finger finger)
    {
        // Empty event handler for FingerDown, since we're not using it in this version
        // You can remove this method if you don't plan to use it for any other functionality
    }
}
