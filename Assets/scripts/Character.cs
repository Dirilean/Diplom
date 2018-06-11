using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
    #region Players Stats
    [Header("Stats")]
    public int FireColb;//количество огня в колбе
    public int lives=100;//количество жизней
    public float speed;
    private float TempSpeed;//временная скорость (изменяется)

    int MinFireColb = 0;//минимальное хп, при котором прекращаются выстрелы
    [Tooltip("Время перезарядки конвертации жизни")]
    public float TimeToPlusLives;
    [Tooltip("Сколько секунд после смерти нужно ждать чтобы воскреснуть")]
    float zaderzhka = 1;
    public int FlyResourse=-1;//ресурс полета
    public int BoomResourse = -1;//ресурс взрыыва
    public float repeat_time; /* Время в секундах для полетов */
    private float curr_time;
    #endregion

    #region For checked time
    [HideInInspector]
    public float LastTimeToPlusLives;//последнее время конвертьации жизней
    float timeDie;//время смерти
    Vector3 napravlenie;//куда смотрит игрок
    float deltaColor;//для плавного изменения цвета игрока
    public int AttackType = 0;//1-near,2-shoot
    float OfssetY;//расстояниие появления атаки по у относительно перса
    float LastBoomTime=0;
    #endregion

    #region System
    Rigidbody2D rb;
    Animator animator;

    public enum AnimState
    {
        stay,
        run,
        jump
    }
    public AnimState State//передаем состояние анимации в аниматор
    {
        get { return (AnimState)animator.GetInteger("state"); }
        set { animator.SetInteger("state", (int)value); }
    }
    #endregion

    #region Eny GameObjects
    [Header("Eny GameObjects")]
    public Fire FirePrefab;
    public Fire AttackWavePrefab;
    public GameObject BoomPrefab;
    public Deleter DeleterSmoke;
    Fire prefab; //префаб текущей атаки

    #endregion

    #region Flags
    [Header("Flags")]
    [HideInInspector]
    public bool isGrounded = false; //проверка, стоит ли на земле
    bool lastIsGrounded = true;//для проверки приземления(звук)
    bool landing = false;//для порверки приземдения (звук)
    [Tooltip ("мы находимся в состоянии прыжка?")]
    bool CheckJump;
    #endregion

    #region Audio
    [Header("Audio")]
    public AudioSource AuSourse;
    public AudioClip[] RunSound = new AudioClip[7];//звуки шагов
    int i;
    public AudioClip JumpSound;
    #endregion

    #region Level settings
    public Character NextPlayerPrefab;
    public int PrefabLevel;//уровень персонажа для этого префаба
    #endregion

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CheckJump = false;
        LastTimeToPlusLives = 0;
        napravlenie = Vector3.right;
        //счетчики для звуков шагов
        i = 0;
        AuSourse.volume = 0.03F;
        LastTimeToPlusLives = -5;
        curr_time = repeat_time;
        // if (PrefabLevel == 2) Destroy(gameObject.transform.Find("Player"));
        // Destroy(LastPlayer);
        DeleterSmoke = GameObject.Find("SmokeDelete").GetComponent<Deleter>();
    }

    private void FixedUpdate()
    {
        CheckGround();
        if (lives <= 0) { Die(); }
        if (PrefabLevel != 3)
        {
            rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
        }
        //else
        //{
        //     rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, Input.GetAxis("Vertical") * speed);
        //}
        #region SpaceFly
        if (PrefabLevel == 3)
        {
            if ((Input.GetAxis("Vertical") != 0) || (Input.GetAxis("Horizontal") != 0))
            {
                //rb.velocity = (transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * speed;
                rb.AddForce((transform.up * Input.GetAxis("Vertical") + transform.right * Input.GetAxis("Horizontal")) * 15F, ForceMode2D.Force);
            }
            if (Input.GetAxis("Horizontal") != 0)
            { GetComponent<SpriteRenderer>().flipX = Input.GetAxis("Horizontal") > 0.0F; }
        }
        #endregion
    }

    private void Update()
    {

        #region input left - right
        if (PrefabLevel != 3)
        {
            if (Input.GetAxis("Horizontal") != 0)//обычное хождение
            {
                napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
                GetComponent<SpriteRenderer>().flipX = napravlenie.x > 0.0F;
                State = AnimState.run;
            }
            else if (isGrounded)
            {
                State = AnimState.stay;
            }
            if (!isGrounded)
            {
                State = AnimState.jump;
            }
        }

        #endregion

        #region input Jump
        if (PrefabLevel == 1)
        {
            if (isGrounded && Input.GetButton("Jump") && (CheckJump == false))//прыжок 
            {
                if (transform.position.y < 5F)//если не на самой верхней платформе
                    rb.AddForce(new Vector3(10F * Input.GetAxis("Horizontal"), 70F), ForceMode2D.Impulse);
                else
                    rb.AddForce(new Vector3(10F * Input.GetAxis("Horizontal"), 65F), ForceMode2D.Impulse);
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
        }
        #endregion

        #region Fly
        if (PrefabLevel == 2)
        {
            curr_time -= Time.deltaTime; /* Вычитаем из 10 время кадра (оно в миллисекундах) */
            if (curr_time <= 0) /* Время вышло пишем */
            {
                curr_time = repeat_time; /* запускает опять таймер,чтобы повторялось бесконечно */
                if (isGrounded && FlyResourse <= 98)
                {
                    FlyResourse += 5;
                }
                else if (FlyResourse > 0 && !isGrounded && Input.GetButton("Jump"))//вычитает только при нажатой клавише
                {
                    FlyResourse -= 1;
                }
            }
            if (Input.GetButton("Jump") && (FlyResourse > 0))
            {
                rb.AddForce(new Vector3(0, 3F * Input.GetAxis("Vertical")), ForceMode2D.Impulse);
            }
            //    if (Input.GetButton("Jump") && (FlyResourse > 0))
            //{
            //    //rb.AddForce(new Vector3(0,10F * Input.GetAxis("Vertical")), ForceMode2D.Impulse);
            //     rb.gravityScale = -0.2F* Input.GetAxis("Vertical");
            //}
            //else
            //{
            //    rb.gravityScale = 0.3F;
            //}
        }
        #endregion


        #region  Attack
        if (Input.GetButtonDown("Fire2")&&FireColb>0)//shooting
        {
            AttackType = 2;
            animator.SetBool("attack", true);
        }
        else if (Input.GetButtonDown("Fire1"))//near attack
        {
            AttackType = 1;
            animator.SetBool("attack", true);
        }
        else
        {
            animator.SetBool("attack", false);
        }
        #endregion

        #region boom
        if (PrefabLevel==3 && BoomResourse < 100 && (Time.time + 20F > LastBoomTime) )
        {
            BoomResourse++;
            LastBoomTime = Time.time;
        }

        if (PrefabLevel==3 && Input.GetButtonDown("Fire3") && FireColb >= 2 && BoomResourse>98)//shooting
        {
            animator.SetBool("attackBoom", true);
        }
        else
        {
            animator.SetBool("attackBoom", false);
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
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 0.15F,1<<13);
        isGrounded = colliders.Length > 0; //один колайдер всегда внутри (кол. персонажа)
    }

    private void Attack()//вызывается из аниматора
    {
        if (AttackType == 1) { prefab = AttackWavePrefab; OfssetY = 0.6F; }
        else if(AttackType==2){ prefab = FirePrefab; OfssetY = 0.7F; }
        Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? 0.8F : -0.8F), transform.position.y+OfssetY);//место создания пули относительно персонажа
        Fire fire = PoolManager.GetObject(prefab.name,position, prefab.transform.rotation).GetComponent<Fire>();
        fire.napravlenie = fire.transform.right * (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F);//задаем направление и скорость пули (?если  true : false)
        fire.CurrentSpeed+=Mathf.Abs(rb.velocity.x);
        fire.GetComponent<SpriteRenderer>().flipX = GetComponent<SpriteRenderer>().flipX;
        FireColb -= fire.minusFire;
    }


    IEnumerator BoomAttack()//вызывается из аниматора
    {
        Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? 0.8F : -0.8F), transform.position.y + OfssetY);//место создания пули относительно персонажа
        BoomPrefab.SetActive(true);
        FireColb -= 2;
        BoomResourse = 0;
        yield return new WaitForSeconds(1F);
        BoomPrefab.SetActive(false);
    }

    #region TakeFire
    private void OnTriggerEnter2D(Collider2D collision)//собирание огоньков
    {
        if (collision.GetComponent<FireSphere>())
        {
            collision.gameObject.GetComponent<PoolObject>().ReturnToPool();
            FireColb++;
        }
    }
    #endregion

    #region ConvertLives
    private void ConvertToLives()//конвертирование огня в жизнь
    {
        if (Time.time>LastTimeToPlusLives+TimeToPlusLives)//в колбе достаточно огня и прошло время перезарядки
        {
            if (FireColb >= (100 - lives))
            {
                FireColb -= 100 - lives;
                lives = 100;
            }
            else
            {
                lives += FireColb;
                FireColb = 0;
            }
            LastTimeToPlusLives = Time.time;
        }
    }
    #endregion

    #region Die
    void Die()//смерть персонажа
    {
        if (FireColb > 0)//возрадится можно только при имеющихся огоньках
        {
            gameObject.GetComponent<SpriteRenderer>().color = Color.black;
            lives = 0;
            timeDie = Time.time;
            if (timeDie + zaderzhka > Time.time)
            {
                Collider2D[] colliders = Physics2D.OverlapCircleAll(DeleterSmoke.RespPos, 1F, 1 << 13);
                //удаляем платформы что попали в зону респауна
                if (colliders.Length > 0)
                {
                    for (int i = 0; i < colliders.Length; i++)
                    {
                        colliders[i].GetComponent<PoolObject>().ReturnToPool();
                    }
                }
                Debug.Log(DeleterSmoke.RespPos);
                transform.position = DeleterSmoke.RespPos;
                Debug.Log(transform.position);
                if (FireColb >= 100)
                {
                    lives = 100;
                }
                else
                {
                    lives += FireColb;
                }
                FireColb = 0;
            }
        }
        else
        {
            lives = -100;
        }
    }
    #endregion

    #region Audio

    private void PlayRunSound()//вызывается из аниматора
    {
        i = Random.Range(0,6);
        AuSourse.PlayOneShot(RunSound[i]);
    }


    #endregion
}
