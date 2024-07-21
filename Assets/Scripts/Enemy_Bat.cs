using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Bat : Enemy
{
    [Header("Bat specifics")]
    [SerializeField]
    private Transform[] idlePoint;
    private Vector2 destination;
    private bool canBeAggresive;
    private bool playerInRange;
    private Transform player;
    
    [SerializeField]
    private float checkRadious;
    [SerializeField]
    private LayerMask whatIsPlayer;

    float defaultSpeed;
    

    protected override void Start()
    {
        base.Start();
        
        // not a good way to do it
        player = GameObject.Find("Player").transform;

        defaultSpeed = speed;

        destination = idlePoint[0].position;
        transform.position = idlePoint[0].position;
    }

    private void Update()
    {
        idleTimeCounter -= Time.deltaTime;
        anim.SetBool("canBeAggresive", canBeAggresive);
        anim.SetFloat("speed", speed);

        if (idleTimeCounter > 0)
        {
            return;
        }

        playerInRange = Physics2D.OverlapCircle(transform.position, checkRadious, whatIsPlayer);

        if (playerInRange && !aggresive && canBeAggresive)
        {
            aggresive = true;
            canBeAggresive = false;
            destination = player.transform.position;
        }

        if (aggresive)
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if(Vector2.Distance(transform.position, destination) < .1f)
            {
                aggresive = false;

                int i = Random.Range(0, idlePoint.Length - 1);
                destination = idlePoint[i].position;

                speed = speed * .5f;
            }
            
        }
        else
        {
            transform.position = Vector2.MoveTowards(transform.position, destination, speed * Time.deltaTime);

            if (Vector2.Distance(transform.position, destination) < .1f)
            {
                if(!canBeAggresive)
                {
                    idleTimeCounter = idleTime;
                    canBeAggresive = true;
                    speed = defaultSpeed;
                }
            }
        }
    }

    protected override void OnDrawGizmos()
    {
        base.OnDrawGizmos();

        Gizmos.DrawWireSphere(transform.position, checkRadious);
    }
}
