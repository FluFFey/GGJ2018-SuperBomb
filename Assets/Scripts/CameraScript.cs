using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {

    public Material scanLinesMat;
	// Use this for initialization
	void Start () {
        
	}
	
	// Update is called once per frame
	void Update () {
        //Camera.main.RenderWithShader(scanLinesShader,"");
    }

    private void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        scanLinesMat.SetFloat("_ScaledTime", TimeStopController.currentTime);
        Graphics.Blit(source, destination, scanLinesMat);
    }



}
