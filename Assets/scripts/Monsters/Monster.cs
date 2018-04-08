﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Unit

{
    [SerializeField]
    public int Damage;//количество наносимого урона
    [SerializeField]
    public float TimeToDamage;//время за которое наносятся один удар (указывается в префабе)
    [SerializeField]
    public int lives;//жизни
    [SerializeField]
    public float speed;//скорость передвижения
    [SerializeField]
    public int PlusFireColb;//сколько упадет огня с монстров
    [SerializeField]
    FireSphere FireSpherePrefab;

    public Animator animator;

    public float radius;//радиус удара
    public int layerMask;//слой "жертвы" (игрок)
    public float Dalnost;//дальность удара (центр окружности)
    protected float LastTime;//Время последнего удара

    protected float XPos;
    protected float YPos;
    System.Random rnd = new System.Random();


    private void Start()
    {
        layerMask = 10;
        LastTime = 0;
    }


    public void Die()
    {
        XPos = gameObject.transform.position.x;
        int k = 0;
        while (k < PlusFireColb)//генерирование огоньков
        {
            YPos = (float)(rnd.NextDouble()) / 3 + 0.3F;//от 0,3 до 0,6
            FireSphere FireSphere = Instantiate(FireSpherePrefab, new Vector2(XPos, gameObject.transform.position.y + YPos), FireSpherePrefab.transform.rotation);
            XPos += 0.5F;
            k++;
        }
        Destroy(gameObject);
    }

    // функция возвращает ближайший объект из массива, относительно указанной позиции
    static GameObject NearTarget(Vector3 position, Collider2D[] array)
    {
        Collider2D current = null;
        float dist = Mathf.Infinity;

        foreach (Collider2D coll in array)
        {
            float curDist = Vector3.Distance(position, coll.transform.position);

            if (curDist < dist)
            {
                current = coll;
                dist = curDist;
            }
        }

        return current.gameObject;
    }

    public static void DoDamage(Vector2 point, float radius, int layerMask, int damage)//ближний бой
    {

     //   Animator.SetBool("attack", true);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        GameObject obj = NearTarget(point, colliders);
        if (obj.GetComponent<Character>())
        {   
            obj.GetComponent<Character>().lives -= damage;
        }
        return;
    }
}
