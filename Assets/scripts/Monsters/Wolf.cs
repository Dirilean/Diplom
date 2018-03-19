using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Wolf : Monster {



    [SerializeField]
    Rigidbody2D rb;
  
    private bool isplayer;//проверка на игрока впереди
    private Vector3 napravlenie;
    public Vector2 point;

    public CharState State
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        napravlenie = transform.right;//начальное направление вправо
        lives = 40;
        Damage = 10;
    }


    private void FixedUpdate()
    {
        Move();
        
    }

    private void Update()
    {
        if (lives <= 0) { Die(); }
        //ближний бой
        point = new Vector2(transform.position.x + (Dalnost*napravlenie.x), transform.position.y+0.5F);//текущая позция удара
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        if (colliders.Length > 0)
        {
            State = CharState.attack;
            if (Time.time >= TimeToDamage + LastTime)//Удар в ближнем бою
            {
                DoDamage(point, radius, layerMask, Damage); //точка удара, радиус поражения, слой врага, урон
                LastTime = Time.time;
            }
        }
    }

    private void Move()
    {

        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 0.01F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(point.x,point.y-0.7F), 0.3F);
        //условие поворота
        if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()) && colliders.All(x => !x.GetComponent<FireSphere>())) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа, и огоньков
        if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
        {          
            rb.velocity = new Vector2(speed*napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот}
            State = CharState.walk;
        }  
    }

    public enum CharState
    {
        walk,
        attack,
        die
    }
}
