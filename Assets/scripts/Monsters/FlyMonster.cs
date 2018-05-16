using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyMonster : Monster
{
    float distance = 5F;
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

        point = new Vector2(transform.position.x + (Dalnost * napravlenie.x), transform.position.y + 0.5F);//текущая позция проверки стен
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 1F, 1 << 13);
        //условие поворота
        if ((colliders.Length >0)||(distance<=Mathf.Abs(transform.position.x-StartPosX))) napravlenie *= -1.0F;//перевернуть при условии появления в области стен
        if (!playerNear)//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x * SpriteSeeRight < 0.0F;//поворот}
            State = CharState.walk;
        }
    }
}
