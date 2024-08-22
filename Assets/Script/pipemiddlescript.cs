using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class pipemiddlescript : MonoBehaviour
{
    public logicscript logic;
    private audioscript audioManager;


    void Start()
    {
        logic = GameObject.FindGameObjectWithTag("logic").GetComponent<logicscript>();
        audioManager = GameObject.FindGameObjectWithTag("audio").GetComponent<audioscript>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == 3)
        {
            logic.addScore(1);
            audioManager.Playscore();
        }
    }

}
