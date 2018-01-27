using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopController : MonoBehaviour{

    public static float currentTime = 0;
    private static float prevTime = 0;
    public float deltaTimeScale;
    public static float timeScale = 1;
    public List<float> prevScales = new List<float>();

    public static TimeStopController instance;

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

    private void Start()
    {
        prevScales.Add(1);
        prevScales.Add(1);
    }

    // Update is called once per frame
    void Update() {
        prevTime = currentTime;
        currentTime += Time.deltaTime * timeScale;

        if(timeScale != prevScales[1])
        {
            prevScales.Add(timeScale);
        }

        if (prevScales.Count > 2)
        {
            prevScales.RemoveAt(0);
        }

        deltaTimeScale = prevScales[0] - prevScales[1];
    }

    public void setTimeScale(float value)
    {
        if(value <= 0)
        {
            timeScale = 0;
        }
        else if(value >= 1)
        {
            timeScale = 1;
        }
        else
        {
            timeScale = value;
        }
    }

    public static float getTimeScale()
    {
        return timeScale;
    }

    public static float deltaTime() //Tiden som er gått
    {
        return (currentTime - prevTime);
    }

    public bool isRising() //deltaTime is gettin smaller
    {
        if (prevScales[0] - prevScales[1] >= 0) // 0 - 0.1 = -0.1 vs.  0.1 - 0
        {
            return true;
        }
        return false;
    }

    public float getTime()
    {
        return currentTime;
    }
}
