using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickupSpawner : MonoBehaviour {

    public float pickUpRespawnTime;
    float timeSinceLastSpawn = 10;
    public GameObject bombPickup;
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += TimeStopController.deltaTime();

        if (timeSinceLastSpawn > pickUpRespawnTime)
        {
            spawnPickup();
            pickUpRespawnTime *= 0.9f;
            timeSinceLastSpawn = 0;
        }
	}

    private void spawnPickup()
    {
        Vector3 spawnPos = new Vector3(
            UnityEngine.Random.Range(5, 60),
            1.2f,
            UnityEngine.Random.Range(20, 55));
        Instantiate(bombPickup).transform.position = spawnPos;
    }
}
