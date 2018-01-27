using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    int playerLives = 1;
    public Text livesText;
    public static GameController instance;
    void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else if (instance != this)
        {
            DestroyObject(gameObject);
        }
        DontDestroyOnLoad(this);
    }
    
    // Use this for initialization
    void Start ()
    {
        livesText.text = "Lives: " + playerLives;
    }
	
    public void removePlayerLives(int amountToRemove)
    {
        playerLives -= amountToRemove;
        livesText.text = "Lives: " + playerLives;
        if (playerLives >= 0)
        {
            //game over event
        }
    }

	// Update is called once per frame
	void Update () {
		
	}
}
