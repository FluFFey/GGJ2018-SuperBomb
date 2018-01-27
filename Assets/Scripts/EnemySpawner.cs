using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnInterval;
    float timeSinceLastSpawn = 0.0f;
    public GameObject enemGO;
	// Use this for initialization
	void Start ()
    {
        timeSinceLastSpawn = -2.0f; //set it to negative to delay first spawns
	}
	
	// Update is called once per frame
	void Update () {
        timeSinceLastSpawn += Time.deltaTime;
        if (timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0.0f;
            if (spawnInterval > 1.0f)
            { 
                spawnInterval *= 0.95f;
            }
            Instantiate(enemGO, transform).transform.localScale = new Vector3(2, 1, 2);
        }
	}
}
