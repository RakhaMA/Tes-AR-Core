using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ZombieScript : MonoBehaviour
{
    public float health = 100f;
    public TextMeshProUGUI debugText;
    public UnityEvent onDie;
    

    public void TakeDamage(float damage)
    {
        health -= damage;
        debugText.text = "Health: " + health.ToString();
        if (health <= 0f)
        {
            onDie.Invoke();
            StartCoroutine(DieRoutine());
        }
    }

    IEnumerator DieRoutine()
    {
        yield return new WaitForSeconds(3f);
        Destroy(gameObject);
    }

    
}
