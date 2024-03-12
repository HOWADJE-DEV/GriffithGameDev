using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UI_Script : MonoBehaviour
{
    public TMP_Text scoreText;
    public TMP_Text healthText;
    
    public GameManagerScript gameManager;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }


    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score: " + gameManager.score;
        healthText.text = "Health: " + gameManager.health;
    }
}
