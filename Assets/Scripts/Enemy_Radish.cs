using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Radish : Enemy
{
    private RaycastHit2D groundBelowDetected;
    private bool groundAboveDetected;

    [Header("Radish specifics")]
    [SerializeField] private float distanceToGroundBelow;
    [SerializeField] private float distanceToGroundAbove;

    [SerializeField] private float aggroTime;
    private float aggroTimeCounter;

    
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        aggroTimeCounter -= Time.deltaTime;
        if(aggroTimeCounter < 0)
        {
            rb.gravityScale = 1;
            aggresive = false;
        }

        if(!aggresive)
        {
            aggroTimeCounter = aggroTime;
            if (groundAboveDetected == false)
            {
                rb.velocity = new Vector2(0, 1);
                rb.gravityScale = 0;
            }
            else
            {
                rb.velocity = new Vector2(0,0);
                rb.gravityScale = 0;
            }
        }
        else if (!wasHited) 
        {
            if (groundBelowDetected.distance < 1.25f)
            WalkAround();
        }

        CollisionsCheck();

        anim.SetFloat("xVelocity", rb.velocity.x);
        anim.SetBool("aggresive", aggresive);
    }
    public override void Damage()
    {
        if (!aggresive)
        {
            rb.gravityScale = 12;
            aggresive = true;
        }
        else
            base.Damage();
    }
    protected override void CollisionsCheck()
    {
        base.CollisionsCheck();

        groundAboveDetected = Physics2D.Raycast(transform.position, Vector2.up, distanceToGroundAbove, whatIsGroud);
        groundBelowDetected = Physics2D.Raycast(transform.position, Vector2.up, distanceToGroundBelow, whatIsGroud);

    }
    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y + distanceToGroundAbove));
        Gizmos.DrawLine(transform.position, new Vector2(transform.position.x , transform.position.y - distanceToGroundBelow));

    }


}
