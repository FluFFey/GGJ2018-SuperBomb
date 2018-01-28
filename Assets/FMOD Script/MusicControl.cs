using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicControl : MonoBehaviour {
	[FMODUnity.EventRef]
	public string music;

	FMOD.Studio.EventInstance musicEv;

	// Use this for initialization
	void Start () {
		musicEv = FMODUnity.RuntimeManager.CreateInstance (music);

		musicEv.start();
	
	}
	// Player standing still
	public void PlayerStand()
	{
		musicEv.setParameterValue("Progress", 0f);
	}

	// Player moving
	public void PlayerMove()
	{
		musicEv.setParameterValue("Progress", 1f);
	}


	// Update is called once per frame
	void Update ()
	{
		if ( TimeStopController.timeScale > 0.5)
		{
			musicEv.setParameterValue("Progress", 1);
		}
		else
		{
			musicEv.setParameterValue("Progress", 0);
		}
			

	}
}
