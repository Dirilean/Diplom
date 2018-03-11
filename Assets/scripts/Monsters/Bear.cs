using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Bear : Monster {

    [SerializeField]
    Rigidbody2D rb;

    private bool isplayer;//проверка на игрока впереди
    private Vector3 napravlenie;


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

    private void Move()
    {
        Vector2 q = transform.position + transform.up * -0.1F + transform.right * napravlenie.x*0.7F;
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(q , 0.1F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.8F + transform.right * napravlenie.x * 0.8F, 0.3F);
        Debug.Log(colliders.Length);

        Debug.Log(q+", tr= "+transform.position);
        //условие поворота
        if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()) && colliders.All(x => !x.GetComponent<FireSphere>())) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа, и огоньков
        if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот}
        }

    }
}
