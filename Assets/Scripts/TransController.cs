using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransController : MonoBehaviour {

    public float lifeTime;
    private float currentLifeTime;

    public TimeStopController TSC;
    private Rigidbody rb;

    public Vector3 startVal;
    public float startValFactor;
    private Vector3 topVal;

    private void Start()
    {
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
        rb = GetComponent<Rigidbody>();

        currentLifeTime = lifeTime;

        topVal = startVal * startValFactor;
    }

    private void Update()
    {
        if(currentLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentLifeTime -= Time.deltaTime * TSC.timeScale;
        }

        rb.velocity = topVal * TSC.getTimeScale(); //Stop mid air
    }

    private void OnCollisionEnter(Collision other)
    {
        //print("Collision!");
        if (other.gameObject.GetComponent<BombController>() != null)
        {
            //print("Collision2!");
            other.gameObject.GetComponent<BombController>().recieveTrans();
            Destroy(this.gameObject);
        }
        else if(!other.gameObject.GetComponent<PlayerController>() && !other.gameObject.GetComponent<TransController>())
        {
            Destroy(this.gameObject);
        }
    }
}
