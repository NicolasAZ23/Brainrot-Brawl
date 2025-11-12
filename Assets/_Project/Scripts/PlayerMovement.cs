using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    private Rigidbody2D rb;
    public float jumpForce = 7f;
    public Transform groundCheck;
    public LayerMask groundLayer;
    private bool isGrounded;
    [Header("Attack Settings")]
    public float attackCooldown = 0.5f; // Medio segundo entre ataques
    private bool canAttack = true;
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Dejamos este vac�o por ahora
        // 1. Revisamos si el sensor (groundCheck) está tocando el suelo (groundLayer)
        // Dibuja un círculo invisible de 0.2 de radio en el sensor. Si toca "Ground", isGrounded = true.
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);

        // 2. Revisamos si el jugador APRETA el botón de salto
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            // "Jump" es la barra espaciadora por defecto
            // Le damos un "impulso" vertical.
            rb.AddForce(new Vector2(rb.linearVelocity.x, jumpForce), ForceMode2D.Impulse);
        }
        // 3. Revisamos si el jugador APRETA el botón de ataque
        // "Fire1" es Clic Izquierdo o Ctrl Izquierdo por defecto
        if (Input.GetButtonDown("Fire1") && canAttack)
        {
            StartCoroutine(Attack());
        }
    }

    void FixedUpdate()
    {
        // 1. Obtener el input (qu� tecla estoy presionando)
        float horizontalInput = Input.GetAxis("Horizontal"); // Esto es A/D o Flecha Izq/Der

        // 2. Calcular la nueva velocidad
        // (No queremos afectar el salto/gravedad, as� que mantenemos la velocidad vertical 'rb.velocity.y')
        Vector2 nuevaVelocidad = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);

        // 3. Aplicar la velocidad al cuerpo
        rb.linearVelocity = nuevaVelocidad;
    }
    void OnDrawGizmos()
    {
        if (groundCheck == null)
            return;

        // Dibuja un círculo rojo donde está nuestro sensor
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, 0.2f);
    }
    private IEnumerator Attack()
    {
        // 1. Marcar que estamos atacando (para no poder atacar de nuevo)
        canAttack = false;

        // 2. La acción (aquí irá la animación)
        Debug.Log("¡TUNG TUNG SAHUR! (Ataque)");

        // 3. La pausa (el cooldown)
        yield return new WaitForSeconds(attackCooldown);

        // 4. Permitir atacar de nuevo
        canAttack = true;
    }
}