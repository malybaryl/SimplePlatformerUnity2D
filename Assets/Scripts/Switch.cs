using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Switch : Trap
{
    private Animator anim;

    public Trap_Fire trapFire;
    public float cooldown = 5f;
    private bool isWorking = false;


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
        if (collision.tag == "Player")
        {
            if (!isWorking)
            {
                FireSwitch();
                isWorking = true;
                Invoke("FireSwitch", cooldown);
            }
        }
    }

    private void FireSwitch()
    {
        trapFire.FireSwitch();
        isWorking = false;
    }
}
