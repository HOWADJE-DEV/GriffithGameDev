using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CheckPointScript : MonoBehaviour
{
    AudioManagerScript audioManager;
    // Start is called before the first frame update
    void Start()
    {
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().spawnPoint = transform;
            gameObject.GetComponent<Animator>().SetTrigger("CheckPointTriggered");
            // Play the checkpoint sound
            audioManager.PlaySFX(audioManager.checkpoint);
        }
    }
}
