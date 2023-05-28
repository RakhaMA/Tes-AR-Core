using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PlayerHealth : MonoBehaviour
{
    
    public float playerHealth = 100f;
    public float damageTaken = 25f;

    public void TakeDamage(float damage)
    {
        playerHealth -= damage;
        if (playerHealth <= 0f)
        {
            GameManager.instance.GameOver();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Zombie"))
        {
            TakeDamage(damageTaken);
        }
    }
}
