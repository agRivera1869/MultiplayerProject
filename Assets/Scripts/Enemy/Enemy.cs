using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class Enemy : NetworkBehaviour
{
    public int health;              //enemy health
    public float speed;             //enemy speed
    public float stunDuration;     //total stun time when an enemy is attacked
    public float startStun;         //cooldown for stun

    public Transform playerTransform;
    public bool isChasing;
    public float chaseDistance;

    private Animator anim;

    void Start()
    {
        //anim = GetComponent<Animator>();    //start animator
        //anim.SetBool("isRunning", true);    //set isRunning true
    }

    void Update()
    {
        if (isChasing)
        {
            if (transform.position.x > playerTransform.position.x)
            {
                transform.localScale = new Vector3(1, 1, 1);
                transform.position += Vector3.left * speed * Time.deltaTime;
            }

            if (transform.position.x < playerTransform.position.x)
            {
                transform.localScale = new Vector3(-1, 1, 1);
                transform.position += Vector3.right * speed * Time.deltaTime;
            }
        }
        else
        {
            if (Vector2.Distance(transform.position, playerTransform.position) < chaseDistance)
            {
                isChasing = true;
            }
        }

        //When enemy takes damage
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
        anim.SetBool("damaged", true);
        stunDuration = startStun;
        health -= damage;           //decrease health when player attacks
        Debug.Log("damage taken");  //check if enemy is successfully taking damage
    }
}
