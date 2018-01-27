using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public int exRadius;

    private Rigidbody rb;
    private bool stuck;
    public List<Vector3> prevVelocities = new List<Vector3>();

    public TimeStopController TSC;

    public List<Vector3> newPositions = new List<Vector3>();
    public float timer;

    // Use this for initialization
    void Start () {
        rb = GetComponent<Rigidbody>();
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
    }

    // Update is called once per frame
    void Update () {
        //each frame, set my position to 
        if (timer >= 0.01667f)
        {
            if(newPositions.Count > 0)
            {
                transform.position = newPositions[0];
                newPositions.RemoveAt(0);
            }
            timer += TimeStopController.deltaTime() - 0.01667f;
        }
        else
        {
            timer += TimeStopController.deltaTime();
        }

        if (stuck)
        {
            rb.velocity = Vector3.zero;
            rb.useGravity = true;
        }
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
