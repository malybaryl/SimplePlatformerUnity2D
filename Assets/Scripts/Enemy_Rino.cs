using UnityEngine;

public class Enemy_Rino : Enemy
{
    [Header("Move info")]
    [SerializeField] private float agroSpeed;
    


    [SerializeField] private float shockTime;
    private float shockTimeCounter;


    [SerializeField] private LayerMask whatIsPlayer;
    private bool playerDetected;
    private bool agressive;
    protected override void Start()
    {
        base.Start();
        invincible = true;
    }

    void Update()
    {

        playerDetected = Physics2D.Raycast(wallCheck.position, Vector2.right * facingDirection, 25, whatIsPlayer);
        if (playerDetected)
            agressive = true;

        if (!agressive && !wasHited )
        {
            WalkAround();

        }
        else
        {
            rb.velocity = new Vector2(agroSpeed * facingDirection, rb.velocity.y);

            if (wallDetected && invincible)
            {
                invincible = false;
                shockTimeCounter = shockTime;
            }

            if (shockTimeCounter <= 0 && !invincible)
            {
                invincible = true;
                Flip();
                agressive = false;
            }
        }

        shockTimeCounter -= Time.deltaTime;

        CollisionsCheck();
        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("invincible", invincible);
    }

    //protected override void OnDrawGizmos()
    //{
    //    base.OnDrawGizmos();

    //    Gizmos.DrawLine(wallCheck.position, new Vector2(wallCheck.position.x + 25 * facingDirection, wallCheck.position.y));
    //}
}
