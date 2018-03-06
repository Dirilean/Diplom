using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fire : MonoBehaviour
{
    private GameObject parent; //тот, кто пускает эту пулю
    public GameObject Parent { set { parent = value; } }
    
    public GameObject fire;
    private float speed=100.0F;
    private Vector3 napravlenie; //направление пули

    public Vector3 Napravlenie { set { napravlenie = value; } } //берем из вне

    private SpriteRenderer sprite;

    private void Start()
    {
        Destroy(gameObject, 1.6F); //уничтожить объект с задержкой в 1,6сек
    }

    private void Update() //движение огня
    {
        transform.position = Vector3.MoveTowards(transform.position, transform.position + napravlenie, speed * Time.deltaTime);
    }
	
    private void OnTriggerEnter2D(Collider2D collider)//уничтожение пули в момент попадания в юнит
    {
        Unit unit = collider.GetComponent<Unit>();

        if (unit && unit.gameObject!=parent)
        {
            unit.GetDamage();//получение урона от пули
            Destroy(gameObject);//уничтожение около врага, а не родителя
        }
    }
}
