using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI; // Required for manipulating UI elements
using UnityEngine.SceneManagement; // Required for loading scenes

public class TrophyEndScript : MonoBehaviour
{
    public GameObject endGameScreen; // Assign this in the inspector
    public TextMeshProUGUI scoreText; // Assign this in the inspector

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check if the colliding object is the player
        if (collision.gameObject.CompareTag("Player"))
        {
            // Get the player's score
            int deathCount = collision.gameObject.GetComponent<FG_CharacterController>().gameManager.deathCount;

            // Update the score text
            scoreText.text = "Congratulations!\nNumber of death: " + deathCount;
            // Another text, showing the player's score x 100 divided by the number of deaths
            // We first check if the death count is 0 to avoid division by zero
            if (deathCount == 0)
                scoreText.text += "\nScore: " + (collision.gameObject.GetComponent<FG_CharacterController>().gameManager.score * 100);
            else
                scoreText.text += "\nScore: " + (collision.gameObject.GetComponent<FG_CharacterController>().gameManager.score * 100 / deathCount);

            // Display the end game screen
            endGameScreen.SetActive(true);
        }
    }
}