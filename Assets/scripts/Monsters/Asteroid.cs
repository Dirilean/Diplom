using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Asteroid : MonoBehaviour {

    [SerializeField]
    int live;
    [SerializeField]
    int Damage;//количество наносимого урона
    [SerializeField]
    bool Udar;//одиночный удар уже был нанесен?
    [SerializeField]
    bool dvig;

   public GameObject asteroidboom;
    Vector3 t;

    private void OnEnable()
    {
        if (dvig)
        {
            transform.rotation = new Quaternion(0, 0, Random.Range(0F, 360F),0);
            t = new Vector3(Random.Range(-60F, 60F), Random.Range(-50F, 50F));
            GetComponent<Rigidbody2D>().AddForce(t);
        }
    }

    private void Update()
    {
        if (live < 1)
        { 
            GetComponent<PoolObject>().ReturnToPool();//"удаление объекта"
        }
    }

    private void OnDisable()
    {
        if (asteroidboom != null)
        {
            GameObject Asterboom = PoolManager.GetObject(asteroidboom.name, transform.position, transform.rotation);
            Debug.Log(Asterboom.transform.position);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        Character unit = collision.collider.GetComponent<Character>();
        if (unit)//Обнуляем флаг одиночного удара
        {
            Udar = false;
        }
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Character unit =collision.collider.GetComponent<Character>();
        if (unit)//Одиночный удар
        {
            unit.lives = unit.lives - Damage;
            Udar = true;
        }
        else if (collision.collider.GetComponent<Asteroid>())
        {
            live -= Damage;
        }
    }

}
