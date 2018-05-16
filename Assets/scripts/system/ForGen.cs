using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGen : MonoBehaviour {

    //если true значит там есть что то
    public bool busy;
    public Character Player;
    byte chet;//считает количество коллайдеров в триггере

    private void Start()
    {
        transform.position = new Vector3(Static.ForgenPosition, 0);
    }

    private void Update()//перемещение
    {
        switch (Player.PrefabLevel)
        {
            case 1: { transform.position = new Vector3(Player.transform.position.x + Static.ForgenPosition, 0); break; }
            case 2: { transform.position = new Vector3(Player.transform.position.x + Static.ForgenPosition, 10F); break; }
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
