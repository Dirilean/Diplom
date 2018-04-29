using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameObject parent; //тот, кто пускает эту пулю
    public GameObject Parent { set { parent = value; } }
    
    public GameObject fire;
    private float speed=6.0F;
    private Vector3 napravlenie; //направление пули

    public Vector3 Napravlenie { set { napravlenie = value; } } //берем из вне

    private SpriteRenderer sprite;

    IEnumerator ForBullet()
    {
        yield return new WaitForSeconds(2.0F);
        GetComponent<PoolObject>().ReturnToPool();//"удаление" объекта
    }

    private void OnEnable()
    {
        StartCoroutine(ForBullet());
    }

    private void Update() //движение огня
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + napravlenie, speed * Time.deltaTime);
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.01F, 1 << 13);
        if (colliders.Length>0)//уничтожение пули при касании с платформой
        {
            GetComponent<PoolObject>().ReturnToPool();
        }
    }
	
    private void OnTriggerEnter2D(Collider2D collider)//уничтожение пули в момент попадания в юнит
    {
        Monster unit = collider.GetComponent<Monster>();

        if (unit&& gameObject != parent)
        {
            unit.lives-=20;//получение урона от пули
            GetComponent<PoolObject>().ReturnToPool();
        }
    }
}
