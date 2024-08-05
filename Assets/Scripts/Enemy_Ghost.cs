using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Ghost : Enemy
{
    [Header("Ghost specifics")]
    [SerializeField] private float activeTime;
    private float activeTimeCounter;

 
    private SpriteRenderer sr;

    protected override void Start()
    {
        base.Start();
        sr = GetComponent<SpriteRenderer>();
   
        activeTimeCounter = activeTime;
        aggresive = false;
        invincible = true;
    }

    void Update()
    {
        if (player == null)
        {
            anim.SetTrigger("desappear");
            return;
        }

        activeTimeCounter -= Time.deltaTime;
        idleTimeCounter -= Time.deltaTime;

        if (activeTimeCounter > 0)
        {
            transform.position = Vector2.MoveTowards(transform.position, player.transform.position, speed * Time.deltaTime);
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && aggresive)
        {
            anim.SetTrigger("disappear");
            aggresive = false;
            idleTimeCounter = idleTime;
        }

        if (activeTimeCounter < 0 && idleTimeCounter < 0 && !aggresive)
        {
            ChoosePosition();
            anim.SetTrigger("appear");
            aggresive = true;
            activeTimeCounter = activeTime;
        }

        FlipController();

    }

    private void FlipController()
    {
        if (player == null)
        {
            return;
        }

        if (facingDirection == -1 && transform.position.x < player.transform.position.x)
        {
            Flip();
        }
        else if (facingDirection == 1 && transform.position.x > player.transform.position.x)
        {
            Flip();
        }
    }

    private void ChoosePosition()
    {
        float yOffset = Random.Range(-7, 7);
        float[] xOffsetValues = { -7, -6, -5, -4, 4, 5, 6, 7 };
        float xOffset = xOffsetValues[Random.Range(0,xOffsetValues.Length)];

        transform.position = new Vector2(player.transform.position.x + xOffset, player.transform.position.y + yOffset);
    }
    public override void Damage()
    {
        if(aggresive)
            base.Damage();
    }

    public void Desappear()
    {
        //sr.enabled = false;

        // other method

        sr.color = Color.clear;
    }

    public void Appear()
    {
        //sr.enabled = true;

        // other method

        sr.color = Color.white;
    }

    protected override void OnTriggerEnter2D(Collider2D collision)
    {
        if (aggresive)
            base.OnTriggerEnter2D(collision);
    }
}
