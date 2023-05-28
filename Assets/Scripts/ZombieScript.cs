using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;

public class ZombieScript : MonoBehaviour
{
    public float health, maxHealth = 100f;
    public TextMeshProUGUI debugText;
    public UnityEvent onDie;
    [SerializeField] private FloatingHealthBar healthBar;
    
    private void Awake() {
        healthBar = GetComponentInChildren<FloatingHealthBar>();
    }

    private void Start() {
        health = maxHealth;
        healthBar.UpdateHealthBar(health, maxHealth);
    }

    public void TakeDamage(float damage)
    {
        health -= damage;
        healthBar.UpdateHealthBar(health, maxHealth);
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
