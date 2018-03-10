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
    private float speed = 3.3F;
    [SerializeField]
    public Fire FirePrefab;
    public Monster MonsterPrefab;
    [SerializeField]
    public GUIText FireGui;
    [SerializeField]
    public GUIText LivesGui;
    [SerializeField]
    private Rigidbody2D rb;
    public float move;
    float ForJump;
    private bool isGrounded=false; //проверка, стоит ли на земле
    int MinXP=50;//минимальное хп, при котором прекращаются выстрелы
    bool CheckJump;
    bool IsEnemy;
    Vector3 napravlenie;
    


    private void Start()
    {
        ForJump = 0;
        CheckJump = false;
        lives = 100;
    }

    private void FixedUpdate()
    {
        LivesGui.text ="Жизней: "+lives;
        CheckGround();
        //используем Input.GetAxis для оси Х. метод возвращает значение оси в пределах от -1 до 1.
        //при стандартных настройках проекта 
        //-1 возвращается при нажатии на клавиатуре стрелки влево (или клавиши А),
        //1 возвращается при нажатии на клавиатуре стрелки вправо (или клавиши D)
        move = Input.GetAxis("Horizontal");

        //обращаемся к компоненту персонажа RigidBody2D. задаем ему скорость по оси Х, 
        //равную значению оси Х умноженное на значение макс. скорости
        rb.velocity = new Vector2(move * speed, rb.velocity.y);
        if (Input.GetAxis("Horizontal")!=0)
            {
            napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;
            }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire2")) Shoot();//выстрел
        if (Input.GetButtonDown("Fire1"))//ближний бой
        {
            Character.DoDamage(new Vector2(this.transform.position.x+(0.55F* (GetComponent<SpriteRenderer>().flipX ? -1F : 1F)),this.transform.position.y+0.45F),0.3F, 11, 15, false); //точка удара, радиус поражения, слой врага, урон, на всех?
        }

        if (isGrounded && Input.GetButton("Jump")&&CheckJump==false)//прыжок 
        {
            //прикладываем силу вверх, чтобы персонаж подпрыгнул
            rb.AddForce(new Vector3(0, 72)+this.transform.position, ForceMode2D.Impulse);
            CheckJump = true;
        }
        if ((Input.GetButton("Jump"))==false)//если отпустили клавишу прыжка
        {
            CheckJump = false;
        }
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
            FireGui.text = "Огня: " + FireColb;
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<FireSphere>())//собирание огоньков
        {
            Destroy(collision.gameObject);
            FireColb = FireColb + 5 ;
            FireGui.text = "Огня: " + FireColb;
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
                Debug.Log(point);
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
}
