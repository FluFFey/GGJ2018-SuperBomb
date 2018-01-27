using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

    public GameObject gunPrefab;
    public TimeStopController TSC;
    public GameController GC;

	// Use this for initialization
	void Start () {
        TSC = GameObject.Find("TimeStopController").GetComponent<TimeStopController>();
	}
	
	// Update is called once per frame
	void Update () {
        int lerpSpeed = 10;
        float targetScale = 1;
        if (Input.GetKeyUp(KeyCode.Mouse0) && !GC.pauseGameObj.activeSelf && !GC.gameOverObj.activeSelf)
        {
            print("Mouse0: Trans-Shoot");
            gunPrefab.GetComponent<GunController>().shootTrans();
            //gunController handler animations for this.
            targetScale = 0.5f;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse1) && !GC.pauseGameObj.activeSelf && !GC.gameOverObj.activeSelf)
        {
            print("Mouse0: Bomb-Shoot");
            if (GameController.instance.haveBombs())
            {
                if (gunPrefab.GetComponent<GunController>().throwBomb())
                {
                    //same as above
                    targetScale = 0.5f;
                    GameController.instance.spendBombs(1);
                }
            }
        }

        if (Input.GetKey(KeyCode.Mouse1))
        {
            gunPrefab.GetComponent<GunController>().bombPrediction();
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

        TSC.setTimeScale(Mathf.Lerp(TSC.getTimeScale(), targetScale, Time.deltaTime * lerpSpeed));
    }
}
