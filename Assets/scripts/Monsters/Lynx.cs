using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lynx : Monster
{


    float DistanceSee;//видимость
    public bool isGrounded;
    public float betveen;//расстояние между рысью и игроком в плоскости х
   // private bool jumping;//прыгнули ли уже


    private void OnEnable()
    {
        lives = DefaultLives;
        speed = DefaultSpeed;
        die = false;
        napravlenie = transform.right;//начальное направление вправо
        DistanceSee = 5;
    }


     private void FixedUpdate()
    {
        CheckGround();
    }


    public override void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * napravlenie.x * 0.5F, 0.01F,1 << 13);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.6F + transform.right * napravlenie.x * 0.8F, 0.1F, 1 << 13);
        //условие поворота и прыжок

        if (Player!=null)//если видит лису
        {
            //смена направления
            betveen = Player.transform.position.x-transform.position.x;
            //если над или под игроком на платформах разной высоты
            if ((Mathf.Abs(Player.transform.position.y - transform.position.y) < 1F)&&(Player.isGrounded==true))
            {
                if (betveen < 0F)//поворот к игроку
                {
                    napravlenie = -Vector3.right;
                }
                else
                {
                    napravlenie = Vector3.right;
                }
            }

            //прыжок
            betveen = Mathf.Abs(betveen);
            if ((Player.transform.position.y - transform.position.y > 1F) && ((Player.transform.position.y - transform.position.y < 3F))//смотрим по У
            && (betveen < 3F) && (betveen > 2F)//смотрим по Х
            && (Player.isGrounded == true) && (isGrounded == true))//для прыжка
            {
                //прикладываем силу вверх, чтобы персонаж подпрыгнул
                rb.AddForce(new Vector3(10F * napravlenie.x, 15), ForceMode2D.Impulse);
            }
        }
        else //если не видит лису
        {
            if (((colliders.Length > 0) || (nocolliders.Length < 1)) && napravlenie != Vector3.zero)//перевернуть при условии появления в области платформ или пустоты
            {
                napravlenie *= -1;
            }
            else if (napravlenie == Vector3.zero)
            {
                napravlenie = -Vector3.right;//если лис стоял, а игрок потерялся из зоны видимости идти налево
            }
        }


        //условие движения
        if (!playerNear)//идет если не врежется в персонажа
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
