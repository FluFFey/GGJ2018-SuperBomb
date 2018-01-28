using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour {

    public float spawnInterval;
    public float enemyInterval;
    float timeSinceLastSpawn = 0.0f;
    float timeSinceLastEnemySpawn = 0.0f;
    public GameObject enemGO;
    public GameObject attackerGO;
    // Use this for initialization
    void Start ()
    {
        timeSinceLastSpawn = Random.Range(-2.0f, 2.0f);  //set it manipulate first spawn
        timeSinceLastEnemySpawn = Random.Range(-10.0f, -4.0f); //set it manipulate first spawn
        transform.GetChild(0).GetComponent<MeshRenderer>().material = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        transform.GetChild(1).GetComponent<MeshRenderer>().material = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        //GetComponentInChildren<MeshRenderer>().material = new Material(GetComponentInChildren<MeshRenderer>().material);
        //print(GetComponentInChildren<MeshRenderer>().gameObject);
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_IsDoor", 1);
        transform.GetChild(1).GetComponent<MeshRenderer>().material.SetFloat("_IsDoor", 1);

    }

    // Update is called once per frame
    void Update()
    {
        timeSinceLastSpawn += TimeStopController.deltaTime();
        if (timeSinceLastSpawn > spawnInterval)
        {
            timeSinceLastSpawn = 0.0f;
            if (spawnInterval > 1.2f)
            {
                spawnInterval *= 0.97f;
            }
            Instantiate(enemGO, transform).transform.localScale = new Vector3(2, 1, 2);//new Vector3(2 * 1.4f, 1 * 1.4f, 2 * 1.4f);

            //GameObject newGo  = Instantiate(enemGO, transform.position, Quaternion.identity);
            //newGo.transform.position = new Vector3(newGo.transform.position.x, 1.4f, newGo.transform.position.z);
        }

        if (timeSinceLastEnemySpawn > enemyInterval)
        {
            timeSinceLastEnemySpawn = 0.0f;
            if (enemyInterval > 1.4f)
            {
                enemyInterval *= 0.97f;
            }
            Instantiate(attackerGO, transform).transform.localScale = new Vector3(2, 1, 2); //new Vector3(2 * 1.5f, 1 * 1.5f, 2 * 1.5f);
            //GameObject newGo = Instantiate(attackerGO, transform.position, Quaternion.identity);
            //    newGo.transform.position = new Vector3(newGo.transform.position.x, 1.4f, newGo.transform.position.z);
        }
    }
}
