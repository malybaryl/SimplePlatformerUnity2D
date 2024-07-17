using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private Animator anim;
    private SpriteRenderer sr;

    [Header("Camera")]
    [SerializeField] private GameObject camera;
    [SerializeField] private float cameraOffsetX = 2f;
    [SerializeField] private float cameraOffsetY = 1f;
    [SerializeField] private float cameraSmoothTime = 0.2f;

    private Vector3 cameraVelocity = Vector3.zero;
    private bool isFlipped = false;

    [Header("Move info")]
    public float moveSpeed;
    public float jumpForce;

    private bool canDoubleJump = true;
    private float movingInput;
    [SerializeField] private float bufferJumpTime;
    private float bufferJumpTimer;
    [SerializeField] private float cayoteJumpTime;
    private float cayoteJumpTimer;
    private bool canHaveCayoteJump;

    [Header("Knockback info")]
    [SerializeField] private Vector2 knockbackDirection;
    private bool isKnocked;
    [SerializeField] private float knockBackTime;
    private bool canBeKnocked = true;
    [SerializeField] private float knockBackProtectionTime;

    [Header("Collision info")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform enemyCheck;
    [SerializeField] private float enemyCheckRadious;
    private bool isGrounded;
    private bool isWallToLeft;
    private bool isWallToRight;
    private bool canWallSlide;
    private bool isWallSliding;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        AnimationsControllers();

        if (isKnocked)
        {
            return;
        }

        UpdateCameraPosition();

        DirectionsControllers();

        CollisionChecks();
        InputChecks();
        CheckForEnemy();
        
        BufferJumpAndCoyoteJump();

        HandleWallSliding();
        Move();
    }

    private void BufferJumpAndCoyoteJump()
    {
        bufferJumpTimer -= Time.deltaTime;
        cayoteJumpTimer -= Time.deltaTime;

        if (isGrounded)
        {
            canDoubleJump = true;

            if (bufferJumpTimer > 0)
            {
                bufferJumpTime = -1;
                Jump();
            }
            canHaveCayoteJump = true;
        }
        else
        {
            if (canHaveCayoteJump)
            {
                cayoteJumpTimer = cayoteJumpTime;
                canHaveCayoteJump = false;
            }
        }
    }

    private void CheckForEnemy()
    {
        Collider2D[] hitedColliders = Physics2D.OverlapCircleAll(enemyCheck.position, enemyCheckRadious);
        foreach (var enemy in hitedColliders)
        {
            if (enemy.GetComponent<Enemy>() != null)
            {
                Enemy newEnemy = enemy.GetComponent<Enemy>();

                if (newEnemy.invincible)
                    return;

                if (rb.velocity.y < 0)
                {
                    newEnemy.Damage();
                    Jump();
                }
            }
        }

    }

    private void UpdateCameraPosition()
    {
        if (camera != null)
        {
            Vector3 targetPosition = transform.position;

            if (sr.flipX)
            {
                targetPosition.x -= cameraOffsetX;
            }
            else
            {
                targetPosition.x += cameraOffsetX;
            }

            targetPosition.y += cameraOffsetY;
            targetPosition.z = camera.transform.position.z;

            camera.transform.position = Vector3.SmoothDamp(camera.transform.position, targetPosition, ref cameraVelocity, cameraSmoothTime);
        }
    }

    private void DirectionsControllers()
    {
        if (rb.velocity.x < -0.1f)
        {
            sr.flipX = true;
            isFlipped = true;
        }
        else if (rb.velocity.x > 0.1f)
        {
            sr.flipX = false;
            isFlipped = false;
        }
    }

    private void AnimationsControllers()
    {
        bool isMoving = Mathf.Abs(rb.velocity.x) > 0.1f;
        anim.SetBool("isMoving", isMoving);

        bool isJumping = !isGrounded && rb.velocity.y > 0;
        anim.SetBool("isJumping", isJumping);

        bool isFalling = !isGrounded && rb.velocity.y < 0 && !isWallSliding;
        anim.SetBool("isFalling", isFalling);

        anim.SetBool("isWallSliding", isWallSliding);
        anim.SetBool("isKnocked", isKnocked);
    }

    private void InputChecks()
    {
        movingInput = Input.GetAxisRaw("Horizontal");

        if (Input.GetKeyDown(KeyCode.Space) || Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
        {
            JumpButton();
        }
    }

    private void JumpButton()
    {
        if (!isGrounded)
            bufferJumpTimer = bufferJumpTime;

        if (isGrounded || cayoteJumpTimer > 0)
        {
            Jump();
        }
        else if (canDoubleJump)
        {
            canDoubleJump = false;
            Jump();
        }
        else if (isWallSliding)
        {
            WallJump();
        }
    }

    public void Knockback(Transform damageTransform)
    {
        if (!canBeKnocked)
            return;

        isKnocked = true;
        canBeKnocked = false;

        #region Define horizontal direction for knockback
        int hDirection = 0;

        if (transform.position.x > damageTransform.position.x)
            hDirection = 1;
        else if (transform.position.x < damageTransform.position.x)
            hDirection = -1;
        else
            hDirection = 0;
        #endregion

        rb.velocity = new Vector2(knockbackDirection.x * hDirection, knockbackDirection.y);

        Invoke("CancelKnockBack", knockBackTime);
        Invoke("AllowKnockback", knockBackProtectionTime);
    }

    private void AllowKnockback()
    {
        canBeKnocked = true;
    }

    private void CancelKnockBack()
    {
        isKnocked = false;
    }

    private void WallJump()
    {
        if (isWallToLeft)
        {
            rb.velocity = new Vector2(jumpForce, jumpForce);
        }
        else if (isWallToRight)
        {
            rb.velocity = new Vector2(-jumpForce, jumpForce);
        }

        canWallSlide = false;
    }

    private void HandleWallSliding()
    {
        if ((isWallToLeft && movingInput < 0) || (isWallToRight && movingInput > 0))
        {
            isWallSliding = true;
            canDoubleJump = true;
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f);
        }
        else
        {
            isWallSliding = false;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - groundCheckDistance));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x - wallCheckDistance, transform.position.y));
        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x + wallCheckDistance, transform.position.y));
        Gizmos.DrawWireSphere(enemyCheck.position, enemyCheckRadious);
    }

    private void Move()
    {
        rb.velocity = new Vector2(moveSpeed * movingInput, rb.velocity.y);
    }

    private void CollisionChecks()
    {
        isGrounded = Physics2D.Raycast(transform.position, Vector2.down, groundCheckDistance, whatIsGround);
        isWallToLeft = Physics2D.Raycast(transform.position, Vector2.left, wallCheckDistance, whatIsGround);
        isWallToRight = Physics2D.Raycast(transform.position, Vector2.right, wallCheckDistance, whatIsGround);

        canWallSlide = (isWallToLeft || isWallToRight) && !isGrounded && rb.velocity.y < 0;
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
    }
}
