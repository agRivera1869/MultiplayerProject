using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerMovement : NetworkBehaviour
{
    [Header("Movement")]
    private float horizontal;               //left and right movement
    public int speed;                       //how fast the character walks
    private float jumpingPower = 16f;       //jumping power of character
    private bool isFacingRight = true;      //detect if the player is moving left or right

    [Header("Knockback")]
    public float kbForce;                   //amount of force exerted when a player gets knocked back
    public float kbCounter;                 //countdown the cooldown timer
    public float kbTotalTime;               //timer for the knockback cooldown
    public bool kbRight;                    //detect if player was hit from the right or left

    [Header("References")]
    [SerializeField] private Rigidbody2D rb;            //reference to the players rigidbody component
    [SerializeField] private Transform groundCheck;     //groundcheck variable
    [SerializeField] private LayerMask groundLayer;     //reference to groundLayer
    [SerializeField] private LayerMask enemyLayer;      //reference to enemyLayer

    [Header("Audio and Graphics")]
    private Animator anim;                              //reference for animation controller
    [SerializeField] private AudioClip walk;            //walk audio clip
    [SerializeField] private AudioClip jump;            //jump audio clip
    [SerializeField] private AudioClip attack;          //attack audio clip

    [Header("Attacking")]
    private float timeBetweenAttack;        //cooldown
    public float startTimeBetweenAttack;    //begin cooldown
    public Transform attackPos;     //position of attack circle
    public float attackRange;       //range of the players attack
    public LayerMask detectEnemy;   //layer mask to differentiate enemies and friendly players
    public int damage;              //amount of damage player will do

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) enabled = false;  //check if player is connected to right character instance
    }

    private void Awake()
    {
        anim = GetComponent<Animator>();    //get animator component
    }

    void Update()
    {
        horizontal = Input.GetAxisRaw("Horizontal");                    //get player horizontal input

        //check if  player is able to jump
        if (Input.GetButtonDown("Jump") && IsGrounded())                //jumping function
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpingPower);     //calculate velocity of jump
            SoundManager.instance.PlaySound(jump);                      //play jump sound
        }

        //
        if (Input.GetButtonUp("Jump") && rb.velocity.y > 0f)
        {
            rb.velocity = new Vector2(rb.velocity.x, rb.velocity.y * 0.5f); //formula for jumping power
        }

        Flip();                                         //flip sprite to whatever direction player is facing

        SoundManager.instance.PlaySound(walk);          //play footstep sound
        anim.SetBool("walk", horizontal != 0);          //boolean walk true
        anim.SetBool("grounded", IsGrounded());         //boolean grounded true

        //script for attacking
        if (timeBetweenAttack <= 0)          //check if cooldown is done
        {
            if (Input.GetKey(KeyCode.W))    //get attack key
            {
                anim.SetBool("swing", true);                //boolean attack true
                SoundManager.instance.PlaySound(attack);    //play attack sound
                Debug.Log("attacked");                      //check if attack is working
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, detectEnemy);    //radius of attack
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    GetComponent<Enemy>().TakeDamage(damage);
                }
            }
            timeBetweenAttack = startTimeBetweenAttack;     //begin attack cooldown
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;    //cooldown timer
            anim.SetBool("swing", false);           //boolean swing false
        }

    }

    private void FixedUpdate()
    {
        if (kbCounter <= 0) //if player is not currently being knocked back
        {
            rb.velocity = new Vector2(horizontal * speed, rb.velocity.y);   //allow player to move
        }
        else
        {
            if (kbRight)
            {
                rb.velocity = new Vector2(-kbForce, kbForce);   //move player to the left and up if knocked back from right
            }

            if (!kbRight)
            {
                rb.velocity = new Vector2(kbForce, kbForce);   //move player to the right and up if knocked back from left
            }

            kbCounter -= Time.deltaTime;
        }
    }

    //check if player is grounded
    private bool IsGrounded()
    {
        return Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer);    //circle to detect ground layer
    }

    //flip player sprite direction
    private void Flip()
    {
        if (isFacingRight && horizontal < 0f || !isFacingRight && horizontal > 0f)  //check if left or right
        {
            isFacingRight = !isFacingRight;
            Vector3 localScale = transform.localScale;
            localScale.x *= -1f;
            transform.localScale = localScale;
        }
    }

    //render shape of player attack range in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}
