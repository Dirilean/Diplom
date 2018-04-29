using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ezh : Monster {

    Vector3 povorot= new Vector3(1,0,0);
    float Predpos;
    float Pos;
    float LastProv=1F;//последняя проверка на движение
    float LastTimeProv;
    bool once=false;//только один раз сталкиваемся с игроком

    private void OnEnable()
    {
        lives = DefaultLives;
        speed = DefaultSpeed;
        once = false;
    }

    private void FixedUpdate()
    {
        rb.velocity = new Vector2(speed * povorot.x, 0);

        Predpos = Pos;
        Pos = transform.position.x;

        if (LastProv + LastTimeProv < Time.time)
        {
            if (Predpos == transform.position.x)//если мы никуда не продвинулись
            { povorot = povorot * -1;
            }//вращение в другую сторону
            LastTimeProv = Time.time;
        }
    }

    private void Update()
    {
        //проверяем на живучесть
        if ((lives < 1) && (die == false)) { Die(); }
    }

    private void OnCollisionEnter2D(Collision2D collider)//столкновение с игроком
    {
        if ((collider.gameObject.name == "Player")&&(once==false))
        {
            collider.gameObject.GetComponent<Character>().lives -=Damage;
            lives = 0;
            once = true;
        }
        if (collider.gameObject.tag == "Mob")
        {
            povorot = povorot * -1;
        }
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(0.3F);
        GetComponent<PoolObject>().ReturnToPool();
    }
}
