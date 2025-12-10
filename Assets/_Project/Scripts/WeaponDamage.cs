using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponDamage : MonoBehaviour
{
    [Header("Configuración")]
    public int damage = 10; // <--- CAMBIADO A INT (Para coincidir con Health)
    public string targetTag;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag(targetTag))
        {
            Health vidaRival = collision.gameObject.GetComponent<Health>();

            if (vidaRival != null)
            {
                // Ahora sí coinciden: damage es int y TakeDamage pide int
                vidaRival.TakeDamage(damage);
                Debug.Log("Golpeaste a " + collision.name);
            }
        }
    }
}