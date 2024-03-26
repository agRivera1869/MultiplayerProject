using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Unity.Netcode;
[System.Serializable]
public class Wave
{
    public string waveName;             //name of the current wave
    public int nOfEnemies;              //how many types of enemies the waves will spawn
    public GameObject[] typeOfEnemy;    //array for different enemy types
    public float spawnInterval;         //interval between enemy spawns

}
public class WaveSpawner : NetworkBehaviour
{
    public Wave[] waves;                //array for different waves
    public Transform[] spawnPoints;     //array for enemy spawn points
    public Animator animator;           //animator for wave text
    public Text waveName;               //variable for name of wave

    //track what wave we are currently on
    private Wave currentWave;
    private int currentWaveNum;
    private float nextSpawnTime;

    private bool canSpawn = true;       //can enemies spawn?
    private bool canAnimate = false;

    private void Update()
    {
        currentWave = waves[currentWaveNum];
        SpawnWave();    //call method to begin the wave
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy"); //track how many enemies are in the scene

        //if wave is over
        if (totalEnemies.Length == 0 && currentWaveNum+1 != waves.Length && canAnimate)
        {
            if (currentWaveNum + 1 != waves.Length && canAnimate)
            {
                //UI for starting a wave, displays wave name and a countdown 
                if (canAnimate) {
                    waveName.text = waves[currentWaveNum + 1].waveName; 
                    animator.SetTrigger("WaveComplete");                
                    canAnimate = false;
                }
            }
            else
            {
                Debug.Log("GameFinished");  //debug for if all waves are cleared
            }
        }
    }
    void SpawnNextWave()
    {
        currentWaveNum++;   //once wave is over, increment wave number
        canSpawn = true;    //allow enemies to spawn again
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemy[Random.Range(0, currentWave.typeOfEnemy.Length)];  //create a random enemy
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];   //set a random spawn point from our array
            Instantiate (randomEnemy, randomPoint.position, Quaternion.identity);       //instantiate the next enemy
            currentWave.nOfEnemies--;                                   //remove from counter when an enemy spawns
            nextSpawnTime = Time.time + currentWave.spawnInterval;      //make enemies spawn at intervals

            //if counter reaches 0, stop enemies from spawning
            if (currentWave.nOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }
}
