using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour {

    public Vector3 RespPos;//позиция воскрешения
    Rigidbody2D rb;
    float speed=0.01F;
    Character player;
    [SerializeField]
    float distance;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        player = GameObject.Find("Player").GetComponent<Character>();
    }

    private void Update()
    {

        if (player.isActiveAndEnabled == false)
        {
            if (player.name == "Player") player = GameObject.Find("Player2(Clone)").GetComponent<Character>();
            else if (player.name == "Player2(Clone)") player = GameObject.Find("Player3(Clone)").GetComponent<Character>();
        }


        transform.position += Vector3.right * Time.deltaTime;
        rb.velocity = new Vector2(speed, rb.velocity.y);
        distance = player.transform.position.x - transform.position.x;
        //10F - расстояние свободного хода
        if (distance > 50F)//если расстояние между игроком и удаляющим объектом больше 100
        {
            speed = player.speed;
        }
        if (distance < 10F)
        {
            speed = 0.01F;
        }
        else if (distance < 40F)
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
        if (!collision.GetComponent<Character>())//&&!collision.GetComponent<EndLevel>())//если не игрок и не конечная платформа
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
