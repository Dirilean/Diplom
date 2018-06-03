using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Monster : MonoBehaviour

{
    [SerializeField]
    public int Damage;//количество наносимого урона
    public int DefaultLives;//изначальные жизни
  //  [HideInInspector]
    public int lives;// текущие жизни
    public float DefaultSpeed;//изначальная скорость
    [HideInInspector]
    public float speed;//скорость передвижения
    [SerializeField]
    public int PlusFireColb;//сколько упадет огня с монстров
    [SerializeField]
    FireSphere FireSpherePrefab;
    [HideInInspector]
    public Vector2 point;//центр окружнгости для определения стен и игрока
    protected Animator animator;
    [HideInInspector]
    public Rigidbody2D rb;
    [SerializeField]
    ParticleSystem Smoke;
    [HideInInspector]
    public bool die;//запустили уже скрипт умирания? (используется для корутины)
   // [HideInInspector]
    public bool playerNear;//мы столкнулись с игроком?
    [HideInInspector]
    public Character Player;
    public float deltax;//записывается текущее расстояние до персонажа
    public float deltay;
    public float Dalnost;//дальность(центр окружности) для проверки стен
    [SerializeField]
    float deltaColor;//для плавного изменения цвета игрока

    public Vector3 napravlenie;
    [SerializeField]
    public float SpriteSeeRight;//множитель направления спрайта

    protected float XPos;
    System.Random rnd = new System.Random();

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    public CharState State//передаем состояние анимации в аниматор
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().color=new Color(255,255,255,1);
        lives = DefaultLives;
        speed = DefaultSpeed;
        napravlenie = Vector3.right;//начальное направление вправо
        die = false;
    }

    IEnumerator ForDie()
    {
        yield return new WaitForSeconds(0.3F);
        GetComponent<PoolObject>().ReturnToPool();//"удаление объекта"
    }

    public void Die()//смерть персонажа
    {
        die = true;
        XPos = gameObject.transform.position.x;
        //smoke = Instantiate(Smoke, new Vector3(XPos, transform.position.y + 0.5F), gameObject.transform.rotation);//создание дымки после смерти
        GameObject smoke = PoolManager.GetObject(Smoke.name, new Vector3(XPos, transform.position.y + 0.5F), transform.rotation);
        int k = 0;

        while (k < PlusFireColb)//генерирование огоньков в зависимости от указанаого в префабе значения
        {    
            FireSphere firesphere = PoolManager.GetObject(FireSpherePrefab.name, new Vector2(XPos, transform.position.y + 0.5F), FireSpherePrefab.transform.rotation).GetComponent<FireSphere>();
            firesphere.CheckPlayer = true;//чтобы как только огоньки упадут с моба, сразу летели к игроку
            XPos += 0.2F;
            k++;
        }
        speed = 0;
        StartCoroutine(ForDie());
    }

    private void Update()
    {
        Move();

        //проверяем на живучесть
        if ((lives < 1) && (die == false))
        {
            Die();
        }

        if (playerNear)//если столкнулись с игроком спереди то наносим урон
        {
            State = CharState.attack;//запускаем анимацию удара (она же и вызовет метод самого удара)
        }

        //получение урона
            deltaColor = Mathf.Lerp(deltaColor, ((float)lives / DefaultLives) + 0.2F, Time.deltaTime * 10);
            GetComponent<SpriteRenderer>().color = new Color(255, 255, 255, deltaColor);
        
    }

    public void Attack()//метод атаки
    {
        if (Player!=null) Player.lives -= Damage;
    }

    #region CheckPlayer
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            Player = collision.gameObject.GetComponent<Character>();
            deltax = ((Player.transform.position.x - gameObject.transform.position.x) * napravlenie.x);
            deltay = Player.transform.position.y - transform.position.y;
            if ((deltax > 0) && (deltax <1F) && (deltay >= -1) && (deltay <= 1F))//если игрок находится спереди моба и при этом коснулся триггера, на расстоянии ближе чем 1,5по х, и ниже чем 1F относительно моба
            {
                playerNear = true;
            }
            else
            {
                playerNear = false;
            }
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Character>())
        {
            playerNear = false;
            deltax = 0;
            deltay = 0;
            Player = null;
        }
    }
    #endregion

    #region Move
    public virtual void Move()
    {
        point = new Vector2(transform.position.x + (Dalnost * napravlenie.x), transform.position.y + 0.5F);//текущая позция проверки стен
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 0.02F, 1 << 13);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(point.x, point.y - 0.3F), 0.2F, 1 << 13);
        //условие поворота
        if ((colliders.Length > 0) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области стен
        if (!playerNear)//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x * SpriteSeeRight < 0.0F;//поворот}
            State = CharState.walk;
        }
    }
    #endregion

    public enum CharState
    {
        walk,
        attack,
        jump
    }
}
