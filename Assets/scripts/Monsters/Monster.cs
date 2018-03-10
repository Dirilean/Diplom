using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Monster : Unit

{
    [SerializeField]
    public int Damage;//количество наносимого урона
    [SerializeField]
    public float TimeToDamage;//время за которое наносятся один удар
    [SerializeField]
    public int lives;//жизни
    [SerializeField]
    public float speed;//скорость передвижения
    [SerializeField]
    public int PlusFireColb;//сколько упадет огня с монстров


    public float radius;//радиус удара
    public int layerMask;//слой "жертвы" (игрок)
    public float Dalnost;//дальность удара (центр окружности)
    public float LastTime;//Время последнего удара

    private void Start()
    {
        Dalnost = 0.5F;
        radius = 0.3F;
        layerMask = 10;
        LastTime = 0;
    }

    private void Update()
    {
        if (lives <= 0) Destroy(gameObject);
        Vector2 point = new Vector2(transform.position.x + (Dalnost ), transform.position.y);//текущая позция удара
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        if ((colliders.Length > 0)&& (Time.time - TimeToDamage > LastTime))//Удар в ближнем бою
        {
            DoDamage(point, radius, layerMask, Damage); //точка удара, радиус поражения, слой врага, урон
            LastTime = LastTime + Time.time;
        }
        
    }


    // функция возвращает ближайший объект из массива, относительно указанной позиции
    static GameObject NearTarget(Vector3 position, Collider2D[] array)
    {
        Collider2D current = null;
        float dist = Mathf.Infinity;

        foreach (Collider2D coll in array)
        {
            float curDist = Vector3.Distance(position, coll.transform.position);

            if (curDist < dist)
            {
                current = coll;
                dist = curDist;
            }
        }

        return current.gameObject;
    }

    public static void DoDamage(Vector2 point, float radius, int layerMask, int damage)//ближний бой
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        GameObject obj = NearTarget(point, colliders);
        if (obj.GetComponent<Character>())
        {
           Debug.Log("boom");
            obj.GetComponent<SpriteRenderer>().color = Color.red;
            obj.GetComponent<Character>().lives -= damage;
        }
        obj.GetComponent<SpriteRenderer>().color = Color.white;
        return;
    }
}
