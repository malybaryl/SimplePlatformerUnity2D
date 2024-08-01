using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_Level_3 : MonoBehaviour
{
    [SerializeField] private GameObject myCamera;
    [SerializeField] private bool activeByStart = false;


    private void Start()
    {
        if (activeByStart != true)
        {
            myCamera.SetActive(false);
        }
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            myCamera.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Player>() != null)
        {
            myCamera.SetActive(false);
        }
    }
}
