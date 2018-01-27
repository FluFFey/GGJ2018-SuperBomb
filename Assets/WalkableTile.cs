using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[ExecuteInEditMode]
public class WalkableTile : MonoBehaviour {
    public enum tileType
    {
        HORIZONTAL,
        VERTICAL,
        CORNER_LU,
        CORNER_LD,
        CORNER_RU,
        CORNER_RD,
        CROSS_SECTION
    }


    public tileType type;
    private Material newMat;
    private void Awake()
    {
        GetComponent<MeshRenderer>().material = new Material(GetComponent<MeshRenderer>().material);
    }
    // Use this for initialization
    void Start ()
    {
		switch (type)
        {
            case tileType.HORIZONTAL:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_HorizontalLine", 1);
                break;
            case tileType.VERTICAL:
                
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_VerticalLine", 1);
                break;
            case tileType.CORNER_LU:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_CornerLU", 1);
                break;
            case tileType.CORNER_LD:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_CornerLD", 1);
                break;
            case tileType.CORNER_RU:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_CornerRU", 1);
                break;
            case tileType.CORNER_RD:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_CornerRD", 1);
                break;
            case tileType.CROSS_SECTION:
                GetComponent<MeshRenderer>().sharedMaterial.SetFloat("_IsCrossSection", 1);
                break;
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
