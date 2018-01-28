using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleSimulator : MonoBehaviour {

    ParticleSystem ps;
	// Use this for initialization
	void Start ()
    {
        ps = GetComponent<ParticleSystem>();
	}

    // Update is called once per frame
    void Update()
    {
        //Time.deltaTime* TimeStopController.timeScale == TimeStopController.instance.deltaTimeScale;
        ps.Simulate(TimeStopController.deltaTime(), true, false);
    }
}
