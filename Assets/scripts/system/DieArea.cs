using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DieArea : MonoBehaviour {

    int Damage=1;//количество наносимого урона
    float TimeToDamage=0.5F;//время за которое наносятся один удар
    bool attack;

    private void OnTriggerStay2D(Collider2D collider)
    {
        Character unit = collider.GetComponent<Character>();
        if (unit && attack==false)//нанесение урона
        {
            StartCoroutine(ForDamage(unit));
        }
    }

    IEnumerator ForDamage(Character unit)
    {
        attack = true;
        yield return new WaitForSeconds(TimeToDamage);
        unit.lives = unit.lives - Damage;
        attack = false;

    }
    //private void OnCollisionEnter2D(Collision2D collision)
    //{
    //    Character unit = collision.collider.GetComponent<Character>();
    //    // unit.lives -=50;
    //    Debug.Log(unit.transform.position);
    //    unit.transform.position = new Vector3(unit.transform.position.x,25F);
    //    Debug.Log(unit.transform.position);
    //}
}
