using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Configuración de Vida")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar; // La barra verde

    [Header("Configuración de Muerte")]
    public GameObject gameOverPanel; // El panel de Game Over que debe aparecer
    public float voidYLevel = -10f;  // A qué altura mueres si caes (eje Y)

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false; // Para que no muera dos veces

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) originalColor = spriteRenderer.color;

        // Iniciar barra llena
        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        // Asegurarnos de que el panel de Game Over esté oculto al empezar
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(false);
        }
    }

    void Update()
    {
        // --- DETECTAR CAÍDA AL VACÍO ---
        // Si mi posición Y es menor que el límite, muero.
        if (transform.position.y < voidYLevel && !isDead)
        {
            Die();
        }
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return; // Si ya está muerto, no le pegues más

        currentHealth -= amount;

        if (healthBar != null) healthBar.value = currentHealth;

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0)
        {
            Die();
        }
    }

    void Die()
    {
        isDead = true;
        Debug.Log(gameObject.name + " HA MUERTO.");

        // 1. Mostrar el Game Over
        if (gameOverPanel != null)
        {
            gameOverPanel.SetActive(true);
        }

        // 2. Desactivar al personaje para que no se mueva más
        gameObject.SetActive(false);
    }

    private IEnumerator DamageFlash()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.color = Color.red;
            yield return new WaitForSeconds(0.1f);
            spriteRenderer.color = originalColor;
        }
    }
}