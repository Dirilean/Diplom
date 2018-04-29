using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameObject parent; //тот, кто пускает эту пулю
    public GameObject Parent { set { parent = value; } }

    public float speed;
    public float CurrentSpeed;
    [HideInInspector]
    public Vector3 napravlenie; //направление пули
    public int damage;
    public float lifetime;
    public int minusFire;//сколько отнимается огня на действие
    [Tooltip ("анимировать ли обьект?")]
    public bool animate=false;
    Color CurrenAlfa;

    private void Start()
    {
       CurrenAlfa=GetComponent<SpriteRenderer>().color;
    }

    IEnumerator ForBullet()
    {
        if (animate)
        {
            transform.localScale = Vector3.zero;//маленький
            CurrenAlfa= new Color(255,255,255,0);//прозрачный
        }
        yield return new WaitForSeconds(lifetime);
        GetComponent<PoolObject>().ReturnToPool();//"удаление" объекта
    }

    private void OnEnable()
    {
        StartCoroutine(ForBullet());
        CurrentSpeed = speed;
    }

    private void Update() //движение огня
    {
        if (animate)//постепенное увеличивание и увеличение прозрачности
        {
            CurrenAlfa.a = Mathf.Lerp(CurrenAlfa.a,255F,Time.deltaTime);
            transform.localScale = new Vector3(Mathf.Lerp(transform.localScale.x, 1F, Time.deltaTime), Mathf.Lerp(transform.localScale.y, 1F, Time.deltaTime*2));
        }
        transform.position = Vector3.MoveTowards(transform.position, transform.position + napravlenie, CurrentSpeed* Time.deltaTime);
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
            unit.lives-=damage;//получение урона от пули
            GetComponent<PoolObject>().ReturnToPool();
        }
    }
}
