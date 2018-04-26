using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class fon : MonoBehaviour {


    [SerializeField]
    Camera MyCamera;
    float scale;
    // Use this for initialization
    void Start ()
    {
        scale = MyCamera.pixelWidth / 1086.0F;
        transform.localScale = new Vector3(scale, 1, 0);
        transform.localPosition =new Vector3(0,-MyCamera.pixelHeight/100+1);
    }
	

}
