using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGen : MonoBehaviour {

    //если true значит там есть что то
    public bool busy;
    Character Player;
    byte chet;//считает количество коллайдеров в триггере

    private void Start()
    {
        transform.position = new Vector3(Static.ForgenPosition, 0);
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void Update()//перемещение
    {
        if (Player.enabled == false)
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Character>();
        }

        if (Player.PrefabLevel==1) transform.position =new Vector3(Player.transform.position.x+Static.ForgenPosition, 0);
        else if (Player.PrefabLevel==2) transform.position = new Vector3(Player.transform.position.x + Static.ForgenPosition, 10F);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        chet++;
        if (chet > 0)
        {
            busy = true;
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        chet--;
        if (chet == 0)
        {
            busy = false;
        }
    }
}
