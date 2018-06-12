using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Strizh : Monster
{
    Vector3 povorot = new Vector3(1, 0, 0);
    float Predpos;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        speed = 2F;
    }

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, 1);
        lives = DefaultLives;
        speed = DefaultSpeed;
        napravlenie = Vector3.right;//начальное направление вправо
        die = false;
        StartCoroutine(Zastryal());
        transform.rotation = Quaternion.Euler(0, 0, -45);
        napravlenie = new Vector3(1, 1, 0);
    }

    private void FixedUpdate()
    {   
        rb.velocity = new Vector3(speed * napravlenie.x, speed * napravlenie.y, 0);
    }

    public override void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.36F, (1 << 13) | (1 << 12));

        //условие поворота
        if (colliders.Length > 0)//перевернуть при условии появления в области платформ
        {
            napravlenie *= -1;
            if (transform.rotation == Quaternion.Euler(0, 0, -45))
            {
                transform.rotation = Quaternion.Euler(0, 0, 135);
            }
            else
            {
                transform.rotation = Quaternion.Euler(0, 0,-45);
            }

        }
        
    }

    private void OnCollisionEnter2D(Collision2D collider)//столкновение с игроком
    {
        if ((collider.gameObject.tag== "Player"))
        {
            collider.gameObject.GetComponent<Character>().lives -= Damage;
            lives = 0;
        }
    }
}
