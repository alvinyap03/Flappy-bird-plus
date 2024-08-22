using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.SceneManagement;

public class MultiplayerManager : MonoBehaviourPunCallbacks
{
    void Start()
    {
        PhotonNetwork.ConnectUsingSettings();
    }

    public override void OnConnectedToMaster()
    {
        PhotonNetwork.JoinLobby();
    }

    public override void OnJoinedLobby()
    {
        JoinOrCreateRoom();
    }

    void JoinOrCreateRoom()
    {
        RoomOptions roomOptions = new RoomOptions { MaxPlayers = 2 };
        PhotonNetwork.JoinRandomOrCreateRoom(roomOptions: roomOptions);
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        // If failed to join a random room, create a new room
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }

    public override void OnJoinedRoom()
    {
        CheckRoomStatus();
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        CheckRoomStatus();
    }

    void CheckRoomStatus()
    {
        // Check if the room has 2 players
        if (PhotonNetwork.CurrentRoom.PlayerCount == 2)
        {
            // Load the multiplayer scene
            PhotonNetwork.LoadLevel(3); // Change to the multiplayer scene using index 3
        }
    }
}