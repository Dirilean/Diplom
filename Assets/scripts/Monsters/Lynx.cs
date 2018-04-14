using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lynx : Monster
{

    Character Player;

    float DistanceSee;//видимость

    public bool isGrounded;
    float betveen;//расстояние между рысью и игроком в плоскости х
    bool jumping;//прыгнули ли уже


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
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.6F + transform.right * napravlenie.x * 0.8F, 0.1F);

        //условие поворота и прыжок
        if (((betveen < DistanceSee)&&(Mathf.Abs(Player.transform.position.y - transform.position.y)<DistanceSee)))//если видит лису
        {

            //смена направления
            //если над или под игроком на платформах разной высоты
            if ((betveen > 2f)&&((Mathf.Abs(Player.transform.position.y - transform.position.y) < 1F)&&(Player.isGrounded==true)))
            {
                napravlenie = ((Player.transform.position.x - transform.position.x > 0) ? Vector3.right : -Vector3.right);//поворот к игроку
            }

            //прыжок
            if ((Player.transform.position.y - transform.position.y > 1F) && ((Player.transform.position.y - transform.position.y < 3F))//смотрим по У
            && (betveen < 3F) && (betveen > 2F)//смотрим по Х
            && (Player.isGrounded == true) && (isGrounded == true))//для прыжка
            {
                //прикладываем силу вверх, чтобы персонаж подпрыгнул
                rb.AddForce(new Vector3(10F * napravlenie.x, 20), ForceMode2D.Impulse);
            }
        }
        else //если не видит лису
        {
            if (((colliders.Length > 0)&&colliders.Any(x => x.CompareTag("Platform")) && colliders.All(x => !x.GetComponent<Character>()) || (nocolliders.Length < 1)) && napravlenie != Vector3.zero)//перевернуть при условии появления в области платформ или пустоты
            {
                napravlenie *= -1;
            }
            else if (napravlenie == Vector3.zero)
            {
                napravlenie = -Vector3.right;//если лис стоял, а игрок потерялся из зоны видимости идти налево
            }
        }


        //условие движения
        if (!isplayer)//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот
        }

        if (isGrounded)
        {
            State = CharState.walk;
        }
        else
        {
            State = CharState.jump;
        }
    }


    private void CheckGround()
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position-0.6F*Vector3.up, 0.1F);
        isGrounded = colliders.Length > 0; // колайдера всегда внутри (кол. персонажа)
    }

}
