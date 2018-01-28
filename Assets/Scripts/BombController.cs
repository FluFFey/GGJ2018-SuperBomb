using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public int exRadius;

    private Rigidbody rb;
    private bool stuck;

    public TimeStopController TSC;

    public List<Vector3> newPositions = new List<Vector3>();
    public float timer;
    public bool useGravity = false;

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
            useGravity = true;
        }

        //print(rb.velocity + "/" + (Physics.gravity * TimeStopController.getTimeScale()));

        if (useGravity)
        {
            rb.AddForce(Physics.gravity * TimeStopController.getTimeScale());
        }
        //if (transform.position.y < 0.40f)
        //{
        //    Vector3 newPos = transform.position;
        //    newPos.y = 0.40f;
        //    transform.position = newPos;
        //}
    }

    private void OnCollisionEnter(Collision other)
    {
        print("Bomb collided with: " + other.gameObject.name);
        if(other.gameObject.tag == "Floor")
        {
            rb.velocity = Vector3.zero;
            stuck = true;
        }
        else if (other.gameObject.name.Contains("GunPrefab") || other.gameObject.name.Contains("PlayerPrefab"))
        {
            return;
        }
        print("Collider: " + other.gameObject.name);
        newPositions = new List<Vector3>();
        useGravity = true;
        rb.isKinematic = false;
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
                //MonsterScript MS = col.GetComponent<MonsterScript>();
                //MS.dealDmg(1); //Deal 1 dmg, assuming regular enemies have 1hp, bigger have 2
                Destroy(col.gameObject);
            }
            if(col && col.gameObject.name.Contains("BombPrefab") && col.gameObject != this.gameObject)
            {
                print("hit another bomb!");
                //col.GetComponent<BombController>().recieveTrans();
                col.GetComponent<BombController>().startColl();
            }
        }
        Destroy(this.gameObject);
    }

    public void startColl()
    {
        StartCoroutine(collateral());
    }

    public IEnumerator collateral()
    {
        yield return new WaitForSeconds(0.1f);
        print("Done with waitin");
        recieveTrans();
    }
}
