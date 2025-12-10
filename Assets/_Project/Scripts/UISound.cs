using UnityEngine;

public class UISound : MonoBehaviour
{
    [Header("Arrastra tu sonido de Click aquí")]
    public AudioClip sonidoClick;

    private AudioSource audioSource;

    void Start()
    {
        // Creamos el componente de audio automáticamente
        audioSource = gameObject.AddComponent<AudioSource>();
        audioSource.playOnAwake = false;
        audioSource.spatialBlend = 0; // Fuerza el sonido a 2D (se escucha fuerte)
    }

    // Esta es la función que llamarán los botones
    public void ReproducirSonido()
    {
        if (sonidoClick != null)
        {
            audioSource.PlayOneShot(sonidoClick);
        }
    }
}
