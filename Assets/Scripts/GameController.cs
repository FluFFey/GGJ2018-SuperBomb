using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameController : MonoBehaviour {

    int playerLives = 1;
    int remainingBombs = 5;
    public Text livesText;
    public Text bombsText;
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
        bombsText.text = "Bombs: " + remainingBombs;
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

    internal bool haveBombs()
    {
        return remainingBombs > 0;
    }

    // Update is called once per frame
    void Update () {
		
	}

    internal void spendBombs(int v)
    {
        remainingBombs -= v;
        bombsText.text = "Bombs: " + remainingBombs;
    }

    internal void gainBombs(int v)
    {
        remainingBombs += v;
        bombsText.text = "Bombs: " + remainingBombs;
    }
}
