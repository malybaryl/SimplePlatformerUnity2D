using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Start_Point : MonoBehaviour
{
    [SerializeField] private Transform _respawnPoint;
    private void Awake()
    {
        Player_Menager.instance.respawnPoint = _respawnPoint;
        Player_Menager.instance.PlayerRespawn();
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.GetComponent<Player>() != null)
        {
            if(collision.transform.position.x > transform.position.x)
                GetComponent<Animator>().SetTrigger("touch");
        }
    }
}

