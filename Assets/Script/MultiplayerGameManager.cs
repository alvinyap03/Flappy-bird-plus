using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI; // Add this to use UI elements

public class MultiplayerGameManager : MonoBehaviourPunCallbacks
{
    public GameObject redBirdPrefab;
    public GameObject blueBirdPrefab;
    public Text playerColorText; // UI Text to display player color

    void Start()
    {
        SpawnPlayer();
    }

    void SpawnPlayer()
    {
        GameObject playerBirdPrefab = PhotonNetwork.IsMasterClient ? redBirdPrefab : blueBirdPrefab;
        Vector3 spawnPosition = new Vector3(0, 0, 0); // Adjust as needed
        PhotonNetwork.Instantiate(playerBirdPrefab.name, spawnPosition, Quaternion.identity);

        // Update the UI to display the correct color
        if (PhotonNetwork.IsMasterClient)
        {
            playerColorText.text = "Red Bird";
            playerColorText.color = Color.red; // Optional: Change text color to red
        }
        else
        {
            playerColorText.text = "Blue Bird";
            playerColorText.color = Color.blue; // Optional: Change text color to blue
        }
    }
}
