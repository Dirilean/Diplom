using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGen : MonoBehaviour {

    //если true значит там есть что то
    public bool busy;
    public Character Player;
    byte chet;//считает количество коллайдеров в триггере
    float x;

    private void Start()
    {
        transform.position = new Vector3(Static.ForgenPosition, 0);
    }

    private void Update()//перемещение
    {
        x = Mathf.Lerp(transform.position.x, Player.transform.position.x + Static.ForgenPosition, Time.deltaTime * 50F);
        switch (Player.PrefabLevel)
        {
            case 1: { transform.position = new Vector3(x, 0); break; }
            case 2: { transform.position = new Vector3(x, 10F); break; }
        }
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            chet++;
            if (chet > 0)
            {
                busy = true;
            }
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        if (collider.tag == "Ground")
        {
            chet--;
            if (chet == 0)
            {
                busy = false;
            }
        }
    }
}
