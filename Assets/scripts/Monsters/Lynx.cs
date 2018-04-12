using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lynx : Monster
{

    Character Player;

    float DistanceSee;//видимость

    private bool isplayer;//проверка на игрока впереди
    public bool isGrounded;
    float betveen;//расстояние между рысью и игроком в плоскости х

    private void Start()
    {
        napravlenie = transform.right;//начальное направление вправо
        DistanceSee = 5;
        Player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }


    private void FixedUpdate()
    {
        Move();
        betveen = Mathf.Abs(Player.transform.position.x - transform.position.x);
        CheckGround();    
    }


    public override void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * napravlenie.x * 0.5F, 0.01F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.3F + transform.right * napravlenie.x * 0.5F, 0.3F);

        //условие поворота и прыэок
        if (betveen < DistanceSee)//если видит лису
        {

            //смена направления
            if (betveen < 0.3f)//если над или под
            {
                napravlenie = Vector3.zero;
            }
            else
            {
                napravlenie = ((Player.transform.position.x - transform.position.x > 0) ? Vector3.right : -Vector3.right);//поворот к игроку
            }

            //прыжок
            if ((Player.transform.position.y - transform.position.y > 1.5F) && ((Player.transform.position.y - transform.position.y < 2.5F))//смотрим по У
            && (betveen < 3F) && (betveen > 2F)//смотрим по Х
            && (Player.isGrounded == true) && (isGrounded == true))//для прыжка
            {
                //прикладываем силу вверх, чтобы персонаж подпрыгнул
                rb.AddForce(new Vector3(10F * napravlenie.x, 25), ForceMode2D.Impulse);
            }
        }
        else //если не видит лису
        {
            if (((colliders.Length > 0) && colliders.All(x => !x.GetComponent<Character>()) || (nocolliders.Length < 1)) && napravlenie != Vector3.zero)//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа
            {
                napravlenie *= -1;
            }
            else if (napravlenie == Vector3.zero)
            {
                napravlenie = -Vector3.right;//если лис стоял, а игрок потерялся из зоны видимости идти налево
            }
        }


        //условие движения
        if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот
        }

    }


    private void CheckGround()
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)
    }

}
