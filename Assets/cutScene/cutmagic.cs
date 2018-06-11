using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class cutmagic : MonoBehaviour {

    float X=17F;
    float x1;

	void Update () {
        x1 = Mathf.Lerp(transform.position.x, 30F, Time.deltaTime*0.3F);
        transform.position=new Vector3(x1,transform.position.y);
	}
}
