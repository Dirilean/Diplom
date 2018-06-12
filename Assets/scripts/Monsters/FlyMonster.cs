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
        if (Player)//смотрим только за игроком
        {
            napravlenie.y = (Player.transform.position.y+0.5F - transform.position.y) / Mathf.Abs((Player.transform.position.y+0.5F - transform.position.y));
            napravlenie.x = (Player.transform.position.x - transform.position.x) / Mathf.Abs((Player.transform.position.x - transform.position.x));
        }
        else //смотрим только за платформами
        {
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
