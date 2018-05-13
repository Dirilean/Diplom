using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParalaxMenu : MonoBehaviour {

    Vector3 offset;
    Vector3 lastPos;
    public float speed;
	// Update is called once per frame
	void Update ()
    {
        offset = Camera.main.ScreenToWorldPoint(Input.mousePosition) - lastPos;
        lastPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);

        transform.Translate(offset * speed * Time.deltaTime);
       // transform.position = Camera.main.ScreenToWorldPoint(Input.mousePosition);
	}
}
