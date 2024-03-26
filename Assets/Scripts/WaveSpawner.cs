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
    public float spawnInterval;         //interval between enemy spawns\

}
public class WaveSpawner : NetworkBehaviour
{
    public Wave[] waves;
    public Transform[] spawnPoints;
    public Animator animator;
    public Text waveName;

    //track what wave we are currently on
    private Wave currentWave;
    private int currentWaveNum;
    private float nextSpawnTime;

    private bool canSpawn = true;
    private bool canAnimate = false;

    private void Update()
    {
        currentWave = waves[currentWaveNum];
        SpawnWave();
        GameObject[] totalEnemies = GameObject.FindGameObjectsWithTag("Enemy");

        if (totalEnemies.Length == 0 && currentWaveNum+1 != waves.Length && canAnimate)
        {
            if (currentWaveNum + 1 != waves.Length && canAnimate)
            {
                if (canAnimate) {
                    waveName.text = waves[currentWaveNum + 1].waveName;
                    animator.SetTrigger("WaveComplete");
                    canAnimate = false;
                }
            }
            else
            {
                Debug.Log("GameFinished");
            }
        }
    }
    void SpawnNextWave()
    {
        currentWaveNum++;
        canSpawn = true;
    }

    void SpawnWave()
    {
        if (canSpawn && nextSpawnTime < Time.time)
        {
            GameObject randomEnemy = currentWave.typeOfEnemy[Random.Range(0, currentWave.typeOfEnemy.Length)];
            Transform randomPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
            Instantiate (randomEnemy, randomPoint.position, Quaternion.identity);
            currentWave.nOfEnemies--;   //remove from counter when an enemy spawns
            nextSpawnTime = Time.time + currentWave.spawnInterval;

            //if counter reaches 0, stop enemies from spawning
            if (currentWave.nOfEnemies == 0)
            {
                canSpawn = false;
                canAnimate = true;
            }
        }
    }
}
