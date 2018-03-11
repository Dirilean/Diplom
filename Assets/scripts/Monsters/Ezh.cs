using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ezh : Monster {

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

    private void Update()
    {
        if (lives <= 0) { Die(); }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.3F + transform.right * napravlenie.x * 0.5F, 0.01F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.2F + transform.right * napravlenie.x * 0.5F, 0.1F);
        //условие поворота
        if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>()) && colliders.All(x => !x.GetComponent<FireSphere>())) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа, и огоньков
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот}
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)//смерть и урон от соприкосновения с персонажем
    {
        Character unit = collision.gameObject.GetComponent<Character>();
        if (unit is Character)
        {
            lives = 0;
            unit.lives -= 20;
        }
    }
}
