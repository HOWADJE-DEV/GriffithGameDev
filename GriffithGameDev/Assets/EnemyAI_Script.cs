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
    
    // Start is called before the first frame update
    void Start()
    {
        enemyAIState = State.Idle;
        _myRb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _myRb.velocity = new Vector2(speed, _myRb.velocity.y);

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
                speed = changeSpeed;
                break;
            case State.AggroIdle:
                speed = 0;
                break;
        }
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
