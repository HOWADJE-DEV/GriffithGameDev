using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FG_CharacterController : MonoBehaviour
{
    public float maxSpeed;
    public float acceleration;

    public Rigidbody2D myRb;
    public float jumpForce;
    public bool isGrounded;

    public float secondaryJumpForce;
    public float secondaryJumpTime;

    public bool secondaryJump;

    public bool canTakeDamage = true;
    public Animator anim;
    
    public Collider2D groundDetector;
    
    // GameManager object
    public GameManagerScript gameManager;

    // Start is called before the first frame update
    void Start()
    {
        myRb = GetComponent<Rigidbody2D>(); // look for a component called Rigidbody2D and assign it to myRb
        anim = GetComponentInChildren<Animator>();
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        anim.SetFloat("speed", Mathf.Abs(myRb.velocity.x));

        if (Input.GetAxis("Horizontal") > 0)
        {
            anim.transform.localScale = new Vector3(1, 1, 1);
        }
        if (Input.GetAxis("Horizontal") < 0)
        {
            anim.transform.localScale = new Vector3(-1, 1, 1);
        }

        if (Mathf.Abs(Input.GetAxis("Horizontal")) > 0.1f)
        {
            float targetSpeed = Input.GetAxis("Horizontal") * maxSpeed;
            myRb.velocity = new Vector2(Mathf.Lerp(myRb.velocity.x, targetSpeed, acceleration * Time.fixedDeltaTime), myRb.velocity.y);
        }
        else
        {
            myRb.velocity = new Vector2(Mathf.Lerp(myRb.velocity.x, 0, acceleration * Time.fixedDeltaTime), myRb.velocity.y);
        }

        // Clamp the velocity to the maximum speed
        myRb.velocity = new Vector2(Mathf.Clamp(myRb.velocity.x, -maxSpeed, maxSpeed), myRb.velocity.y);

        if (!isGrounded)
        {
            myRb.velocity = new Vector2(myRb.velocity.x, myRb.velocity.y);
        }
    }

    private void Update()
    {
        // Replace isGrounded with a check that uses the ground detector
        if (groundDetector.IsTouchingLayers(LayerMask.GetMask("Ground")) && Input.GetButtonDown("Jump"))
        {
            myRb.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            StartCoroutine(SecondaryJump());
        }

        if (!groundDetector.IsTouchingLayers(LayerMask.GetMask("Ground")) && Input.GetButton("Jump") && secondaryJump == true)
        {
            myRb.AddForce(new Vector2(0, secondaryJumpForce), ForceMode2D.Force);
        }
    }

    private void OnTriggerStay2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other) 
    {
        if (other.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            isGrounded = false;
        }
    }

    IEnumerator SecondaryJump()
    {
        secondaryJump = true;
        yield return new WaitForSeconds(secondaryJumpTime); // wait for a certain amount of time
        secondaryJump = false;
        //yield return null;
    }
    
    public void TakeDamage(int damage)
    {
        if (!canTakeDamage)
            return;
        if (gameManager.health > 0)
            gameManager.health -= damage;
        if (gameManager.health <= 0)
            Die();
        StartCoroutine(DamageCoroutine());
    }
    
    // Coroutine to handle the player's damage (wait 1.5 seconds before taking damage again)
    IEnumerator DamageCoroutine()
    {
        canTakeDamage = false;
        yield return new WaitForSeconds(1.5f);
        canTakeDamage = true;
    }
    
    public void Die()
    {
        // Find the GameManagerScript and move the player to the spawn point
        transform.position = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>().spawnPoint.position;
        gameManager.health = 3;
    }
}
