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
    int MinFireColb=0;//минимальное хп, при котором прекращаются выстрелы
    bool CheckJump;
    Vector3 napravlenie;//куда смотрит игрок
    float TimeToPlusLives;//время перезарядки конвертации жизни
    float LastTimeToPlusLives;//последнее время конвертьации жизней
    

    float timeDie;//время смерти
    float zaderzhka = 1;//сколько секунд после смерти нужно ждать чтобы воскреснуть

    float TimeShoot;//время выстрела
    float delayShooy=0.3F;//задержка при выстрелах

    float TimeDoDamage;//время удара в ближнем бою
    float delayDoDamage= 0.3F;//задержка при ударах
    public int damagehit;//урон в ближнем бою
    public float damagehitdistanse;//дальность ближнего боя


    private void Start()
    {
        ForJump = 0;
        CheckJump = false;
        lives = 100;
        speed = 3.5F;
        TimeToPlusLives = 5;
        LastTimeToPlusLives = 0;
        TimeShoot = Time.time;
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

        if (isGrounded && Input.GetButton("Jump") && (CheckJump == false))//прыжок 
        {
            //прикладываем силу чтобы персонаж подпрыгнул
            rb.AddForce(new Vector3(10F*Input.GetAxis("Horizontal"), 72), ForceMode2D.Impulse);
            CheckJump = true;

        }
        if (((Input.GetButton("Jump")) == false)&& isGrounded)//если отпустили клавишу прыжка
        {
            CheckJump = false;
        }
    }

    private void Update()
    {
        FireGui.text = "Огня: " + FireColb;
        LivesGui.text = "Жизней: " + lives;

        if (lives <= 0) { Die(); }

        if (Input.GetButtonDown("Fire2")) Shoot();//выстрел
        if (Input.GetButtonDown("Fire1")) DoDamage();//ближний бой
        if (Input.GetButtonDown("Lives")&&(lives<=80)) ConvertToLives();//поменять огонь на жизни
    }

    private void CheckGround()//проверка стоит ли персонаж на земле
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)
    }

    private void DoDamage()//урон в ближнем бою
    {
        if (TimeDoDamage + delayDoDamage < Time.time)
        {
            RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5F), Vector2.right * napravlenie.x, 2F, 1 << 11);
            hit.collider.gameObject.GetComponent<Monster>().lives -= damagehit;
            TimeDoDamage = Time.time;
        }
    }

    private void Shoot()//выстрелы
    {
        if ((FireColb> MinFireColb)&&(TimeShoot+delayShooy<Time.time))
        {
            Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F), transform.position.y + 0.7F);//место создания пули относительно персонажа
            Fire cloneFire = Instantiate(FirePrefab, position, FirePrefab.transform.rotation);//создание экземпляра огня(пули)
            cloneFire.Napravlenie = cloneFire.transform.right * (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F);//задаем направление и скорость пули (?если  true : false)
            cloneFire.Parent = gameObject;//родителем пули является текущий объект
            FireColb--;
            TimeShoot = Time.time;
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

    private void ConvertToLives()//конвертирование огня в жизнь
    {
        if ((FireColb >= 50)&&(Time.time>LastTimeToPlusLives+TimeToPlusLives))//в колбе достаточно огня и прошло время перезарядки
        {
            FireColb = FireColb - 50;
            lives = lives + 20;//добавляем жизней
            LastTimeToPlusLives = Time.time;
        }
    }

    void Die()//смерть персонажа
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.black;
        lives = 0;
        timeDie = Time.time;
        if (timeDie + zaderzhka > Time.time)
        {
            transform.position = new Vector3(0, 5);
            gameObject.GetComponent<SpriteRenderer>().color = Color.white;
            lives = 100;
            FireColb = 0;
        }
    }

}
