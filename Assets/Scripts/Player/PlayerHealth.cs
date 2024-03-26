using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class PlayerHealth : NetworkBehaviour
{
    public int maxHealth = 10;  //initialize the max health a player can be at
    public int health;          //variable to track current health
    void Start()
    {
        health = maxHealth;     //set health to full
    }

    public void TakeDamage(int damage)  //method for when a player takes damage
    {  
        health -= damage;      //decrease player health by enemy's damage
        if (health <= 0)       //if player health reaches 0
        {
            transform.position = new Vector3(0, -100, 0);   //transform away from scene
            //DestroyImmediate(gameObject, true);           //[TRY TO FIX] destroy game object
        }
    }
}
