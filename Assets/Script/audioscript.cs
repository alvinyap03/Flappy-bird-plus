using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class audioscript : MonoBehaviour
{
    public AudioSource SFXSource;

    public AudioClip score;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Playscore()
    {
        SFXSource.clip = score;
        SFXSource.Play();
    }
}
