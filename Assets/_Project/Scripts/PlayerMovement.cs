using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input (Nombres en Input Manager)")]
    public string horizontalAxis = "Horizontal";
    public string jumpButton = "Jump";
    public string attackButton = "Fire1";
    public string dashButton = "Dash";

    [Header("Movement Stats")]
    public float moveSpeed = 8f;
    public float jumpForce = 16f;

    [Header("Wall Mechanics")]
    public Vector2 wallJumpForce = new Vector2(15f, 20f);
    public float wallJumpDuration = 0.1f;
    private bool isWallJumping;

    [Header("Dash Stats")]
    public float dashForce = 20f;
    public float dashDuration = 0.2f;
    public float dashCooldown = 1f;
    private bool canDash = true;
    private bool isDashing;

    [Header("Attack Settings")]
    public GameObject attackHitbox;
    public float attackDuration = 0.1f;
    public float attackCooldown = 0.5f;
    private bool canAttack = true;

    // ESTADOS (Solo para ver en inspector, no tocar)
    public bool isGrounded;
    public bool isTouchingWall;

    // COMPONENTES
    private Rigidbody2D rb;
    private BoxCollider2D boxCol;
    private Animator anim; // El cerebro de las animaciones
    private float horizontalInput;
    private float facingDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>();
        anim = GetComponent<Animator>(); // Conecta automáticamente con el Animator
    }

    void Update()
    {
        if (isDashing) return;

        // 1. DETECCIÓN INTELIGENTE (Busca el Tag "Ground")
        CheckCollisions();

        // 2. LEER TECLAS
        if (!isWallJumping) horizontalInput = Input.GetAxis(horizontalAxis);

        // 3. ENVIAR DATOS AL ANIMADOR (¡Aquí ocurre la magia!)
        if (anim != null)
        {
            // Le decimos qué tan rápido vamos (siempre positivo) para cambiar a RUN
            anim.SetFloat("Speed", Mathf.Abs(horizontalInput));

            // Le decimos si pisamos suelo para cambiar a JUMP o IDLE
            anim.SetBool("IsGrounded", isGrounded);
        }

        // 4. GIRAR PERSONAJE
        if (!isWallJumping)
        {
            bool lockedOnWall = isTouchingWall && !isGrounded;
            if (!lockedOnWall)
            {
                if (horizontalInput > 0 && facingDirection == -1) Flip();
                else if (horizontalInput < 0 && facingDirection == 1) Flip();
            }
        }

        // 5. SALTAR
        if (Input.GetButtonDown(jumpButton))
        {
            if (isGrounded)
            {
                Jump(); // Salto normal
            }
            else if (isTouchingWall)
            {
                StartCoroutine(WallJumpRoutine()); // Salto de pared
            }
        }

        // 6. ATACAR
        if (Input.GetButtonDown(attackButton) && canAttack)
        {
            // Avisamos al Animator para que inicie la animación
            if (anim != null) anim.SetTrigger("Attack");

            StartCoroutine(Attack());
        }

        // 7. DASH
        if (Input.GetButtonDown(dashButton) && canDash) StartCoroutine(Dash());
    }

    void FixedUpdate()
    {
        if (isDashing || isWallJumping) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // --- SISTEMA DE FÍSICAS (Cajas invisibles) ---
    private void CheckCollisions()
    {
        Bounds bounds = boxCol.bounds;

        // Caja de PIES (Más delgada para no tocar paredes)
        Vector2 groundBoxSize = new Vector2(bounds.size.x * 0.9f, 0.1f);
        RaycastHit2D hitGround = Physics2D.BoxCast(bounds.center, groundBoxSize, 0f, Vector2.down, 0.1f + (bounds.size.y / 2));

        // Verifica si tocamos algo con la etiqueta "Ground"
        isGrounded = (hitGround.collider != null && hitGround.collider.CompareTag("Ground"));

        // Caja de NARIZ (Pared)
        Vector2 wallBoxSize = new Vector2(0.1f, bounds.size.y * 0.8f);
        RaycastHit2D hitWall = Physics2D.BoxCast(bounds.center, wallBoxSize, 0f, Vector2.right * facingDirection, 0.1f + (bounds.size.x / 2));

        isTouchingWall = (hitWall.collider != null && hitWall.collider.CompareTag("Ground"));

        // DIBUJOS VERDES/ROJOS EN LA ESCENA
        Debug.DrawRay(bounds.center, Vector2.down * ((bounds.size.y / 2) + 0.1f), isGrounded ? Color.green : Color.red);
        Debug.DrawRay(bounds.center, Vector2.right * facingDirection * ((bounds.size.x / 2) + 0.1f), isTouchingWall ? Color.green : Color.red);
    }

    private void Jump()
    {
        rb.linearVelocity = new Vector2(rb.linearVelocity.x, 0);
        rb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
    }

    private IEnumerator WallJumpRoutine()
    {
        isWallJumping = true;
        float jumpDirection = -facingDirection;
        rb.linearVelocity = Vector2.zero;
        rb.AddForce(new Vector2(jumpDirection * wallJumpForce.x, wallJumpForce.y), ForceMode2D.Impulse);

        if (jumpDirection != facingDirection) Flip();

        yield return new WaitForSeconds(wallJumpDuration);
        isWallJumping = false;
    }

    private void Flip()
    {
        facingDirection *= -1;
        Vector3 scaler = transform.localScale;
        scaler.x *= -1;
        transform.localScale = scaler;
    }

    private IEnumerator Dash()
    {
        canDash = false;
        isDashing = true;
        float originalGravity = rb.gravityScale;
        rb.gravityScale = 0f;
        float dashDirection = Input.GetAxisRaw(horizontalAxis);
        if (dashDirection == 0) dashDirection = facingDirection;
        if (dashDirection != facingDirection && dashDirection != 0) Flip();

        rb.linearVelocity = new Vector2(dashDirection * dashForce, 0f);

        yield return new WaitForSeconds(dashDuration);
        rb.gravityScale = originalGravity;
        isDashing = false;
        yield return new WaitForSeconds(dashCooldown);
        canDash = true;
    }

    private IEnumerator Attack()
    {
        canAttack = false;
        if (attackHitbox != null) attackHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        if (attackHitbox != null) attackHitbox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}