using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyDamage : NetworkBehaviour
{
    public int damage; //determines how much damage the enemy does

    [Header ("Access Scripts")]
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;
    public Enemy enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")   //if enemy collides with a player
        {
            enemy.stunDuration = 3; //stun the enemy

            //knockback player
            playerMovement.kbCounter = playerMovement.kbTotalTime;
            //detect if collision was from left or right

            //if from left, knock player right
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.kbRight = true;
            }

            //if from right, knock player left
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.kbRight = false;
            }
            playerHealth.TakeDamage(damage);                        //call player's take damage method
        }
    }
}
