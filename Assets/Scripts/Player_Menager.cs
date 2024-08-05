using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Menager : MonoBehaviour
{
    public static Player_Menager instance; 
    public Transform respawnPoint;
   
    [SerializeField] private GameObject playerPrefab;
    public GameObject currentPlayer;
    
    private void Awake()
    {
        instance = this;
        PlayerRespawn();
    }
    
    public void PlayerRespawn()
    {
        if (currentPlayer == null)
            currentPlayer = Instantiate(playerPrefab, respawnPoint.position, transform.rotation);
    }

    
}
