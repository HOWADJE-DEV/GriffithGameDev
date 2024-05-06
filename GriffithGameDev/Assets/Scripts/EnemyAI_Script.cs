using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAIScript : MonoBehaviour
{
    public enum State
    {
        Idle,
        Patrol,
        DetectPlayer,
        Chasing,
        AggroIdle,
    }

    public State enemyAIState;
    public float moveSpeed;
    public float maxSpeed;
    public float changeSpeed;
    private float speed;
    public float detectedPlayerTime;
    public float aggroTime;
    public bool playerDetected;
    public bool aggro;
    private Rigidbody2D _myRb;
    
    public Transform player;
    
    public float bumpForce_X;
    public float bumpForce_Y;

    public int collisionDamage;
    
    public bool lookingRight = false;
    
    public bool isFloating;
    
    // Start is called before the first frame update
    void Start()
    {
        enemyAIState = State.Idle;
        _myRb = GetComponent<Rigidbody2D>();
        
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        switch (enemyAIState)
        {
            case State.Idle:
                speed = 0;
                break;
            case State.Patrol:
                speed = moveSpeed;
                break;
            case State.DetectPlayer:
                speed = 0;
                break;
            case State.Chasing:
                // Calculate the direction to the player
                Vector2 directionToPlayer = player.position - transform.position;

                // Normalize the direction to get a value between -1 and 1
                Vector2 normalizedDirectionToPlayer = directionToPlayer.normalized;

                // If the enemy is floating, it follows the player in both X and Y axes
                // Otherwise, it only follows the player in the X axis
                Vector2 speedVector = isFloating ? changeSpeed * normalizedDirectionToPlayer : new Vector2(changeSpeed * normalizedDirectionToPlayer.x, _myRb.velocity.y);

                // Apply the speed to the enemy's Rigidbody2D
                _myRb.velocity = speedVector;

                // Check the direction the enemy is looking
                if (normalizedDirectionToPlayer.x > 0 && !lookingRight)
                {
                    Flip();
                }
                else if (normalizedDirectionToPlayer.x < 0 && lookingRight)
                {
                    Flip();
                }
                break;
            case State.AggroIdle:
                speed = 0;
                break;
        }
    }
    
    void Flip()
    {
        // Switch the way the enemy is labelled as facing
        lookingRight = !lookingRight;

        // Multiply the enemy's x local scale by -1
        Vector3 theScale = transform.localScale;
        theScale.x *= -1;
        transform.localScale = theScale;
    }

    public void OnTriggerEnter2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerDetected = true;
            if (aggro == false)
            {
                StopCoroutine("DetectTimer");
                StartCoroutine("DetectTimer");
            }
            if (aggro)
            {
                playerDetected = true;
                enemyAIState = State.Chasing;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            playerDetected = false;
            if (aggro)
            {
                StopCoroutine("AggroTimer");
                StartCoroutine("AggroTimer");
            }
        }
    }
    
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Calculate the direction from the enemy to the player
            Vector2 bumpDirection = collision.transform.position - transform.position;
            bumpDirection = bumpDirection.normalized; // Normalize the direction

            // Apply the bump force to the player
            Rigidbody2D playerRb = collision.gameObject.GetComponent<Rigidbody2D>();
            playerRb.AddForce(new Vector2(bumpDirection.x * bumpForce_X, bumpDirection.y * bumpForce_Y), ForceMode2D.Impulse);

            // Make the player take damage
            FG_CharacterController playerScript = collision.gameObject.GetComponent<FG_CharacterController>();
            if (playerScript != null)
            {
                playerScript.TakeDamage(collisionDamage);
            }
        }
    }

    IEnumerator AggroTimer()
    {
        yield return new WaitForSeconds(aggroTime);
        if (playerDetected == false && aggro == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
        if (playerDetected == false && aggro)
        {
            enemyAIState = State.AggroIdle;
        }
        yield return new WaitForSeconds(aggroTime);
        if (playerDetected == false && aggro == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
    }

    IEnumerator DetectTimer()
    {
        enemyAIState = State.DetectPlayer;
        yield return new WaitForSeconds(detectedPlayerTime);
        if (playerDetected)
        {
            aggro = true;
            enemyAIState = State.Chasing;
        }
        
        if (playerDetected == false)
        {
            aggro = false;
            enemyAIState = State.Idle;
        }
    }
}
