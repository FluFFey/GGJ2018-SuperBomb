using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BombController : MonoBehaviour {

    public int exRadius;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void recieveTrans()
    {
        //Run explosion animation
        Collider[] cols = Physics.OverlapSphere(transform.position, exRadius);
        foreach (Collider col in cols)
        {
            if (col && col.tag == "Enemy") // if object has the right tag...
            { 
              //EnemyController EC = col.GetComponent<EnemyController>();
              //EC.dealDmg(1); //Deal 1 dmg, assuming regular enemies have 1hp, bigger have 2
            }
        }
        Destroy(this.gameObject);
    }
}
