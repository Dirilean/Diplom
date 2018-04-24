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
    private Rigidbody2D rb;
    float ForJump;
    public bool isGrounded=false; //проверка, стоит ли на земле
    int MinFireColb=0;//минимальное хп, при котором прекращаются выстрелы
    [SerializeField]
    bool CheckJump;
    Vector3 napravlenie;//куда смотрит игрок
    public float TimeToPlusLives;//время перезарядки конвертации жизни
    public float LastTimeToPlusLives;//последнее время конвертьации жизней
    

    float timeDie;//время смерти
    float zaderzhka = 1;//сколько секунд после смерти нужно ждать чтобы воскреснуть

    float TimeShoot;//время выстрела
    float delayShooy=1F;//задержка при выстрелах

    float TimeDoDamage;//время удара в ближнем бою
    float delayDoDamage= 1F;//задержка при ударах
    public int damagehit;//урон в ближнем бою
    public float damagehitdistanse;//дальность ближнего боя
    float dalnost=2F;//дальность удара ближнего боя

    float deltaColor;//для плавного изменения цвета игрока

    public Animator animator;
   
    public CharState State//передаем состояние анимации в аниматор
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    [SerializeField]
    bool isstay;//проигрываем анимацию простоя?

    delegate void Method();//для передачи методов атаки в корутину
    [SerializeField]
    bool attack;//атакуем в данный момент?

    private void Start()
    {
        ForJump = 0;
        CheckJump = false;
        lives = 100;
        speed = 3.5F;
        TimeToPlusLives = 5;
        LastTimeToPlusLives = 0;
        TimeShoot = Time.time;
        FireColb = 50;
        napravlenie = Vector3.right;
        isstay = true;
        attack = false;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetAxis("Horizontal") != 0)//обычное хождение
        {
            napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x > 0.0F;
            State = CharState.walk;

        }
        else if ((isstay == true))//анимация простоя
        {
            //Debug.Log(Time.time+", "+State);
            State = CharState.stay;
        }

        if (isGrounded && Input.GetButton("Jump") && (CheckJump == false))//прыжок 
        {
            State = CharState.stay;
            //прикладываем силу чтобы персонаж подпрыгнул
            rb.AddForce(new Vector3(10F * Input.GetAxis("Horizontal"), 72), ForceMode2D.Impulse);
            CheckJump = true;
        }
        if (((Input.GetButton("Jump")) == false) && isGrounded)//если отпустили клавишу прыжка
        {
            CheckJump = false;
        }


        if (lives <= 0) { Die(); }

        if ((Input.GetButtonDown("Fire2"))&&(attack==false)) StartCoroutine(ForAnimate(delayShooy, 0.4F, CharState.attack, Shoot));//выстрел
        if ((Input.GetButtonDown("Fire1")) && (attack ==false)) StartCoroutine(ForAnimate(delayDoDamage, 0.4F, CharState.attack, DoDamage)); //ближний бой
        if (Input.GetButtonDown("Lives")&&(lives<100)) ConvertToLives();//поменять огонь на жизни

        deltaColor = Mathf.Lerp(deltaColor, (lives / 100.0F), Time.deltaTime * 2);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(deltaColor, deltaColor, deltaColor);//меняем цвет лисицы
    }


    IEnumerator ForAnimate(float time, float delay, CharState state,Method metod)// для задержки анимации (время между кликами, через сколько после начала анимации ударить, анимация удара, метод удара)
    {
        isstay = false;//завершаем проигрывать анимацию простоя
        State = state;//начинаем проирывать текущую анимацию
        yield return new WaitForSeconds(delay);
        metod.Invoke();//вызываем метод атаки
        attack = true;
        yield return new WaitForSeconds(delay);//пережидаем все время анимации удара
        attack = false;
        isstay = true;//заного включаем анимацию простоя 
    }

    private void CheckGround()//проверка стоит ли персонаж на земле
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)
    }

    private void DoDamage()//урон в ближнем бою
    {
        RaycastHit2D hit = Physics2D.Raycast(new Vector2(transform.position.x, transform.position.y + 0.5F), Vector2.right * napravlenie.x, dalnost, 1 << 11);
        if (hit == true)
        {
            hit.collider.gameObject.GetComponent<Monster>().lives -= damagehit;
            hit.collider.gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }

    private void Shoot()//выстрелы
    {

        Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F), transform.position.y + 0.7F);//место создания пули относительно персонажа
        Fire cloneFire = Instantiate(FirePrefab, position, FirePrefab.transform.rotation);//создание экземпляра огня(пули)
        cloneFire.Napravlenie = cloneFire.transform.right * (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F);//задаем направление и скорость пули (?если  true : false)
        cloneFire.Parent = gameObject;//родителем пули является текущий объект
        FireColb--;
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
        if ((FireColb >= 40)&&(Time.time>LastTimeToPlusLives+TimeToPlusLives))//в колбе достаточно огня и прошло время перезарядки
        {
            if (lives <= 80)
            {
                FireColb -= 40;
                lives = lives + 20;//добавляем жизней
            }
            else//если хп больше 80
            {
                FireColb = FireColb - (100 - lives) * 2;
                lives = 100;
            }
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

    public enum CharState
    {
        stay,
        walk,
        attack,
        jump,
        shoot     
    }
}
