using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombPathController : MonoBehaviour {

    Rigidbody rb;
    TimeStopController TSC;
    private bool stuck;

    public List<Vector3> positions = new List<Vector3>();
    public GameObject myBomb;
    public float timer;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (myBomb == null || stuck)
        {
            Destroy(this.gameObject);
        }

        if (timer >= 0.01667f /128)
        {
            myBomb.GetComponent<BombController>().newPositions.Add(transform.position);
            timer += Time.deltaTime - 0.01667f /128;
        }
        else
        {
            timer += Time.deltaTime;
        }

        rb.AddForce(Physics.gravity);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Floor")
        {
            rb.velocity = Vector3.zero;
            stuck = true;
        }
    }
}
