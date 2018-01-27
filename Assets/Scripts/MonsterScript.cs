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
        findNewTarget();
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
        //print((transform.position - target).magnitude);
        if(currentDirection != (int)direction.NO_DIRECTION)
        {
            
            if ((transform.position - target).magnitude < 0.01f)
            {
                findNewTarget();
            }
        }
		
	}

    void findNewTarget()
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
                currentDirection = i;
                target = transform.position + newPosChecklist[currentDirection % newPosChecklist.Length] * tileLength;
                StartCoroutine(Tools.moveObject(
                    gameObject, 
                    newPosChecklist[currentDirection], 
                    3.0f, 
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
}
