using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathEnd : MonoBehaviour {

	// Use this for initialization
	void Start ()
    {
        transform.GetChild(0).GetComponent<MeshRenderer>().material = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        transform.GetChild(1).GetComponent<MeshRenderer>().material = new Material(transform.GetChild(0).GetComponent<MeshRenderer>().material);
        transform.GetChild(0).GetComponent<MeshRenderer>().material.SetFloat("_IsDoor", 1);
        transform.GetChild(1).GetComponent<MeshRenderer>().material.SetFloat("_IsDoor", 1);
    }
	
	// Update is called once per frame
	void Update ()
    {
		
	}
}
