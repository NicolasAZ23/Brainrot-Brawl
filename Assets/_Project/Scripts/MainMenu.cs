using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [Header("Paneles (Arrastra desde el Canvas)")]
    public GameObject mainMenuContainer; // El grupo con Start/Quit
    public GameObject mapSelectorPanel;  // El panel con las fotos de mapas

    // --- FUNCIONES DE INICIO ---

    public void PlayGame()
    {
        // En lugar de ir directo al juego, abrimos el selector de mapas
        mainMenuContainer.SetActive(false);
        mapSelectorPanel.SetActive(true);
    }

    public void QuitGame()
    {
        Debug.Log("Saliendo...");
        Application.Quit();
    }

    // --- SELECCIÓN DE MAPAS ---

    public void BackToMenu()
    {
        mainMenuContainer.SetActive(true);
        mapSelectorPanel.SetActive(false);
    }

    public void LoadForest() { SceneManager.LoadScene(1); }
    public void LoadCastle() { SceneManager.LoadScene(2); }
}