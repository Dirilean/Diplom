using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonster : Monster
{
    float distance = 5F;
    [SerializeField]
    float StartPosX;

    private void OnEnable()
    {
        StartPosX = transform.position.x;
        lives = DefaultLives;
        speed = DefaultSpeed;
        napravlenie = -Vector3.right;//начальное направление вправо
        die = false;
        distance = 5F;
    }

    public override void Move()
    {
        //point = new Vector2(transform.position.x + (Dalnost * napravlenie.x), transform.position.y + 0.5F);//текущая позция проверки стен
        //стенки
        // Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 1F, 1 << 13);
        //условие поворота
        // if ((colliders.Length >0)||(distance<Mathf.Abs(transform.position.x-StartPosX))) napravlenie *= -1.0F;//перевернуть при условии появления в области стен
       // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.5F, 1 << 13);
        if (Player)//смотрим только за игроком
        {
            napravlenie.y = (Player.transform.position.y - transform.position.y) / Mathf.Abs((Player.transform.position.y - transform.position.y));
            napravlenie.x = (Player.transform.position.x - transform.position.x) / Mathf.Abs((Player.transform.position.x - transform.position.x));
        }
        else //смотрим только за платформами
        {
            //if (colliders.Length > 0)//если в зоне есть платформа разворачиваемся
            //{
            //    Debug.Log(colliders[0].name);
            //    if (colliders[0].transform.position.x - transform.position.x > 0)
            //        napravlenie = -Vector3.right;
            //    else napravlenie = Vector3.right;
            //}
            //else if ((StartPosX + 5F > transform.position.x) && (napravlenie.x < 0) || (StartPosX - 5F < transform.position.x) && (napravlenie.y > 0))//если ушли далеко о стартовой позиции
            //{
            //    // Debug.Log((StartPosX + 5F)+"> "+transform.position.x);
            //    napravlenie *= -1;
            //}
            napravlenie.y = 0F;
        }

        if (!playerNear)//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, speed * napravlenie.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x * SpriteSeeRight < 0.0F;//поворот}
            State = CharState.walk;
        }

    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag!="Player")
        {
            napravlenie *= -1;
        }
    }
}
