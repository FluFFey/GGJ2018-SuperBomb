﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunController : MonoBehaviour {

    public GameObject transPrefab;
    public GameObject bombPrefab;
    public GameObject bombPatherPrefab;
    public GameObject gunMuzzle;
    public GameObject bombSpawn;

    public float transCD;
    private float currentTransCD;
    public float bombCD;
    private float currentBombCD;

    public float transShootForce;
    public float bombThrowForce;

    public GameObject lineDot;
    public int predictSteps;

    private TimeStopController TSC;

    // Use this for initialization
    void Start () {
        TSC = TimeStopController.instance.GetComponent<TimeStopController>();
    }
	
	// Update is called once per frame
	void Update () {
        if(currentTransCD > 0)
        {
            currentTransCD -= TimeStopController.deltaTime();
        }
        if (currentBombCD > 0)
        {
            currentBombCD -= TimeStopController.deltaTime();
        }
    }

    public bool shootTrans()
    {
        if(currentTransCD <= 0)
        {
            //shoot
            GameObject trans = Instantiate(transPrefab, gunMuzzle.transform.position, Quaternion.identity);
            trans.transform.rotation = gunMuzzle.transform.rotation;
            //trans.GetComponent<Rigidbody>().AddForce(gunMuzzle.transform.forward * transShootForce);
            trans.GetComponent<TransController>().startVal = transform.parent.parent.parent.transform.forward;
            currentTransCD = transCD;
            //timeSkip(0.5f, 10);
            return true;
        }
        return false;
    }

    public bool throwBomb(Animator anim)
    {
        if (currentBombCD <= 0)
        {
            //throw
            GameObject bomb = Instantiate(bombPrefab, bombSpawn.transform.position, Quaternion.identity);
            GameObject bombPather = Instantiate(bombPatherPrefab, bombSpawn.transform.position, Quaternion.identity);
            bombPather.GetComponent<BombPathController>().myBomb = bomb;
            bomb.transform.GetChild(0).GetComponent<Animator>().SetBool("bombOpen", true);

            bombPather.GetComponent<Rigidbody>().AddForce(bombSpawn.transform.forward * bombThrowForce);
            currentBombCD = bombCD;
            //timeSkip(0.5f, 10);
            anim.Play("BombThrow");
            return true;
        }
        return false;
    }

    public void bombPrediction()
    {
        destroyDots();

        print(bombSpawn.transform.forward);
        Vector3 veloc = bombSpawn.transform.forward * bombThrowForce /50;
        Vector3[] plots = Plot(bombPrefab.GetComponent<Rigidbody>(), bombSpawn.transform.position, veloc, predictSteps);
        foreach(Vector3 v3 in plots)
        {
            Instantiate(lineDot, v3, bombSpawn.transform.rotation);
        }
    }

    private void timeSkip(float targetScale, int lerpSpeed)
    {
        TSC.setTimeScale(Mathf.Lerp(TimeStopController.timeScale, targetScale, Time.deltaTime * lerpSpeed));
    }

    public void destroyDots()
    {
        GameObject[] dots = GameObject.FindGameObjectsWithTag("LineDot");
        foreach (GameObject go in dots)
        {
            Destroy(go);
        }
    }

    public static Vector3[] Plot(Rigidbody rigidbody, Vector3 pos, Vector3 velocity, int steps)
    {
        print("predicting!");
        Vector3[] results = new Vector3[steps];

        float timestep = Time.fixedDeltaTime / Physics.defaultSolverVelocityIterations;
        Vector3 gravityAccel = Physics.gravity * timestep * timestep;
        float drag = 1f - timestep * rigidbody.drag;
        Vector3 moveStep = velocity * timestep;

        for (int i = 0; i < steps; ++i)
        {
            moveStep += gravityAccel;
            moveStep *= drag;
            pos += moveStep;
            results[i] = pos;
        }
        return results;
    }
}
