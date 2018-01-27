using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GameController : MonoBehaviour {

    public int playerLives;
    public int remainingBombs;
    public Text livesText;
    public Text bombsText;
    public static GameController instance;

    public GameObject crosshair;
    public GameObject gameOverObj;
    public GameObject pauseGameObj;
    public GameObject select1;
    public GameObject select2;
    public Text timer;

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

        //gameOver();
    }
	
    public void removePlayerLives(int amountToRemove)
    {
        playerLives -= amountToRemove;
        livesText.text = "Lives: " + playerLives;
        if (playerLives <= 0)
        {
            gameOver();
        }
    }

    private void gameOver()
    {
        Time.timeScale = 0; //?
        crosshair.SetActive(false);
        gameOverObj.SetActive(true);
        select1.SetActive(true);
        livesText.text = "";
        bombsText.text = "";
    }

    private void togglePauseGame()
    {
        if (pauseGameObj.activeSelf)
        {
            Time.timeScale = 1;
            pauseGameObj.SetActive(false);
            crosshair.SetActive(false);
            select1.SetActive(false);
        }
        else
        {
            Time.timeScale = 0;
            pauseGameObj.SetActive(true);
            crosshair.SetActive(true);
            select1.SetActive(true);
        }
    }

    internal bool haveBombs()
    {
        return remainingBombs > 0;
    }

    // Update is called once per frame
    void Update () {
        if (Input.GetKeyDown(KeyCode.Escape) && !gameOverObj.activeSelf)
        {
            togglePauseGame();
        }
        if (gameOverObj.activeSelf)
        {
            if (Input.GetKeyDown(KeyCode.W))
            {
                if (!select1.activeSelf)
                {
                    select1.SetActive(true);
                    select2.SetActive(false);
                }
            }
            else if (Input.GetKeyDown(KeyCode.S))
            {
                if (!select2.activeSelf)
                {
                    select1.SetActive(false);
                    select2.SetActive(true);
                }
            }
            else if (Input.GetKeyDown(KeyCode.Return)){
                if (select1.activeSelf)
                {
                    mainMenuClick();
                }
                else if (select2.activeSelf)
                {
                    retryClick();
                }
            }
        }
        timer.text = TimeStopController.currentTime.ToString("F2");
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

    public void mainMenuClick()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene("Main Menu");
    }

    public void retryClick()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
