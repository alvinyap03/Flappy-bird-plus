using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipespawnsscript : MonoBehaviour
{
    public GameObject pipe;
    public float spawnrate = 2;
    private float timer = 0;
    public float heightrandom = 10;

    // Start is called before the first frame update
    void Start()
    {
        spawnPipe();
    }

    // Update is called once per frame
    void Update()
    {
        if (timer < spawnrate)
        {
            timer = timer + Time.deltaTime;
        }
        else
        {
            spawnPipe();
            timer = 0;
        }
        
    }

    void spawnPipe()
    {
        float lowerPoint = transform.position.y - heightrandom;
        float higherPoint = transform.position.y + heightrandom;

        Instantiate(pipe, new Vector3(transform.position.x, Random.Range(lowerPoint, higherPoint), 0), transform.rotation);
    }
}
