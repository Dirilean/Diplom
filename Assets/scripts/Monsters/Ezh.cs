using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class Ezh : MonoBehaviour {

    Vector3 povorot= new Vector3(1,0,0);
    float Predpos;
    float Pos;
    float LastProv=1F;//последняя проверка на движение
    float LastTimeProv;

    [SerializeField]
    public int Damage;//количество наносимого урона
    public int DefaultLives;//изначальные жизни
    [HideInInspector]
    public int lives;// текущие жизни
    [SerializeField]
    public float speed;//скорость передвижения
    [SerializeField]
    public int PlusFireColb;//сколько упадет огня с монстров
    [SerializeField]
    FireSphere FireSpherePrefab;
    [SerializeField]
    public Rigidbody2D rb;
    protected float XPos;
    protected float YPos;
    System.Random rnd = new System.Random();
    [SerializeField]
    ParticleSystem Smoke;
    bool die;//запустили уже скрипт умирания?

    private void OnEnable()
    {
        lives = DefaultLives;
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
        if (lives <= 0) {Die(); }
    }

    private void OnCollisionEnter2D(Collision2D collider)//столкновение с игроком
    {
        if (collider.gameObject.name == "Player")
        {
            collider.gameObject.GetComponent<Character>().lives -= 20;
            Die();
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

    public void Die()//смерть персонажа
    {
        die = true;
        XPos = gameObject.transform.position.x;
       // smoke = Instantiate(Smoke, new Vector3(XPos, transform.position.y + 0.5F), gameObject.transform.rotation);//создание дымки после смерти
        GameObject smoke = PoolManager.GetObject(Smoke.name, new Vector3(XPos, transform.position.y + 0.5F), gameObject.transform.rotation);
        int k = 0;
        while (k < PlusFireColb)//генерирование огоньков в зависимости от указанаого в префабе значения
        {
            YPos = (float)(rnd.NextDouble()) / 3 + 0.3F;//от 0,3 до 0,6 для начальной разной высоты
            FireSphere firesphere = PoolManager.GetObject(FireSpherePrefab.name, new Vector2(XPos, gameObject.transform.position.y + YPos), FireSpherePrefab.transform.rotation).GetComponent<FireSphere>();
            //FireSphere FireSphere = Instantiate(FireSpherePrefab, new Vector2(XPos, gameObject.transform.position.y + YPos), FireSpherePrefab.transform.rotation);
            XPos += 0.5F;
            k++;
        }
        speed = 0;

        StartCoroutine(Example());
    }

}
