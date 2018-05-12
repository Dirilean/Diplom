using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class alfa_chanell : MonoBehaviour {


    Color alfa;


    void Start () {
       alfa= GetComponent<SpriteRenderer>().color;
	}
	
	// Update is called once per frame
	void Update () {
        alfa = new Color(alfa.r, alfa.g, alfa.b, Mathf.PingPong(Time.time,1.0f));
	}
}
