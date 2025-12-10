using UnityEngine;
using System.Collections; // Necesario para la espera

public class HealthItem : MonoBehaviour
{
    [Header("Arrastra el sonido aquí")]
    public AudioClip pickupSound;

    private AudioSource audioSource;
    private SpriteRenderer sr;
    private Collider2D col;

    void Start()
    {
        // 1. Configurar AudioSource automáticamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // <--- ESTO FUERZA EL SONIDO A 2D (Volumen Máximo)

        // 2. Obtener componentes visuales
        sr = GetComponent<SpriteRenderer>();
        col = GetComponent<Collider2D>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Health playerHealth = collision.GetComponent<Health>();

        if (playerHealth != null)
        {
            playerHealth.FullHeal(); // Curar
            StartCoroutine(SonarYDesaparecer()); // Rutina especial
        }
    }

    IEnumerator SonarYDesaparecer()
    {
        // 1. Ocultar el corazón (Para que parezca que desapareció)
        if (sr != null) sr.enabled = false;
        if (col != null) col.enabled = false;

        // 2. Reproducir sonido
        if (pickupSound != null)
        {
            audioSource.clip = pickupSound;
            audioSource.Play();
            // Esperar lo que dure el sonido
            yield return new WaitForSeconds(pickupSound.length);
        }
        else
        {
            yield return new WaitForSeconds(0.1f);
        }

        // 3. Restaurar componentes (Para cuando reaparezca luego)
        if (sr != null) sr.enabled = true;
        if (col != null) col.enabled = true;

        // 4. Apagar el objeto (El Spawner lo prenderá después)
        gameObject.SetActive(false);
    }
}