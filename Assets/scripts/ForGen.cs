using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGen : MonoBehaviour {

    //если true значит там есть что то
    public bool busy;
    [SerializeField]
    Character Player;
    byte chet;//считает количество коллайдеров в триггере

    private void Start()
    {
        transform.position = new Vector3(Static.ForgenPosition, 0);
    }

    private void Update()//перемещение
    {
        transform.position =new Vector3(Player.transform.position.x+Static.ForgenPosition, 0);
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
