using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    protected Animator anim;
    protected Rigidbody2D rb;
    protected int facingDirection = -1;

    [SerializeField] protected LayerMask whatIsGroud;
    [SerializeField] protected float groundCheckDistance;
    [SerializeField] protected float wallCheckDistance;
    [SerializeField] protected Transform groundCheck;
    [SerializeField] protected Transform wallCheck;

    protected bool wallDetected;
    protected bool groundDetected;

    [HideInInspector] public bool invincible;
    protected bool wasHited = false;

    [Header("Move info")]
    [SerializeField] protected float speed;
    [SerializeField] protected float idleTime;
    protected float idleTimeCounter;

    protected virtual void Start()
    {
        anim = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    protected virtual void WalkAround()
    {
        if (idleTimeCounter < 0)
        {
            rb.velocity = new Vector2(speed * facingDirection, rb.velocity.y);
        }

        idleTimeCounter -= Time.deltaTime;

        if (wallDetected || !groundDetected)
        {
            idleTimeCounter = idleTime;
            Flip();
        }
    }

    public virtual void Damage()
    {
        if (!invincible)
        {
            anim.SetTrigger("gotHit");
            wasHited = true;
        }
    }
   

    private void OnTriggerEnter2D(Collider2D collision)
    {
    if (collision.GetComponent<Player>() != null)
        {
            Player player = collision.GetComponent<Player>();
            player.Knockback(transform);
        }
    }

    protected virtual void Flip()
    {
        facingDirection = facingDirection * -1;
        transform.Rotate(0, 180, 0);
    }

    protected virtual void CollisionsCheck()
    {
        groundDetected = Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, whatIsGroud);
        wallDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, wallCheckDistance, whatIsGroud);

    }

   protected virtual void OnDrawGizmos()
    {
        Gizmos.DrawLine(groundCheck.position, new Vector2(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + wallCheckDistance * facingDirection, wallCheck.position.y)); 
    }

    public void DestroyMe()
    {
        Destroy(gameObject);
    }
}