/*using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Netcode;

//public class PlayerAttack : NetworkBehaviour
{
    //variables to time attacks
    private float timeBetweenAttack;        //cooldown
    public float startTimeBetweenAttack;    //begin cooldown

    //variables for attacking
    public Transform attackPos; 
    public float attackRange;       //range of the players attack
    public LayerMask detectEnemy;   //layer mask to differentiate enemies and friendly players
    public int damage;


    

    void Update()
    {
        if (timeBetweenAttack <=0)          //check if cooldown is done
        {
            if (Input.GetKey(KeyCode.W))    //get attack key
            {
                playerMovement.isAttacking();
                Debug.Log("attacked");  //check if attack is working
                Collider2D[] enemiesToDamage = Physics2D.OverlapCircleAll(attackPos.position, attackRange, detectEnemy);    //radius of attack
                for (int i = 0; i < enemiesToDamage.Length; i++)
                {
                    enemiesToDamage[i].GetComponent<Enemy>().TakeDamage(damage);
                }   
            }
            timeBetweenAttack = startTimeBetweenAttack;     //begin cooldown
        }
        else
        {
            timeBetweenAttack -= Time.deltaTime;    //cooldown timer
        }
    }

    //render shape of player attack range in scene view
    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(attackPos.position, attackRange);
    }

}//
*/