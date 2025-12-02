using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public string horizontalAxis = "Horizontal";
    public string jumpButton = "Jump";
    public string attackButton = "Fire1";
    public string dashButton = "Dash";
    public string blockButton = "Block";

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

    // ESTADOS (Para que veas si funciona en el Inspector)
    public bool isGrounded;
    public bool isTouchingWall;

    // Visuals
    public bool isBlocking = false;
    private Color originalColor;
    private SpriteRenderer spriteRenderer;

    private Rigidbody2D rb;
    private BoxCollider2D boxCol; // Necesitamos tu colisionador principal
    private float horizontalInput;
    private float facingDirection = 1;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        boxCol = GetComponent<BoxCollider2D>(); // <--- ¡Auto-detectamos tu tamaño!
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (spriteRenderer) originalColor = spriteRenderer.color;
    }

    void Update()
    {
        if (isDashing) return;

        // ---------------------------------------------------------
        // NUEVA DETECCIÓN AUTOMÁTICA (Sin Layers, solo Tags)
        // ---------------------------------------------------------
        CheckCollisions();

        // INPUT
        if (!isWallJumping) horizontalInput = Input.GetAxis(horizontalAxis);

        // BLOQUEO
        if (Input.GetButton(blockButton) && isGrounded)
        {
            isBlocking = true;
            if (spriteRenderer) spriteRenderer.color = Color.blue;
            horizontalInput = 0;
        }
        else
        {
            isBlocking = false;
            if (!isDashing && spriteRenderer) spriteRenderer.color = originalColor;
        }

        // GIRO
        if (!isBlocking && !isWallJumping)
        {
            bool lockedOnWall = isTouchingWall && !isGrounded;
            if (!lockedOnWall)
            {
                if (horizontalInput > 0 && facingDirection == -1) Flip();
                else if (horizontalInput < 0 && facingDirection == 1) Flip();
            }
        }

        // SALTO
        if (Input.GetButtonDown(jumpButton) && !isBlocking)
        {
            if (isGrounded)
            {
                Jump();
            }
            else if (isTouchingWall)
            {
                StartCoroutine(WallJumpRoutine());
            }
        }

        if (!isBlocking)
        {
            if (Input.GetButtonDown(attackButton) && canAttack) StartCoroutine(Attack());
            if (Input.GetButtonDown(dashButton) && canDash) StartCoroutine(Dash());
        }
    }

    void FixedUpdate()
    {
        if (isDashing || isWallJumping) return;
        rb.linearVelocity = new Vector2(horizontalInput * moveSpeed, rb.linearVelocity.y);
    }

    // ---------------------------------------------------------
    // LA MAGIA: DETECCIÓN POR CAJAS (BoxCast)
    // ---------------------------------------------------------
    private void CheckCollisions()
    {
        // 1. Detectar SUELO
        // Creamos una caja un poco más flaca que el jugador (0.9f) para que no toque las paredes laterales
        Bounds bounds = boxCol.bounds;
        Vector2 boxSize = new Vector2(bounds.size.x * 0.9f, 0.1f);

        // Lanzamos la caja hacia abajo
        RaycastHit2D hitGround = Physics2D.BoxCast(bounds.center, boxSize, 0f, Vector2.down, 0.1f + (bounds.size.y / 2));

        // Verificamos: ¿Tocó algo? Y si lo tocó, ¿tiene la etiqueta "Ground"?
        isGrounded = (hitGround.collider != null && hitGround.collider.CompareTag("Ground"));


        // 2. Detectar PARED
        // Lanzamos una caja hacia el frente
        RaycastHit2D hitWall = Physics2D.BoxCast(bounds.center, new Vector2(0.1f, bounds.size.y * 0.8f), 0f, Vector2.right * facingDirection, 0.1f + (bounds.size.x / 2));

        isTouchingWall = (hitWall.collider != null && hitWall.collider.CompareTag("Ground"));

        // DIBUJO PARA QUE VEAS LAS CAJAS EN LA SCENE (Rojo = Nada, Verde = Detectado)
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
        attackHitbox.SetActive(true);
        yield return new WaitForSeconds(attackDuration);
        attackHitbox.SetActive(false);
        yield return new WaitForSeconds(attackCooldown);
        canAttack = true;
    }
}