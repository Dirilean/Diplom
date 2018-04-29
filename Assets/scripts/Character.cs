using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug.Log("прыг") обращение к консоли

public class Character : MonoBehaviour
{
    #region Data
    [SerializeField]
    public int FireColb;//количество огня в колбе
    [SerializeField]
    public int lives;//количество жизней
    [SerializeField]
    public float speed;
    [SerializeField]
    private float TempSpeed;//временная скорость (изменяется)
    [SerializeField]
    public Fire FirePrefab;
    [SerializeField]
    private Rigidbody2D rb;
    float ForJump;
    [HideInInspector]
    public bool isGrounded=false; //проверка, стоит ли на земле
    bool lastIsGrounded=true;//для проверки приземления
    bool landing=false;
    int MinFireColb=0;//минимальное хп, при котором прекращаются выстрелы
    bool CheckJump;//находимся ли мы в состоянии прыжка
    Vector3 napravlenie;//куда смотрит игрок
    [HideInInspector]
    public float TimeToPlusLives;//время перезарядки конвертации жизни
    [HideInInspector]
    public float LastTimeToPlusLives;//последнее время конвертьации жизней
    public GameObject AttackWave;//объект волны атаки
    

    float timeDie;//время смерти
    float zaderzhka = 1;//сколько секунд после смерти нужно ждать чтобы воскреснуть
    public Deleter DeleterSmoke;


    float TimeShoot;//время выстрела
    float delayShooy=0.588F;//задержка при выстрелах
    float TimeDoDamage;//время удара в ближнем бою

    float delayDoDamage= 0.588F;//задержка при ударах
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
    
    bool isstay;//проигрываем анимацию простоя?
    bool isrun;//проигрываем анимацию бега?

    delegate void Method();//для передачи методов атаки в корутину
    [SerializeField]
    bool attack;//атакуем в данный момент?

    public AudioClip[] RunSound = new AudioClip[7];//звуки шагов
    float[] times = new float[4] { 0.12F, 0.03F, 0.36F, 0.03F };//тайминг шагов
    int i, j;
    bool checkaudio;
    public AudioClip JumpSound;

    #endregion

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
        isrun = true;
        attack = false;
        //счетчики для звуков шагов
        i = 0;
        j = 0;
        checkaudio = true;
        AuSourse.volume = 0.03F;
    }

    private void FixedUpdate()
    {
        CheckGround();
    }

    private void Update()
    {
        if (lives <= 0) { Die(); }

        #region input left - right
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        if (Input.GetAxis("Horizontal") != 0)//обычное хождение
        {
            napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x > 0.0F;
            if ((CheckJump == false) && (isrun))
            {
                State = CharState.walk;
            }
            if (isGrounded) MakeRunSound();
        }
        else if ((isstay)&&(CheckJump==false))//анимация простоя
        {
            //Debug.Log(Time.time+", "+State);
            State = CharState.stay;
        }
        #endregion

        #region input Jump
        if (isGrounded && Input.GetButton("Jump") && (CheckJump == false))//прыжок 
        {
            State = CharState.jump;
            //прикладываем силу чтобы персонаж подпрыгнул
            rb.AddForce(new Vector3(10F * Input.GetAxis("Horizontal"), 72), ForceMode2D.Impulse);
            CheckJump = true;
        }
        if (((Input.GetButton("Jump")) == false) && isGrounded)//если отпустили клавишу прыжка
        {
            CheckJump = false;
        }
        //звук приземления
        if (lastIsGrounded != isGrounded)
        {
            if (landing) AuSourse.PlayOneShot(JumpSound);
            landing = !landing;
        }
        lastIsGrounded = isGrounded;
        #endregion        

        #region  input Shoot
        if ((Input.GetButtonDown("Fire2")) && (attack == false))
        {
            if (Input.GetAxis("Horizontal") != 0)//если нажали атаку во время бега
            {
                StartCoroutine(ForAnimate(delayShooy,  CharState.run_attack, Shoot));    
            }
            else
            {
                StartCoroutine(ForAnimate(delayShooy, CharState.attack, Shoot));
            }
        }
        #endregion

        #region input Attack
        if ((Input.GetButtonDown("Fire1")) && (attack == false))
        {
            if (Input.GetAxis("Horizontal") != 0)//если нажали атаку во время бега
            {
                StartCoroutine(ForAnimate(delayDoDamage, CharState.run_attack, DoDamage));
            }
            else
            {
                StartCoroutine(ForAnimate(delayDoDamage, CharState.attack, DoDamage));
            }
        }
        #endregion

        #region input ConvertLives
        if (Input.GetButtonDown("Lives")&&(lives<100)) ConvertToLives();//поменять огонь на жизни
        #endregion

        //изменение цвета лисицы от хп
        deltaColor = Mathf.Lerp(deltaColor, (lives / 100.0F), Time.deltaTime * 2);
        gameObject.GetComponent<SpriteRenderer>().color = new Color(deltaColor, deltaColor, deltaColor);   
    }

    private void CheckGround()//проверка стоит ли персонаж на земле
    {
        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.1F);
        isGrounded = colliders.Length > 1; //один колайдер всегда внутри (кол. персонажа)
    }

    IEnumerator ForAnimate(float time,  CharState state,Method metod)// для задержки анимации (время между кликами, через сколько после начала анимации ударить, анимация удара, метод удара)
    {
        isstay = false;//завершаем проигрывать анимацию простоя
        isrun = false;
        attack = true;//флаг начала атаки
        State = state;//начинаем проирывать текущую анимацию
                      //  Debug.Log("start "+Time.time);
        yield return new WaitForSeconds(time * 0.4F); 
        metod.Invoke();//вызываем метод атаки
        yield return new WaitForSeconds(time- time * 0.4F);//пережидаем все время анимации удара
       // Debug.Log("end " + Time.time);
        attack = false;//флаг завершения атаки
        isstay = true;//заного включаем анимацию простоя 
        isrun = true;
    }

    private void DoDamage()//урон в ближнем бою
    {
        //AttackWave.GetComponent<Animator>().x = napravlenie.x > 0.0F;
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
        Fire bullet = PoolManager.GetObject(FirePrefab.name, position, FirePrefab.transform.rotation).GetComponent<Fire>();
        bullet.Napravlenie = bullet.transform.right * (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F);//задаем направление и скорость пули (?если  true : false)
        bullet.Parent = gameObject;//родителем пули является текущий объект
        FireColb--;
    }

    private void OnTriggerEnter2D(Collider2D collision)//собирание огоньков
    {
        if (collision.GetComponent<FireSphere>())
        {
            collision.gameObject.GetComponent<PoolObject>().ReturnToPool();
            FireColb+=3;
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
            transform.position = DeleterSmoke.RespPos;
            lives = 100;
            FireColb = 0;
        }
    }

    public enum CharState
    {
        stay,
        walk,
        jump,
        attack,
        run_attack
    }

    #region Audio

    public AudioSource AuSourse;

    #region run sounds

    public void MakeRunSound()
    {
        if (checkaudio)//воспроизведение по очереди
        {
            StartCoroutine(AudioWalk(RunSound[i], times[j]));
            i++;
            j++;
            if (i == RunSound.Length) i = 0;
            if (j == times.Length) j = 0;
        }
    }

    IEnumerator AudioWalk(AudioClip sound, float time)// для задержки звука
    {
        checkaudio = false;//нельзя воспроизводить другую музыку
        yield return new WaitForSeconds(time);
        AuSourse.PlayOneShot(sound);
        checkaudio = true;
    }
    #endregion




    #endregion
}
