using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour {

    public Vector3 RespPos;//позиция воскрешения
    Rigidbody2D rb;
    float speed=0.3F;
    public Character Player;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        transform.position += Vector3.right*Time.deltaTime;
        rb.velocity = new Vector2(speed, rb.velocity.y);

        //10F - расстояние свободного хода
        if (Player.transform.position.x-transform.position.x>50F)//если расстояние между игроком и удаляющим объектом больше 100
        {
            speed = Player.speed + 2F;
        }
        if (Player.transform.position.x - transform.position.x <40F)
        {
            speed = 0.3F;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.GetComponent<Character>())
        {
            collision.GetComponent<Character>().lives = 0;
            RespPos = new Vector3(transform.position.x +20F, 4.5F);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Character>())//если не игрок
        {
            if (collision.GetComponent<PoolObject>())
            {
                collision.GetComponent<PoolObject>().ReturnToPool();//деактивация объекта
            }
            else
            {
                Destroy(collision.gameObject);//реальное удаление объекта
            }
        }

    }
}
