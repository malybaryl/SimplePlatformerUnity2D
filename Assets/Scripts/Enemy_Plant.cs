using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy_Plant : Enemy
{
    [Header("Plant specifics")]
    [SerializeField]
    private GameObject bulletPrefab;
    [SerializeField]
    private Transform bulletTransform;
    [SerializeField]
    private float bulletSpeed = 10f;
    [SerializeField]
    private bool facingRight = false;

    private GameObject newBullet = null;
    protected override void Start()
    {
        base.Start();
        if (facingRight)
            Flip();
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
        GameObject newBullet = Instantiate(bulletPrefab, bulletTransform.transform.position, bulletTransform.transform.rotation);

        newBullet.GetComponent<Bullet>().SetupSpeed(bulletSpeed * facingDirection, 0);
    }

}
