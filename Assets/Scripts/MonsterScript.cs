using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonsterScript : MonoBehaviour {
    Vector3 target; //intentionally leaving this empty so we get errors
    public LayerMask pathLayerMask;
    private int lives = 1;
    private int damage = 1;
    enum direction
    {
        LEFT,
        RIGHT,
        FORWARD,
        BACK,
        NO_DIRECTION
    }
    int currentDirection = (int)direction.NO_DIRECTION; //default direction
    public int tileLength;
    Vector3[] newPosChecklist =
    {
        Vector3.left,
        Vector3.right,
        Vector3.forward,
        Vector3.back
    };

    public void takeDamage(int damageToTake)
    {
        lives -= damageToTake;
        if (lives <=0)
        {
            DestroyObject(gameObject);
        }
    }

    // Use this for initialization
    void Start ()
    {
        //print("start");
        findNewTarget(false);
        switch (currentDirection)
        {
            case (int)direction.LEFT:
                transform.rotation = Quaternion.Euler(Vector3.up * 90);
                break;
            case (int)direction.RIGHT:
                transform.rotation = Quaternion.Euler(Vector3.up * 270);
                break;
            case (int)direction.FORWARD:
                transform.rotation = Quaternion.Euler(Vector3.up * 0);
                break;
            case (int)direction.BACK:
                transform.rotation = Quaternion.Euler(Vector3.up * 180);
                break;
        }
        //StartCoroutine(Tools.moveObject(
        //      gameObject,
        //      newPosChecklist[currentDirection],
        //      5.0f,
        //      10,
        //      Tools.INTERPOLATION_TYPE.LERP
        //      ));
    }
	
	// Update is called once per frame
	void Update () {
        GetComponentInChildren<Animator>().speed = TimeStopController.instance.timeScale;
        //print((transform.position - target).magnitude);
        if(currentDirection != (int)direction.NO_DIRECTION)
        {
            if ((transform.position - target).magnitude < 0.01f)
            {
                findNewTarget();
            }
        }
		
	}

    void findNewTarget(bool rotate = true)
    {
        bool endOfPath = true;
        for (int i = 0; i < newPosChecklist.Length; i++)
        {
            switch (currentDirection)
            {
                case (int)direction.LEFT:
                    if (i == (int)direction.RIGHT)
                    {
                        continue;
                    }
                    break;
                case (int)direction.RIGHT:
                    if (i == (int)direction.LEFT)
                    {
                        continue;
                    }
                    break;
                case (int)direction.FORWARD:
                    if (i == (int)direction.BACK)
                    {
                        continue;
                    }
                    break;
                case (int)direction.BACK:
                    if (i == (int)direction.FORWARD)
                    {
                        continue;
                    }
                    break;
                default:
                    break;
            }
            RaycastHit hit;
            if (Physics.Raycast(transform.position + newPosChecklist[i% newPosChecklist.Length]* tileLength + Vector3.up, Vector3.down, out hit, 2, pathLayerMask))
            {
                if (rotate)
                {
                    rotateTowards(i);
                }
                
                currentDirection = i;
                target = transform.position + newPosChecklist[currentDirection % newPosChecklist.Length] * tileLength;

                
                StartCoroutine(Tools.moveObject(
                    gameObject, 
                    newPosChecklist[currentDirection], 
                    1.0f, 
                    10, 
                    Tools.INTERPOLATION_TYPE.LERP
                    ));
                endOfPath = false;
                break;
            }
        }
        if (endOfPath)
        {
            GameController.instance.removePlayerLives(damage);
            Destroy(gameObject);
        }
    }

    void rotateTowards(int newDir)
    {
        float rotTime = 0.33f;
        float startRot = transform.rotation.eulerAngles.y;
        float rotAmount = 0.0f;
        
        switch (newDir)
        {
            case (int)direction.LEFT:
                if (currentDirection == (int)direction.FORWARD)
                {
                    rotAmount = -90.0f;
                }
                if (currentDirection == (int)direction.BACK)
                {
                    rotAmount = 90.0f;
                }
                break;
            case (int)direction.RIGHT:
                if (currentDirection == (int)direction.FORWARD)
                {
                    rotAmount = 90.0f;
                }
                if (currentDirection == (int)direction.BACK)
                {
                    rotAmount = -90.0f;
                }
                break;
            case (int)direction.FORWARD:
                if (currentDirection == (int)direction.LEFT)
                {
                    rotAmount = 90;
                }
                if (currentDirection == (int)direction.RIGHT)
                {
                    rotAmount = -90;
                }
                break;
            case (int)direction.BACK:
                if (currentDirection == (int)direction.LEFT)
                {
                    rotAmount = -90;
                }
                if (currentDirection == (int)direction.RIGHT)
                {
                    rotAmount = 90;
                }
                break;
            default:
                break;
        }
        StartCoroutine(Tools.rotateObject(gameObject, Vector3.up* rotAmount, rotTime,Tools.INTERPOLATION_TYPE.EXPONENTIAL));
        //for (float i = 0; i < rotTime; i+=TimeStopController.instance.deltaTimeScale)
        //{
        //    float pd = i / rotTime;
        //    float newAngle = startRot + (startRot - goalRot) * pd;
        //    Vector3 newRot = transform.rotation.eulerAngles;
        //    newRot.y = newAngle;
        //    transform.rotation = Quaternion.Euler(newRot);
        //}

    }
}
