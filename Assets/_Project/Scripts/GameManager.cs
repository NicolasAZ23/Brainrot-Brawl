using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; // Necesario para el Texto
using UnityEngine.SceneManagement; // Necesario para Reiniciar la escena

public class GameManager : MonoBehaviour
{
    [Header("UI References")]
    public GameObject gameOverPanel; // El panel negro
    public Text winnerText; // El texto que dice quién ganó

    // Esta función se llama cuando alguien muere
    public void GameOver(string deadPlayerName)
    {
        // 1. Mostrar el panel
        gameOverPanel.SetActive(true);

        // 2. Decidir quién ganó
        if (deadPlayerName == "Player")
        {
            winnerText.text = "¡JUGADOR 2 GANA!";
            winnerText.color = Color.blue; // Opcional: Color para P2
        }
        else
        {
            winnerText.text = "¡JUGADOR 1 GANA!";
            winnerText.color = Color.green; // Opcional: Color para P1
        }
    }

    // Esta función la llamará el botón
    public void RestartGame()
    {
        // Recarga la escena actual (reinicia todo)
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
