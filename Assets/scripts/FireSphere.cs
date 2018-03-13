using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSphere : MonoBehaviour {




    //transform.Translate(new Vector2(1,0)*Time.deltaTime);
    public Vector2 target;
    public float smoothTime;
    public float smoothTime1;
    private float Velocity = 0.0F;
    bool once;
    Vector3 Niz;
    Vector3 Verh;
    Vector3 Now;
    System.Random rnd = new System.Random();

    private void Start()
    {

        smoothTime = rnd.Next(3)+1;
        smoothTime1 = smoothTime;
        target = new Vector3(0, 1);
        once = false;
        Verh = new Vector3(0, 0.15F)+transform.position;
        Niz = -new Vector3(0, 0.1F)+transform.position;
    }

    void Update()
    {

        float newPosition = Mathf.SmoothDamp(transform.position.y, Now.y, ref Velocity, smoothTime1);
        transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
        if (((Mathf.Abs(Verh.y - transform.position.y) < 0.02) || (Mathf.Abs(Niz.y - transform.position.y) < 0.02)) && (once == true))//если близко к верху, поменять таргет
        {
            once = false;
            smoothTime1 = smoothTime;
            Velocity = 0.0F;
            target *= -1;
            Now = (Now == Verh) ? Niz : Verh;
        }
        if (((Mathf.Abs(Verh.y - transform.position.y) > 0.02) || (Mathf.Abs(Niz.y - transform.position.y) > 0.02)) && (once == false))
        {
            once = true;

        }

    }
}



