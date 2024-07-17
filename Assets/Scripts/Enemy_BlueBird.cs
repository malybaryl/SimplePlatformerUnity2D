using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_BlueBird : Enemy
{
    private bool ceillingDetected;

    [Header("Blue Bird specifics")]
    [SerializeField] private float distanceToGroundBelow;
    [SerializeField] private float distanceToGroundAbove;


    [SerializeField] private float flyUpForce;
    [SerializeField] private float flyDownForce;
    [SerializeField] private bool flyAround = true;
    private float flyForce;

    private bool canFly = true;

    protected override void Start()
    {
        base.Start();
    }

    private void Update()
    {
        CollisionsCheck();

        if (ceillingDetected)
            flyForce = flyDownForce;
        else if (groundDetected)
            flyForce = flyUpForce;

        if (flyAround)
        {
            if (wallDetected)
                Flip();
        }
   
    }

    [SerializeField] private Transform movePoint;
    [SerializeField] private float xMultiplier;
    [SerializeField] private float yMultiplier;

    public override void Damage()
    {
        canFly = false;
        rb.gravityScale = 0;
        base.Damage();
    }
    public void FlyUpEvent()
    {
        if (flyAround)
        {
            if (canFly)
            {
                rb.velocity = new Vector2(speed * facingDirection, flyForce);
            }
        }
        else
        {
            if (canFly)
            {
                Vector2 direction = transform.position - movePoint.position;
                rb.velocity = new Vector2(-direction.x * xMultiplier, -direction.y * yMultiplier);
            }
        }
    }
    protected override void CollisionsCheck()
    {
        base.CollisionsCheck();

        ceillingDetected = Physics2D.Raycast(transform.position, Vector2.up, distanceToGroundAbove, whatIsGroud);
        

    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + distanceToGroundAbove));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y - distanceToGroundBelow));

    }
}
