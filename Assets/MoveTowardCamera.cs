using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTowardCamera : MonoBehaviour
{
    public float moveSpeed = 0.2f;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(moveTowardCamera(this.gameObject));
    }

    IEnumerator moveTowardCamera(GameObject enemy)
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
}
