using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupScript : MonoBehaviour
{
    public float scoreValue;
    public GameManagerScript gameManager;

    public GameObject collectedEffect;
    AudioManagerScript audioManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        audioManager = GameObject.FindGameObjectWithTag("Audio").GetComponent<AudioManagerScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            gameManager.AddScore(scoreValue);
            audioManager.PlaySFX(audioManager.bonus);
            Instantiate(collectedEffect, transform.position, transform.rotation);
            Destroy(gameObject);
        }
    }
}
