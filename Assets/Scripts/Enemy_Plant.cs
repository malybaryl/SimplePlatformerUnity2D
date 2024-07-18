using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    protected override void Start()
    {
        base.Start();
    }

    void Update()
    {
        CollisionsCheck();
        idleTimeCounter -= Time.deltaTime;
        if( idleTimeCounter < 0 && playerDetected)
        {
            idleTimeCounter = idleTime;
            anim.SetTrigger("attack");
        }    
    }

    private void AttackEvent()
    {
        Debug.Log("Attack");
    }
}
