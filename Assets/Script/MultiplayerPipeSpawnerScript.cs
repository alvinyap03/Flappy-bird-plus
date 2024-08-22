using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;

public class MultiplayerPipeSpawnerScript : MonoBehaviourPun
{
    public GameObject pipe;
    public float spawnRate = 2f;
    private float timer = 0f;
    public float heightRandom = 10f;

    void Start()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            SpawnPipe();
        }
    }

    void Update()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            if (timer < spawnRate)
            {
                timer += Time.deltaTime;
            }
            else
            {
                SpawnPipe();
                timer = 0f;
            }
        }
    }

    void SpawnPipe()
    {
        float lowerPoint = transform.position.y - heightRandom;
        float higherPoint = transform.position.y + heightRandom;
        Vector3 spawnPosition = new Vector3(transform.position.x, Random.Range(lowerPoint, higherPoint), 0);

        GameObject newPipe = PhotonNetwork.Instantiate(pipe.name, spawnPosition, transform.rotation);
    }
}
