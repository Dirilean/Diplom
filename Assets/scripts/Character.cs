using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug.Log("прыг") обращение к консоли

public class Character : Unit
{
    [SerializeField]
    private int FireCount = 100;

    [SerializeField]
    private float speed = 3.3F;
    [SerializeField]
    public Fire FirePrefab;
    public Monster MonsterPrefab;
    public GUIText FireGui;
    [SerializeField]
    private Rigidbody2D rb;
    public float move;
    float ForJump;
    private bool isGrounded=false; //проверка, стоит ли на земле
    int MinXP=50;//минимальное хп, при котором прекращаются выстрелы



    private void Start()
    {
        ForJump = 0;
    }

    private void FixedUpdate()
    {
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
            Vector3 napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x < 0.0F;
            }
    }

    private void Update()
    {
        if (Input.GetButtonDown("Fire1")) Shoot();
        //если персонаж на земле и нажат пробел и прошло минимальное время для сл. прыжка... 
        {
            if (isGrounded && Input.GetButton("Jump") && (ForJump + 0.5F < Time.time))
            {
                //прикладываем силу вверх, чтобы персонаж подпрыгнул
                rb.AddForce(new Vector2(0, 72), ForceMode2D.Impulse);
                ForJump = Time.time;//записываем время прыжка
            }
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

        if (FireCount > MinXP)
        {
            Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F), transform.position.y + 0.7F);//место создания пули относительно персонажа
            Fire cloneFire = Instantiate<Fire>(FirePrefab, position, FirePrefab.transform.rotation);//создание экземпляра огня(пули)
            cloneFire.Napravlenie = cloneFire.transform.right * (GetComponent<SpriteRenderer>().flipX ? -0.3F : 0.3F);//задаем направление и скорость пули (?если  true : false)
            cloneFire.Parent = gameObject;//родителем пули является текущий объект
            FireCount--;
            FireGui.text = "Огня: " + FireCount;
        }
    }

    private void Damage()//ближний бой
    {

    }

    public override void GetDamage()//получение урона
    {
        Debug.Log("ай");
        FireCount--;
        GetComponent<Rigidbody2D>().AddForce(transform.up*3, ForceMode2D.Impulse);//отпрыгиваем при получении удара
        FireGui.text = "Огня: " + FireCount;
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.GetComponent<FireSphere>())//собирание огоньков
        {
            Destroy(collision.gameObject);
            FireCount++;
            FireGui.text = "Огня: " + FireCount;
        }
    }
}
