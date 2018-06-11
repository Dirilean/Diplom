using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Deleter : MonoBehaviour {

    public Vector3 RespPos;//позиция воскрешения
    Rigidbody2D rb;
    [SerializeField]
    float speed;
    [SerializeField]
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
        switch(player.PrefabLevel)
        {
            case 1: RespPos = new Vector3(transform.position.x + 20F,  3.5F); break;
            case 2: RespPos = new Vector3(transform.position.x + 20F,  13.5F); break;
            case 3: RespPos = new Vector3(transform.position.x + 20F,  23.5F); break;
        }
        

        if (player.isActiveAndEnabled == false)
        {
            if (player.name == "Player") player = GameObject.Find("Player2(Clone)").GetComponent<Character>();
            else if (player.name == "Player2(Clone)") player = GameObject.Find("Player3(Clone)").GetComponent<Character>();
        }


        //transform.position += Vector3.right * Time.deltaTime;
         rb.velocity = new Vector2(speed, rb.velocity.y);
        
        distance = player.transform.position.x - transform.position.x;
        if (distance > 50F)
        {
            speed = player.speed;
        }
        else
        {
            speed = 0F;
        }

        //rb.AddForce(new Vector2(speed, 0F),ForceMode2D.Force);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
      if(collision.GetComponent<Character>())
        {
            collision.GetComponent<Character>().lives -=50;
            collision.transform.position = RespPos;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!collision.GetComponent<Character>())//если не игрок и не конечная платформа
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
