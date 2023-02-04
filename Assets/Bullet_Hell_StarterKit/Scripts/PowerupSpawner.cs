using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupSpawner : MonoBehaviour
{
    // This script handles the spawning of powerup prefabs

    [Tooltip("Put all potential powerups to spawn here")]
    public List<GameObject> PowerupPrefabs;
    [Tooltip("This is the weight of being chosen to spawn during a spawn cycle(weight of 10 is twice as likely to spawn as 5)")]
    public List<int> PowerupSpawnWeight;
    [Tooltip("How many frames between each spawn cycle")]
    public int FramesBetweenSpawn;
    [Tooltip("Spawn a certain quantity or spawn forever")]
    public bool InfiniteSpawns;
    [Tooltip("How many spawn cycles before it expires")]
    public int SpawnLimit;
    [Tooltip("How long to wait before spawner becomes active(In Frames)")]
    public int InitialSpawnDelay;

   
    int Tick;
    int TimesSpawned;
    int CurrentSpawnDelay = -1;
    // Start is called before the first frame update
    void Start()
    {
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

    //Spawn code in here
    public void BeginSpawnCycle()
    {
        int TotalWeight=0;
        int PickedWeight;
        bool DidWePick=false;
        GameObject SpawnedPowerup;
        //add the weight of all prefabs here
        for (int i = 0; i < PowerupSpawnWeight.Count; i++)
        {
            TotalWeight = TotalWeight + PowerupSpawnWeight[i];
        }
        TotalWeight++;
        //pick a random value
        PickedWeight = Random.Range(0, TotalWeight);
        //checks which prefab is chosen to spawn
        for (int x = 0; x < PowerupSpawnWeight.Count; x++)
        {
            if (PickedWeight < (PowerupSpawnWeight[x] + 1) && DidWePick == false)
            {
                DidWePick = true;
                //pickedthisone
                SpawnedPowerup = Instantiate(PowerupPrefabs[x], this.gameObject.transform);
                TimesSpawned++;
            }
            PickedWeight = PickedWeight - PowerupSpawnWeight[x];
        }
    }

}
