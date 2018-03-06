using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForGen : MonoBehaviour {

    //если true значит там есть что то
    public bool busy;
    [SerializeField]
    Character Player;
    byte chet;//считает количество коллайдеров в триггере

    private void Update()//перемещение
    {
        transform.position =new Vector3(Player.transform.position.x+Static.StepGen, 0);
    }


    void OnTriggerEnter2D(Collider2D collider)
    {
        chet++;
        if (chet > 0)
        {
            busy = true;
            //Debug.Log("busy=" + busy);
        }
    }

    void OnTriggerExit2D(Collider2D collider)
    {
        chet--;
        if (chet == 0)
        {
            busy = false;
          //  Debug.Log("busy=" + busy);
        }
    }
}
