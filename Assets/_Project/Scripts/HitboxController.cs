using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HitboxController : MonoBehaviour
{
    public int damage = 10; // ¿Cuánto pegamos?

    // Esta función mágica de Unity se llama SOLA
    // cuando nuestro "Trigger" (el puño) TOCA a otro "Collider" (el enemigo).
    private void OnTriggerEnter2D(Collider2D other)
    {
        // 1. Revisamos si "lo otro" (other) que tocamos tiene un script de "Health".
        Health enemyHealth = other.GetComponent<Health>();

        // 2. Si SÍ lo tiene (enemyHealth no es 'null')...
        if (enemyHealth != null)
        {
            // 3. ¡Le pegamos! Llamamos a su función "TakeDamage".
            enemyHealth.TakeDamage(damage);
        }
    }
}