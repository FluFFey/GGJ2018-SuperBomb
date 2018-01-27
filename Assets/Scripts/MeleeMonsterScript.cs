using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeMonsterScript : MonoBehaviour {

    public float minDistance;
    public int speed;
    public GameObject player;
    public Vector3 jumpVector;

    private Rigidbody rb;
    private bool jump;

	// Use this for initialization
	void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        rb.velocity = Vector3.zero;
        bool isWalking = false;
        Vector3 lookAt = player.transform.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);
        if (Vector3.Distance(transform.position, player.transform.position) >= minDistance && TimeStopController.timeScale >= 0.01f)
        {
            isWalking = true;
            transform.position += (transform.forward* speed * TimeStopController.deltaTime());
            //print((transform.forward * speed * TimeStopController.deltaTime()));
            if (Vector3.Distance(transform.position, player.transform.position) <= minDistance)
            {
                isWalking = false;
                //play attack anim
                print("Attacku!");
            }
        }

        if (isWalking)
        {
            //walk-anim
        }

        rb.AddForce(Physics.gravity * TimeStopController.timeScale);
	}

    private void OnCollisionStay(Collision other)
    {
        if (other.gameObject.name.Contains("Jumpable"))
        {
            //transform.position += jumpVector;
            print("Collision!");
        }
    }
}
