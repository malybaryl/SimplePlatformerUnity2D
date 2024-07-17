using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Spiked_Ball : Trap
{
    private Rigidbody2D rb;
    [SerializeField] private Vector2 pushDirection;
    [SerializeField] private float timeToPushAgain;
    private float time = Time.time;
    // Start is called before the first frame update
    void Start()
    {
        rb= GetComponent<Rigidbody2D>();
        rb.AddForce(pushDirection, ForceMode2D.Impulse);
    }

    // Update is called once per frame
    void Update()
    {
        if (Time.time - time >= timeToPushAgain)
        {
            rb.AddForce(pushDirection, ForceMode2D.Impulse);
            time = Time.time;
        }
    }
}
