using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    public int maxHealth = 100;
    private int currentHealth;

    [Header("Connections")]
    public Slider healthSlider;
    public GameManager gameManager; // <--- ¡NUEVA CONEXIÓN!

    void Start()
    {
        currentHealth = maxHealth;

        if (healthSlider != null)
        {
            healthSlider.maxValue = maxHealth;
            healthSlider.value = currentHealth;
        }

        // TRUCO: Si se nos olvida conectar el GameManager manualmente, lo buscamos auto
        if (gameManager == null)
        {
            gameManager = FindObjectOfType<GameManager>();
        }
    }

    public void TakeDamage(int damageAmount)
    {
        currentHealth -= damageAmount;

        if (healthSlider != null)
        {
            healthSlider.value = currentHealth;
        }

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        // Avisamos al árbitro ANTES de desaparecer
        if (gameManager != null)
        {
            gameManager.GameOver(gameObject.name);
        }

        gameObject.SetActive(false);
    }
}