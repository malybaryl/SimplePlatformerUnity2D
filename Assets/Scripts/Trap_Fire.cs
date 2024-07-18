using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Fire : DamagingObject
{
    public bool isWorking;
    private Animator anim;

    private void Start()
    {
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        anim.SetBool("isWorking", isWorking);
    }
    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (isWorking) 
        {
            base.OnTriggerEnter2D(collision);
        }
    }

    public void FireSwitch()
    {
        isWorking = !isWorking;
    }
}
