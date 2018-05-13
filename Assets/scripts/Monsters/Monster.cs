using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Monster : MonoBehaviour

{
    [SerializeField]
    public int Damage;//количество наносимого урона
    [SerializeField]
    public float TimeToDamage;//время за которое наносятся один удар (указывается в префабе)
    public int DefaultLives;//изначальные жизни
    [HideInInspector]
    public int lives;// текущие жизни
    public float DefaultSpeed;//изначальная скорость
    [HideInInspector]
    public float speed;//скорость передвижения
    [SerializeField]
    public int PlusFireColb;//сколько упадет огня с монстров
    [SerializeField]
    FireSphere FireSpherePrefab;
    public Vector2 point;//центр окружнгости для определения стен и игрока
    public Animator animator;
    [SerializeField]
    public Rigidbody2D rb;
    [SerializeField]
    ParticleSystem Smoke;
    public bool die;//запустили уже скрипт умирания? (используется для корутины)
    public bool isplayer;//мы столкнулись с игроком?
    Character TargetPlayer;
    float deltax;

    public float radius;//радиус удара
    public float Dalnost;//дальность(центр окружности) для проверки стен
    protected float LastTime;//Время последнего удара


    public Vector3 napravlenie;
    [SerializeField]
    public float SpriteSeeRight;//множитель направления спрайта

    protected float XPos;
    System.Random rnd = new System.Random();

    public CharState State//передаем состояние анимации в аниматор
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        TargetPlayer = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    private void OnEnable()
    {
        lives = DefaultLives;
        speed = DefaultSpeed;
        LastTime = 0;
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

    private void FixedUpdate()
    {
        //двигаемся
        Move();
    }

    private void Update()
    {
        //проверяем на живучесть
        if ((lives < 1)&&(die == false))
             { Die(); }
        //вызываем ближний бой
        
        if (isplayer)//если столкнулись с игроком спереди то наносим урон
        {
            
            State = CharState.attack;
            if (Time.time >= TimeToDamage + LastTime)//Удар в ближнем бою - задержка(из-за анимации до самого удара)
            {
                TargetPlayer.lives -= Damage;
                LastTime = Time.time;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TargetPlayer = collision.gameObject.GetComponent<Character>();
        if (TargetPlayer != null) { deltax = ((TargetPlayer.transform.position.x - gameObject.transform.position.x) * napravlenie.x); }
        if ((collision.gameObject.name == "Player")&&(deltax>0))//если игрок находится спереди моба и при этом коснулся триггера
        {
            isplayer = true;
            LastTime = Time.time;  
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.name == "Player")
        {
            isplayer = false;
        }
    }
    public virtual void Move()
    {
        point = new Vector2(transform.position.x + (Dalnost * napravlenie.x), transform.position.y + 0.5F);//текущая позция проверки стен
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 0.02F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(point.x, point.y - 0.3F), 0.2F);
        //условие поворота
        if ((colliders.Length > 0 && colliders.Any(x => x.CompareTag("Platform"))) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области стен
        if (!isplayer)//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x * SpriteSeeRight < 0.0F;//поворот}
            State = CharState.walk;
        }
    }

    public enum CharState
    {
        walk,
        attack,
        jump
    }
}
