using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public int health;                  //enemy health
    public float speed;                 //enemy speed
    public float stunDuration;          //total stun time when an enemy is attacked
    public float startStun;             //cooldown for stun

    public Transform playerTransform;   //detect where player is located
    public bool isChasing;              //is enemy currently chasing?
    public float chaseDistance;         //how far the player can be before enemy begins chase

    private Animator anim;              //attach animation controller

    void Start()
    {
        //anim = GetComponent<Animator>();    //start animator
        //anim.SetBool("isRunning", true);    //set isRunning true
    }

    void Update()
    {
        if (isChasing)
        {
            //determine if enemy should be facing left or right if chasing player
            //player is to enemy's right
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * speed * Time.deltaTime;
            }

            //player is to enemy's left
            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }
        else
        {
            //if player is within our determined chaseDistance, chase the player
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
        }

        //when enemy takes damage
        if (stunDuration <= 0)      //check if stun is still in effect
        {
            speed = 5;              //enemy will maintain normal speed
        }
        else
        {
            speed = 0;              //set enemy speed to 0 if stunned
            stunDuration -= Time.deltaTime; //start stun cooldown
        }
        if(health <= 0)
        {
            anim.SetBool("dead", true);
            DestroyImmediate(gameObject, true);    //destroy enemy when it's health is gone
        }


    }

    public void TakeDamage(int damage)  //method for enemy to take damage
    {
        anim.SetBool("damaged", true);  //set off damage animation
        stunDuration = startStun;       //reset stun duration
        health -= damage;               //decrease health when player attacks
        Debug.Log("damage taken");      //check if enemy is successfully taking damage
    }
}
