using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    [Header("Vida y UI")]
    public int maxHealth = 100;
    public int currentHealth;
    public Slider healthBar;

    [Header("Muerte y Caída")]
    public GameObject gameOverPanel;
    public float voidYLevel = -10f;

    [Header("Sonidos")]
    public AudioClip hitSound; // Arrastra el sonido de daño aquí
    private AudioSource audioSource;

    private SpriteRenderer spriteRenderer;
    private Color originalColor;
    private bool isDead = false;

    void Start()
    {
        currentHealth = maxHealth;
        spriteRenderer = GetComponent<SpriteRenderer>();
        audioSource = GetComponent<AudioSource>(); // Necesitas este componente en el objeto

        if (spriteRenderer != null) originalColor = spriteRenderer.color;

        if (healthBar != null)
        {
            healthBar.maxValue = maxHealth;
            healthBar.value = currentHealth;
        }

        if (gameOverPanel != null) gameOverPanel.SetActive(false);
    }

    void Update()
    {
        if (transform.position.y < voidYLevel && !isDead) Die();
    }

    public void TakeDamage(int amount)
    {
        if (isDead) return;

        currentHealth -= amount;
        if (healthBar != null) healthBar.value = currentHealth;

        // Sonido de recibir daño
        if (audioSource != null && hitSound != null) audioSource.PlayOneShot(hitSound);

        StartCoroutine(DamageFlash());

        if (currentHealth <= 0) Die();
    }

    public void FullHeal()
    {
        if (isDead) return;
        currentHealth = maxHealth;
        if (healthBar != null) healthBar.value = currentHealth;
    }

    void Die()
    {
        isDead = true;
        if (gameOverPanel != null) gameOverPanel.SetActive(true);
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