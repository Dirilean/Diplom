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
    [SerializeField]
    FireSphere FireSpherePrefab;

    public float radius;//радиус удара
    public int layerMask;//слой "жертвы" (игрок)
    public float Dalnost;//дальность удара (центр окружности)
    public float LastTime;//Время последнего удара

    public float XPos;
    public float YPos;
    System.Random rnd = new System.Random();

    private void Start()
    {
        Dalnost = 0.5F;
        radius = 0.3F;
        layerMask = 10;
        LastTime = 0;
    }


    public void Die()
    {
        XPos = gameObject.transform.position.x;
        for (int i = 0; i < PlusFireColb; i++)//генерирование огоньков
        {
            YPos = (float)(rnd.NextDouble())/3+0.3F;//от 0,3 до 0,6
            FireSphere FireSphere = Instantiate(FireSpherePrefab, new Vector2(XPos, gameObject.transform.position.y+YPos), FireSpherePrefab.transform.rotation);
            XPos += 0.5F;
        }
        Destroy(gameObject);
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

            obj.GetComponent<SpriteRenderer>().color = Color.red;
            obj.GetComponent<Character>().lives -= damage;
        }
        obj.GetComponent<SpriteRenderer>().color = Color.white;
        return;
    }
}
