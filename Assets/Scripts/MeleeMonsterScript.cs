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
    public GameObject deathParticleSystem;
    

    // Use this for initialization
    void Start () {
        player = GameObject.FindGameObjectWithTag("Player");
        //rb = GetComponent<Rigidbody>();
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        //rb.velocity = Vector3.zero;

        transform.GetChild(0).GetComponent<Animator>().speed = 1 * TimeStopController.timeScale;
        Animator anim = transform.GetChild(0).GetComponent<Animator>();

        bool isWalking = false;
        Vector3 lookAt = player.transform.position;
        lookAt.y = transform.position.y;
        transform.LookAt(lookAt);
        if (Vector3.Distance(transform.position, player.transform.position) >= minDistance && TimeStopController.timeScale >= 0.01f)
        {
            isWalking = true;
            transform.position += (transform.forward* speed * TimeStopController.deltaTime());
            //print((transform.forward * speed * TimeStopController.deltaTime()));
            if (Vector3.Distance(transform.position, player.transform.position) <= minDistance + 1)
            {
                isWalking = false;
                //play attack anim
                print("Attacku!");
                anim.SetBool("isAttacking", true);
            }
            else
            {
                anim.SetBool("isAttacking", false);
                if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemyWalkWeapon"))
                {
                    anim.Play("enemyWalkWeapon");
                }
            }
        }

        if (isWalking)
        {
            anim.SetBool("isAttacking", false);
            if (!anim.GetCurrentAnimatorStateInfo(0).IsName("enemyWalkWeapon"))
            {
                anim.Play("enemyWalkWeapon");
            }
        }
        //rb.AddForce(Physics.gravity * TimeStopController.timeScale);
	}

    private void OnDestroy()
    {
        Instantiate(deathParticleSystem).transform.position = transform.position;
    }

    private void OnCollisionStay(Collision other)
    {
        print("Monster collided with: " + other.gameObject.name);
        if (other.gameObject.name.Contains("Jumpable"))
        {
            //transform.position += jumpVector;
            print("Collision/jump?!");
        }
    }
}
