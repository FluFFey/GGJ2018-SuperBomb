using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gunPrefab;
    public TimeStopController TSC;
    public GameController GC;
    public GameObject BombPrefabHand;

    // Use this for initialization
    void Start () {
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
        GC = GameObject.Find("GameController").GetComponent<GameController>();
    }
	
	// Update is called once per frame
	void Update () {
        transform.GetChild(1).GetComponent<Animator>().speed = 1 * TimeStopController.timeScale;

        int lerpSpeed = 10;
        float targetScale = 1;
        if (Input.GetKeyUp(KeyCode.Mouse0) && !GC.pauseGameObj.activeSelf && !GC.gameOverObj.activeSelf)
        {
            print("Mouse0: Trans-Shoot");
            gunPrefab.GetComponent<GunController>().shootTrans();
            targetScale = 0.5f;
            transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().Play("Gun_Fire");
            StartCoroutine(wait(1));
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && !GC.pauseGameObj.activeSelf && !GC.gameOverObj.activeSelf)
        {
            print("Mouse0: Bomb-Shoot");
            if (GameController.instance.haveBombs())
            {
                if (gunPrefab.GetComponent<GunController>().throwBomb(transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>()))
                {
                    //same as above
                    targetScale = 0.5f;
                    GameController.instance.spendBombs(1);
                    transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().SetBool("isPredictingBomb", false);
                }
            }
        }
        if (!transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().GetBool("isPredictingBomb"))
        {
            gunPrefab.SetActive(true);
            BombPrefabHand.SetActive(false);
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            gunPrefab.GetComponent<GunController>().bombPrediction();
            transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().SetBool("isPredictingBomb", true);
            gunPrefab.SetActive(false);
            BombPrefabHand.SetActive(true);
        }
        else
        {
            gunPrefab.GetComponent<GunController>().destroyDots();
        }

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.D))
        {
            targetScale = 1;
        }
        else
        {
            targetScale = 0;
            lerpSpeed = 4;
        }
        if (Input.GetKey(KeyCode.LeftShift))
        {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().SetBool("isRunning", true);
        }
        else
        {
            transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().SetBool("isRunning", false);
        }

        TSC.setTimeScale(Mathf.Lerp(TimeStopController.getTimeScale(), targetScale, Time.deltaTime * lerpSpeed));
    }

    IEnumerator wait(int id)
    {
        yield return new WaitForSeconds(1);
        switch (id)
        {
            case 1: //wait for isfiring
                transform.GetChild(0).transform.GetChild(0).GetComponent<Animator>().SetBool("isRunning", false);
                break;
        }
    }

    public void removeLife()
    {
        print("Im dying....");
        GC.removePlayerLives(1);
    }

    private void OnTriggerEnter(Collider other)
    {
        print("Colliding (player)");
        if (other.gameObject.name.Equals("enemyKatana"))
        {
            removeLife();
        }
    }
}
