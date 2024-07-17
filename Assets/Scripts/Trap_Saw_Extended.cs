using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Trap_Saw_Extended : Trap
{
    private Animator anim;

    [SerializeField] private Transform[] movePoint;
    [SerializeField] private float speed;
    [SerializeField] private bool revarseBack = false;
    [SerializeField] private bool revarseBackFliping = false;

    private int movePointIndex = 0;
    private bool goingForward = true; 


    void Start()
    {
        anim = GetComponent<Animator>();
        transform.position = movePoint[0].position;
    }

    // Update is called once per frame
    void Update()
    {
      
        anim.SetBool("isWorking", true);

        
        
        transform.position = Vector3.MoveTowards(transform.position, movePoint[movePointIndex].position, speed * Time.deltaTime);
        if (! revarseBack)
        {
           if (Vector3.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
            { 
                movePointIndex++;

                if (movePointIndex >= movePoint.Length)
                {
                    movePointIndex = 0;
                }
            }
        }
        else
        {
            if (Vector3.Distance(transform.position, movePoint[movePointIndex].position) < 0.15f)
            {
                if (goingForward)
                {
                    movePointIndex++;

                    if (movePointIndex >= movePoint.Length)
                    {
                        if (revarseBackFliping)
                        {
                            Flip();
                        }
                        movePointIndex = movePoint.Length - 1;
                        goingForward = false;
                    }

                }
                else
                {
                    movePointIndex--;

                    if (movePointIndex < 0)
                    {
                        if (revarseBackFliping)
                        {
                            Flip();
                        }
                        movePointIndex = 0;
                        goingForward = true;
                    }
                }
            }
        }
        
    }

    private void Flip()
    {
        transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y * -1);
    }

}
