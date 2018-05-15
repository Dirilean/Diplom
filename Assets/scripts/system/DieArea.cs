using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieArea : MonoBehaviour {

    int Damage=1;//количество наносимого урона
    float TimeToDamage=0.5F;//время за которое наносятся один удар
    float LastTime=0F;//Время последнего удара

    private void OnTriggerStay2D(Collider2D collider)
    {
        Character unit = collider.GetComponent<Character>();
        if ((unit) && (Time.time - TimeToDamage > LastTime))//нанесение урона
        {
            unit.lives = unit.lives - Damage;
            LastTime = Time.time;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character unit = collision.collider.GetComponent<Character>();
        unit.lives -=50;
        unit.transform.position = new Vector3(unit.transform.position.x,25F);
    }
}
