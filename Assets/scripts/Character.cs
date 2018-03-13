using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug.Log("прыг") обращение к консоли

public class Character : Unit
{
    [SerializeField]
    public int FireColb = 100;//количество огня в колбе
    [SerializeField]
    public int lives;//количество жизней
    [SerializeField]
    private float speed;
    [SerializeField]
    private float TempSpeed;//временная скорость (изменяется)
    [SerializeField]
    public Fire FirePrefab;
    [SerializeField]
    public GUIText FireGui;
    [SerializeField]
    public GUIText LivesGui;
    [SerializeField]
    private Rigidbody2D rb;
    float ForJump;
    public bool isGrounded=false; //проверка, стоит ли на земле
    int MinXP=50;//минимальное хп, при котором прекращаются выстрелы
    bool CheckJump;
    bool IsEnemy;
    Vector3 napravlenie;
    int ProcentFireInColb;
    float TimeToPlusLives;//время перезарядки конвертации жизни
    float LastTimeToPlusLives;
    


    private void Start()
    {
        ForJump = 0;
        CheckJump = false;
        lives = 100;
        speed = 3.5F;
       // TempSpeed = speed;
        TimeToPlusLives = 5;
        LastTimeToPlusLives = 0;

    }

    private void FixedUpdate()
    {
        CheckGround();
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetAxis("Horizontal") != 0)//обычное хождение
        {
            napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;  
        }

        if (isGrounded && Input.GetButton("Jump") && CheckJump == false)//прыжок 
        {
            //прикладываем силу чтобы персонаж подпрыгнул
            rb.AddForce(new Vector3(transform.position.x, transform.position.y + 72), ForceMode2D.Impulse);
            CheckJump = true;
           // TempSpeed = speed * 1.1F;
        }
        if (((Input.GetButton("Jump")) == false)&& isGrounded)//если отпустили клавишу прыжка
        {
            CheckJump = false;
           // TempSpeed = speed;
        }
    }

    private void Update()
    {
        FireGui.text = "Огня: " + FireColb;
        LivesGui.text = "Жизней: " + lives;

        if (lives <= 0) { Destroy(gameObject); }

        if (Input.GetButtonDown("Fire2")) Shoot();//выстрел
        if (Input.GetButtonDown("Fire1"))//ближний бой
        {
            DoDamage(new Vector2(transform.position.x+(0.55F* (GetComponent<SpriteRenderer>().flipX ? -1F : 1F)),this.transform.position.y+0.45F),0.3F, 11, 15, false); //точка удара, радиус поражения, слой врага, урон, на всех?
        }
        if (Input.GetButtonDown("Lives")) ConvertToLives();//поменять огонь на жизни
    }


    private void CheckGround()
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
            isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)

    }

    private void Shoot()//выстрелы
    {

        if (FireColb> MinXP)
        {
            Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F), transform.position.y + 0.7F);//место создания пули относительно персонажа
            Fire cloneFire = Instantiate<Fire>(FirePrefab, position, FirePrefab.transform.rotation);//создание экземпляра огня(пули)
            cloneFire.Napravlenie = cloneFire.transform.right * (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F);//задаем направление и скорость пули (?если  true : false)
            cloneFire.Parent = gameObject;//родителем пули является текущий объект
            FireColb--;

        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<FireSphere>())//собирание огоньков
        {
            Destroy(collision.gameObject);
            FireColb = FireColb + 5 ;
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

    // point - точка контакта
    // radius - радиус поражения
    // layerMask - номер слоя, с которым будет взаимодействие
    // damage - наносимый урон
    // allTargets - должны-ли получить урон все цели, попавшие в зону поражения
    public static void DoDamage(Vector2 point, float radius, int layerMask, int damage, bool allTargets)//ближний бой
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        if (!allTargets)//по 1 тарегу
        {
            GameObject obj = NearTarget(point, colliders);
            if (obj.GetComponent<Monster>())
            {
                obj.GetComponent <SpriteRenderer>().color=Color.red;
                obj.GetComponent<Monster>().lives -= damage;
            }
            return;
        }

        foreach (Collider2D hit in colliders)//по всем
        {
            if (hit.GetComponent<Monster>())
            {
                hit.GetComponent<Monster>().lives -= damage;
            }
        }
    }


    private void ConvertToLives()
    {
        if ((FireColb >= 50)&&(Time.time>LastTimeToPlusLives+TimeToPlusLives))//в колбе достаточно огня и прошло время перезарядки
        {
            FireColb = FireColb - 50;
            lives = lives + 20;//добавляем жизней
            LastTimeToPlusLives = Time.time;
        }
    }



}
