using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public int exRadius;

    private Rigidbody rb;
    private bool stuck;
    private List<Vector3> prevVelocities = new List<Vector3>();

    public TimeStopController TSC;

	// Use this for initialization
	void Start () {
        rb = GetComponent<Rigidbody>();
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
    }
	
	// Update is called once per frame
	void Update () {
        if (stuck)
        {
            rb.velocity = Vector3.zero;
        }

        rb.AddForce(Physics.gravity * TSC.getTimeScale()); //Instead of gravity

        if(TSC.isRising())
        {
            prevVelocities.Add(rb.velocity);
        }
        else
        {
            print("rising?");
            if(prevVelocities.Count > 0)
            {
                rb.velocity = prevVelocities[prevVelocities.Count - 1];
                prevVelocities.RemoveAt(prevVelocities.Count - 1);
            }
        }

        rb.velocity *= TSC.getTimeScale(); //Stop mid air
    }

    private void OnCollisionEnter(Collision other)
    {
        if(other.gameObject.tag == "Floor")
        {
            rb.velocity = Vector3.zero;
            stuck = true;
        }
    }

    public void recieveTrans()
    {
        print("Recieved!");
        //Run explosion animation
        Collider[] cols = Physics.OverlapSphere(transform.position, exRadius);
        foreach (Collider col in cols)
        {
            if (col && col.tag == "Enemy") // if object has the right tag...
            {
                //EnemyController EC = col.GetComponent<EnemyController>();
                //EC.dealDmg(1); //Deal 1 dmg, assuming regular enemies have 1hp, bigger have 2
                Destroy(col.gameObject);
            }
        }
        Destroy(this.gameObject);
    }
}
