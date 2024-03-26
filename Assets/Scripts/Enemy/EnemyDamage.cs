using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

public class EnemyDamage : NetworkBehaviour
{
    public int damage; //determines how much damage the enemy does

    //allow script to access player scripts
    public PlayerHealth playerHealth;
    public PlayerMovement playerMovement;

    //access enemy script
    public Enemy enemy;

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            enemy.stunDuration = 3; //stun the enemy when it collides with player

            playerMovement.kbCounter = playerMovement.kbTotalTime;  //knockback player
            if (collision.transform.position.x <= transform.position.x)
            {
                playerMovement.kbRight = true;
            }
            if (collision.transform.position.x > transform.position.x)
            {
                playerMovement.kbRight = false;
            }
            playerHealth.TakeDamage(damage);                        //make player take damage
        }
    }
}
