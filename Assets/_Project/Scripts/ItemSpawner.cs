using UnityEngine;
using System.Collections;

public class ItemSpawner : MonoBehaviour
{
    public GameObject healthObject; // Arrastra aquí tu poción
    public float respawnTime = 10f; // Segundos para reaparecer

    void Start()
    {
        // Empezamos la rutina de vigilancia
        StartCoroutine(SpawnRoutine());
    }

    IEnumerator SpawnRoutine()
    {
        while (true) // Bucle infinito
        {
            // Si el objeto está desactivado (porque alguien lo agarró)
            if (!healthObject.activeSelf)
            {
                yield return new WaitForSeconds(respawnTime); // Espera el tiempo
                healthObject.SetActive(true); // Lo vuelve a activar
            }
            yield return new WaitForSeconds(1f); // Revisa cada segundo
        }
    }
}