using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Monster : Unit

{
    [SerializeField]
    private float speed = 2.0F;


    private bool isplayer;//проверка на игрока впереди
    private Vector3 napravlenie;
    int lives = 40;

    private void Start()
    {
        napravlenie = transform.right;//начальное направление вправо
    }

    private void Update()
    {
        Move();
    }

    private void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position + transform.up * 0.5F + transform.right * napravlenie.x * 0.5F, 0.01F);//при попадании в область, коллайдеры записываются в массив
        //если в массиве коллайдеров нет игрока то true
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(transform.position + transform.up * -0.3F + transform.right * napravlenie.x * 0.5F, 0.3F);
        //условие поворота
        if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>())&& colliders.All(x => !x.GetComponent<FireSphere>())) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа, и огоньков
        if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
        {
            transform.position = Vector3.MoveTowards(transform.position, transform.position + napravlenie, speed * Time.deltaTime);//само движение
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;//поворот}
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        //if (tag != "Wolf (1)" && other.gameObject.tag != "Player")
        //        return;
        //Debug.LogFormat("{0} touch {1}", gameObject.name, other.gameObject.name);
        //Debug.Log("Collided with " + other.gameObject.name);
        Unit unit = other.collider.GetComponent<Unit>();
        if (unit && unit is Character)
        {
            unit.GetDamage();
        }
    }


    public override void GetDamage()
    {
        lives=lives-15;
        GetComponent<Rigidbody2D>().AddForce(transform.up * 3, ForceMode2D.Impulse);//отпрыгиваем при получении удара
        if (lives <= 0) Die();
    }
}
