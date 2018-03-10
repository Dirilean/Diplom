using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PassiveEnemy : MonoBehaviour {

    [SerializeField]
    int Damage;//количество наносимого урона
    [SerializeField]
    float TimeToDamage;//время за которое наносятся один удар
    [SerializeField]
    bool Once;//урон единожды?
    [SerializeField]
    int MinusFireSphere;//количество огнесвета в процентах, который забирается при ударе
    float LastTime;//Время последнего удара
    bool Udar;//одиночный удар уже был нанесен?

    private void OnTriggerStay2D(Collider2D collider)
    {
        Character unit = collider.GetComponent<Character>();
        if ((unit)&&(Once==true))//Одиночный удар
        {
            unit.lives = unit.lives - Damage;
            unit.FireColb=Mathf.RoundToInt(unit.FireColb-(unit.FireColb*MinusFireSphere*0.01F));
            Udar = true;
        }
        else if ((unit) && (Time.time-TimeToDamage>LastTime))//нанесение урона
        {
            unit.lives = unit.lives - Damage;
            LastTime = Time.time;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        Character unit = collision.GetComponent<Character>();
        if ((unit) && (Once == true))//Обнуляем флаг одиночного удара
        {
            Udar = false;
        }
    }


}
