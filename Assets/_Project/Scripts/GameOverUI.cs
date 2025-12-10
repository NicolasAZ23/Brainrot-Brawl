using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections; // <--- OBLIGATORIO para la espera

public class GameOverUI : MonoBehaviour
{
    [Header("Arrastra el sonido aquí")]
    public AudioClip clickSound;

    private AudioSource audioSource;

    void Start()
    {
        // Buscamos el Audio Source en este mismo objeto
        audioSource = GetComponent<AudioSource>();
    }

    // --- BOTÓN REINICIAR ---
    public void RestartLevel()
    {
        StartCoroutine(EsperarYReiniciar());
    }

    // --- BOTÓN MENÚ ---
    public void GoToMainMenu()
    {
        StartCoroutine(EsperarYMenu());
    }

    // --- RUTINAS DE ESPERA (LA SOLUCIÓN) ---

    IEnumerator EsperarYReiniciar()
    {
        // 1. Sonar
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        // 2. Esperar 0.4 segundos
        yield return new WaitForSeconds(0.4f);

        // 3. Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    IEnumerator EsperarYMenu()
    {
        // 1. Sonar
        if (audioSource != null && clickSound != null)
            audioSource.PlayOneShot(clickSound);

        // 2. Esperar 0.4 segundos
        yield return new WaitForSeconds(0.4f);

        // 3. Cargar menú (Índice 0)
        SceneManager.LoadScene(0);
    }
}
