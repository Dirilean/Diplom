using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//Debug.Log("прыг") обращение к консоли

public class Character : MonoBehaviour
{
    #region Players Stats
    [Header("Stats")]
    public int FireColb;//количество огня в колбе
    public int lives;//количество жизней
    public float speed;
    private float TempSpeed;//временная скорость (изменяется)

    int MinFireColb = 0;//минимальное хп, при котором прекращаются выстрелы
    [Tooltip("Время перезарядки конвертации жизни")]
    public float TimeToPlusLives;
    [Tooltip("Сколько секунд после смерти нужно ждать чтобы воскреснуть")]
    float zaderzhka = 1;
    #endregion

    #region For checked time
    [HideInInspector]
    public float LastTimeToPlusLives;//последнее время конвертьации жизней
    float timeDie;//время смерти
    float TimeShoot;//время выстрела
    float delayShooy = 0.588F;//задержка при выстрелах
    float TimeDoDamage;//время удара в ближнем бою
    Vector3 napravlenie;//куда смотрит игрок
    float deltaColor;//для плавного изменения цвета игрока
    #endregion

    #region System
    Rigidbody2D rb;
    Animator animator;

    public CharState state//передаем состояние анимации в аниматор
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }
    #endregion

    #region Eny GameObjects
    [Header("Eny GameObjects")]
    public Fire FirePrefab;
    public Fire AttackWavePrefab;
    public Deleter DeleterSmoke;
    Fire prefab; //префаб текущей атаки
    #endregion

    #region Flags
    [Header("Flags")]
    [Tooltip("проигрываем анимацию простоя?")]
    bool isstay;
    [Tooltip("проигрываем анимацию бега?")]
    bool isrun;
    [HideInInspector]
    public bool isGrounded = false; //проверка, стоит ли на земле
    bool lastIsGrounded = true;//для проверки приземления(звук)
    bool landing = false;//для порверки приземдения (звук)
    [Tooltip ("мы находимся в состоянии прыжка?")]
    bool CheckJump;
    [Tooltip("атакуем в данный момент?")]
    bool attack;
    #endregion

    #region Audio
    [Header("Audio")]
    public AudioClip[] RunSound = new AudioClip[7];//звуки шагов
    float[] times = new float[4] { 0.12F, 0.03F, 0.36F, 0.03F };//тайминг шагов
    int i, j;
    bool checkaudio;
    public AudioClip JumpSound;
    #endregion
   

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        CheckJump = false;
        lives = 100;
        speed = 3.5F;
        LastTimeToPlusLives = 0;
        TimeShoot = Time.time;
        FireColb = 50;
        napravlenie = Vector3.right;
        isstay = false;
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
        rb.velocity = new Vector2(Input.GetAxis("Horizontal") * speed, rb.velocity.y);
    }

    private void Update()
    {
        if (lives <= 0) { Die(); }

        #region input left - right
        
        if (Input.GetAxis("Horizontal") != 0)//обычное хождение
        {
            napravlenie = transform.right * Input.GetAxis("Horizontal"); //(возвращает 1\-1) Unity-> edit-> project settings -> Input 
            GetComponent<SpriteRenderer>().flipX = napravlenie.x > 0.0F;
            if ((CheckJump == false))
            {
                animator.SetInteger("state", 1);
            }
            else
            {
                animator.SetInteger("state", 2);
            }
            if (isGrounded) MakeRunSound();
        }
        else if (isGrounded)//анимация простоя
        {
            animator.SetInteger("state", 0);
        }
        #endregion

        #region input Jump
        if (isGrounded && Input.GetButton("Jump") && (CheckJump == false))//прыжок 
        {
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
        //if ((Input.GetButtonDown("Fire2")) && (attack == false))
        //{
        //    if (Input.GetAxis("Horizontal") != 0)//если нажали атаку во время бега
        //    {
        //        StartCoroutine(ForShoot(delayShooy,  CharState.run_attack, FirePrefab));    
        //    }
        //    else
        //    {
        //        StartCoroutine(ForShoot(delayShooy, CharState.attack, FirePrefab));
        //    }
        //}
        #endregion

        #region input Attack
        if((Input.GetButtonDown("Fire1")) && (attack == false))
        {
            animator.SetBool("attack", true);
            animator.SetInteger("attack_type",1);
        }
        else
        {
            animator.SetBool("attack", false);
            animator.SetInteger("attack_type", 0);
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

    private void Attack()//вызывается из аниматора
    {
        
        prefab = AttackWavePrefab;
        Vector3 position = new Vector3(transform.position.x + (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F), transform.position.y+0.65F);//место создания пули относительно персонажа
        Fire fire = PoolManager.GetObject(prefab.name,position, prefab.transform.rotation).GetComponent<Fire>();
        fire.napravlenie = fire.transform.right * (GetComponent<SpriteRenderer>().flipX ? 0.5F : -0.5F);//задаем направление и скорость пули (?если  true : false)
        fire.CurrentSpeed+=Mathf.Abs(rb.velocity.x);
        FireColb -= fire.minusFire;
    }

    private void OnTriggerEnter2D(Collider2D collision)//собирание огоньков
    {
        if (collision.GetComponent<FireSphere>())
        {
            collision.gameObject.GetComponent<PoolObject>().ReturnToPool();
            FireColb++;
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
        run,
        jump
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
