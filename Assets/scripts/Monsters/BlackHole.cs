using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlackHole : Asteroid {

    private void Start()
    {
        dvig = false;
        live=10000;
    }

    public override void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.GetComponent<Asteroid>())
        {
            collision.collider.GetComponent<Asteroid>().live = 0;
        }
        else if (collision.collider.GetComponent<Character>())
        {
            collision.collider.GetComponent<Character>().lives = 0;
        }
    }
}
