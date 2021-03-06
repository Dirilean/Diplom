﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameObject parent; //тот, кто пускает эту пулю
    public GameObject Parent { set { parent = value; } }

    public float speed;
    public float CurrentSpeed;
    [HideInInspector]
    public Vector3 napravlenie; //направление пули
    public int damage;
    public float lifetime;
    public int minusFire;//сколько отнимается огня на действие
    [Tooltip ("анимировать ли обьект?")]
    public bool animate=false;
    Color CurrenAlfa;
    [SerializeField]
    ParticleSystem Boom;

    private void Start()
    {
       CurrenAlfa=GetComponent<SpriteRenderer>().color;
    }

    IEnumerator ForBullet()
    {
        yield return new WaitForSeconds(lifetime);
        GetComponent<PoolObject>().ReturnToPool();//"удаление" объекта
    }

    private void OnEnable()
    {
        if (animate)
        {
            transform.localScale = new Vector3(0.2F, 0.2F, 0.2F);//маленький
            CurrenAlfa = new Color(255, 255, 255, 0);//прозрачный
        }
        StartCoroutine(ForBullet());
        CurrentSpeed = speed;
    }

    private void Update() //движение огня
    {
        if (animate)//постепенное увеличивание и увеличение прозрачности
        {
            CurrenAlfa.a = Mathf.Lerp(CurrenAlfa.a, 255F, Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 2F, Time.deltaTime), Mathf.Lerp(transform.localScale.y, 1F, Time.deltaTime * 4));
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + napravlenie, CurrentSpeed* Time.deltaTime);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
     //   Debug.Log(collision.collider.name);
        Monster monster = collision.collider.GetComponent<Monster>();
        Asteroid asteroid = collision.collider.GetComponent<Asteroid>();
        if (monster)
        {
            monster.lives -= damage;//получение урона от пули
            if (Boom != null)
            { GameObject boom = PoolManager.GetObject(Boom.name, transform.position, transform.rotation); }
            gameObject.GetComponent<PoolObject>().ReturnToPool();
        }
        else if (asteroid)
        {
            asteroid.live -= damage;//получение урона от пули

            if (Boom != null)
            { GameObject boom = PoolManager.GetObject(Boom.name, transform.position, transform.rotation); }
            gameObject.GetComponent<PoolObject>().ReturnToPool();
        }
        else if (!collision.gameObject.GetComponent<Character>())
        {
            GetComponent<PoolObject>().ReturnToPool();
        }
    }

}
