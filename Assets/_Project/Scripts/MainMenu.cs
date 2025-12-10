using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; // ¡Vital para cambiar de escena!

public class MainMenu : MonoBehaviour
{
    public void PlayGame()
    {
        // Carga la siguiente escena en la lista (la del juego)
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitGame()
    {
        Debug.Log("¡Saliendo del juego!"); // Esto solo se ve en Unity
        Application.Quit(); // Esto cierra el juego real
    }
}