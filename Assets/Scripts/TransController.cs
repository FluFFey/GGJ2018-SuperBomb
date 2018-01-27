using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransController : MonoBehaviour {

    public float lifeTime;
    private float currentLifeTime;

    private void Start()
    {
        currentLifeTime = lifeTime;
    }

    private void Update()
    {
        if(currentLifeTime <= 0)
        {
            Destroy(this.gameObject);
        }
        else
        {
            currentLifeTime -= Time.deltaTime;
        }
    }

    private void OnCollisionEnter(Collision other)
    {
        print("Collision!");
        if (other.gameObject.GetComponent<BombController>() != null)
        {
            print("Collision2!");
            other.gameObject.GetComponent<BombController>().recieveTrans();
            Destroy(this.gameObject);
        }
        else if(!other.gameObject.GetComponent<PlayerController>())
        {
            Destroy(this.gameObject);
        }
    }
}
