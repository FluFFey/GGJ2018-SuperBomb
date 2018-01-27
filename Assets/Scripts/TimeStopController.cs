using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimeStopController : MonoBehaviour {

    public float currentTime = 0;
    private float prevTime = 0;

    private List<float> prevScales = new List<float>();

    public float timeScale = 1;

    public GameObject player;

    // Update is called once per frame
    void Update () {
        prevTime = currentTime;
        currentTime += Time.deltaTime * timeScale;

        if(prevScales.Count > 2)
        {
            prevScales.RemoveAt(0);
        }
        prevScales.Add(timeScale);
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

    public float getTimeScale()
    {
        return timeScale;
    }

    public float deltaTime() //Tiden som er gått
    {
        return (currentTime - prevTime) * timeScale;
    }

    public bool isRising() //deltaTime is gettin smaller
    {
        if (prevScales[0] - prevScales[1] > 0) // 0 - 0.1 = -0.1 vs.  0.1 - 0
        {
            return true;
        }
        return false;
    }

    public float getTime()
    {
        return currentTime * timeScale;
    }
}
