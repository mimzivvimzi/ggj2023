using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    // This script handles the spawning of enemy prefabs

    [Tooltip("Prefab of enemy to spawn")]
    public GameObject ObjectToSpawn;
    [Tooltip("How long to wait before spawner becomes active(In Frames)")]
    public int InitialSpawnDelay;
    [Tooltip("How many frames between each spawn cycle")]
    public int FramesBetweenSpawn;
    [Tooltip("Spawn a certain quantity or spawn forever")]
    public bool InfiniteSpawns;
    [Tooltip("How many spawn cycles before it expires")]
    public int SpawnLimit;
    [Tooltip("How many enemies to spawn each spawn cycle")]
    public int SpawnQuantity;
    [Tooltip("Use this to allow the enemy to spawn in a random X position on the screen")]
    public bool RandomXPosMode;
    [Tooltip("Lower X position limit for random spawn position")]
    public float XPosLimitLower = -8.5f;
    [Tooltip("Upper X position limit for random spawn position")]
    public float XPosLimitUpper = 8.5f;
    
    public GameObject PlayerObject;
    int Tick;
    int TimesSpawned;
    int CurrentSpawnDelay = -1;
    float XPos;



    // Start is called before the first frame update
    void Start()
    {
        PlayerObject = GameObject.Find("Player");
        if (InfiniteSpawns == true)
        {
            SpawnLimit = 99999;
        }
    }

    // Update is called once per frame
    private void FixedUpdate()
    {
            Tick++;
            if (CurrentSpawnDelay < InitialSpawnDelay)
            {
                //Tick initial spawn delay
                CurrentSpawnDelay++;
                //check if initial spawn delay is met
                if (CurrentSpawnDelay == InitialSpawnDelay)
                {
                    Tick = FramesBetweenSpawn;
                    CurrentSpawnDelay++;
                }
            }

            //If all spawn cycle conditions are met, begin spawncycle
            if (Tick == FramesBetweenSpawn && TimesSpawned < SpawnLimit && CurrentSpawnDelay > InitialSpawnDelay)
            {
                BeginSpawnCycle();
                Tick = 0;
            }
        }

    //spawn cycle code in here
    void BeginSpawnCycle()
    {
        GameObject SpawnedObject;
        for (int i = 0; i < SpawnQuantity; i++)
        {
            //instantiate enemy object
            SpawnedObject = Instantiate(ObjectToSpawn, this.gameObject.transform);
            TimesSpawned++;
            if(RandomXPosMode==true)
            {
                XPos = Random.Range(XPosLimitLower, XPosLimitUpper);
                SpawnedObject.transform.localPosition = new Vector3(XPos, 0, 0);
            }
        }
        //disable spawner when spawnlimit is reached
        if (TimesSpawned == SpawnLimit)
        {
            this.gameObject.GetComponent<EnemySpawner>().enabled = false;
        }

    }

    //Call This to reset spawner to original parameters
    void ResetSpawner()
    {
        TimesSpawned = 0;
        Tick = 0;
        CurrentSpawnDelay = -1;
    }

}
