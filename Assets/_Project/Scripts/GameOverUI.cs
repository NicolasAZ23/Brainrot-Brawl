using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ¡Necesario para cambiar de escena!

public class GameOverUI : MonoBehaviour
{
    // Función para el botón "Reiniciar Nivel" (Si lo quieres tener)
    public void RestartLevel()
    {
        // Vuelve a cargar la escena en la que estás ahora mismo
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }

    // Función para el botón "Menú Principal"
    public void GoToMainMenu()
    {
        // Carga la escena con índice 0 (que debería ser tu menú)
        // Asegúrate en File > Build Settings que el menú sea el 0.
        SceneManager.LoadScene(0);
    }
}
