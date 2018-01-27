﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnInterval;
    float timeSinceLastSpawn = 0.0f;
    public GameObject enemGO;
	// Use this for initialization
	void Start ()
    {
        timeSinceLastSpawn = 4.0f; //set it manipulate first spawn
        transform.GetChild(0).GetComponent<MeshRenderer>().material = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        //GetComponentInChildren<MeshRenderer>().material = new Material(GetComponentInChildren<MeshRenderer>().material);
        //print(GetComponentInChildren<MeshRenderer>().gameObject);
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_IsDoor", 1);

    }
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += TimeStopController.deltaTime();
        if (timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0.0f;
            if (spawnInterval > 1.2f)
            { 
                spawnInterval *= 0.97f;
            }
            Instantiate(enemGO, transform).transform.localScale = new Vector3(2, 1, 2);
        }
	}
}
