using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Lynx : Monster
{



    //[SerializeField]
    //Rigidbody2D rb;
    //[SerializeField]
    //Character Player;

    //float DistanceSee;//видимость

    //private bool isplayer;//проверка на игрока впереди
    //[SerializeField]
    //private Vector3 napravlenie;
    //bool CheckJump;
    //bool isGrounded;

    //private void Start()
    //{
    //    napravlenie = transform.right;//начальное направление вправо
    //    lives = 40;
    //    Damage = 10;
    //    DistanceSee = 5;
    //}


    //private void FixedUpdate()
    //{
    //    Move();
    //    CheckGround();
    //}

    //private void Update()
    //{
    //    if (lives <= 0) { Die(); }
    //    //ближний бой
    //    Vector2 point = new Vector2(transform.position.x + (Dalnost), transform.position.y);//текущая позция удара
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
    //    if ((colliders.Length > 0) && (Time.time >= TimeToDamage + LastTime))//Удар в ближнем бою
    //    {
    //        //Debug.Log("point " + point +", radius" + radius + ", layer" + layerMask + ", damage" + Damage + ", lasttime" + LastTime + ", time" +Time.time);
    //        DoDamage(point, radius, layerMask, Damage); //точка удара, радиус поражения, слой врага, урон
    //        LastTime = Time.time;
    //    }
    //}

    //private void Move()
    //{
    //    //стенки
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * napravlenie.x * 0.5F, 0.01F);
    //    //пустота
    //    Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.3F + transform.right * napravlenie.x * 0.5F, 0.3F);
       
    //    //условие поворота
    //    if (Vector3.Distance(Player.transform.position,transform.position)<DistanceSee)//если видит лису
    //    {
    //        if (Mathf.Abs(Player.transform.position.x - transform.position.x) < 0.3f)//если над или под
    //        {
    //            napravlenie = Vector3.zero;
    //        }
    //        else
    //        {
    //            napravlenie = ((Player.transform.position.x - transform.position.x > 0) ? Vector3.right : -Vector3.right);//поворот к игроку
    //        }
    //    }
    //    else //если не видит лису
    //    {
    //        if (((colliders.Length > 0) && colliders.All(x => !x.GetComponent<Character>()) || (nocolliders.Length < 1))&&napravlenie!=Vector3.zero)//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа
    //        {
    //            napravlenie *= -1;
    //        }
    //        else if (napravlenie == Vector3.zero)
    //        {
    //            napravlenie = -Vector3.right;//если лис стоял, а игрок потерялся из зоны видимости идти налево
    //        }
    //    }


    //    if ((Player.transform.position.y - transform.position.y > 1.5F)&&((Player.transform.position.y - transform.position.y < 2.5F))//смотрим по У
    //        &&(Mathf.Abs(Player.transform.position.x - transform.position.x)< 3F)&&((Mathf.Abs(Player.transform.position.x - transform.position.x) > 2F))//смотрим по Х
    //        &&(Player.isGrounded==true)&&(isGrounded==true))//для прыжка
    //    {
    //        //прикладываем силу вверх, чтобы персонаж подпрыгнул
    //        rb.AddForce(new Vector3(0,300), ForceMode2D.Impulse);
    //        CheckJump = true;
    //    }

    //    //условие движения
    //    if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
    //    {
    //        rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
    //        GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот
    //        CheckJump = false;
    //    }

    //}


    //private void CheckGround()
    //{
    //    //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
    //    Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
    //    isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)
    //}

}
