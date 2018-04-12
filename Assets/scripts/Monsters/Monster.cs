using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;


public class Monster : Unit

{
    [SerializeField]
    public int Damage;//количество наносимого урона
    [SerializeField]
    public float TimeToDamage;//время за которое наносятся один удар (указывается в префабе)
    [SerializeField]
    public int lives;//жизни
    [SerializeField]
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
    ParticleSystem smoke;
    public bool die;//запустили уже скрипт умирания?

    public float radius;//радиус удара
    public int layerMask;//слой "жертвы" (игрок)
    public float Dalnost;//дальность удара (центр окружности)
    protected float LastTime;//Время последнего удара


    public Vector3 napravlenie;
    [SerializeField]
    public float SpriteSeeRight;//множитель направления спрайта

    protected float XPos;
    protected float YPos;
    System.Random rnd = new System.Random();

    public CharState State//передаем состояние анимации в аниматор
    {
        get { return (CharState)animator.GetInteger("State"); }
        set { animator.SetInteger("State", (int)value); }
    }

    private void Start()
    {
        layerMask = 10;
        LastTime = 0;
        napravlenie = Vector3.right;//начальное направление вправо
        die = false;
    }

    IEnumerator Example()
    {
        yield return new WaitForSeconds(0.3F);
        Destroy(gameObject);
    }

    public void Die()//смерть персонажа
    {
        die = true;
            XPos = gameObject.transform.position.x;
            smoke = Instantiate(Smoke, new Vector3(XPos, transform.position.y + 0.5F), gameObject.transform.rotation);//создание дымки после смерти
            int k = 0;
            while (k < PlusFireColb)//генерирование огоньков в зависимости от указанаого в префабе значения
            {
                YPos = (float)(rnd.NextDouble()) / 3 + 0.3F;//от 0,3 до 0,6 для начальной разной высоты
                FireSphere FireSphere = Instantiate(FireSpherePrefab, new Vector2(XPos, gameObject.transform.position.y + YPos), FireSpherePrefab.transform.rotation);
                XPos += 0.5F;
                k++;
            }
            speed = 0;
        
        StartCoroutine(Example());
    }

    // функция возвращает ближайший объект из массива, относительно указанной позиции
    //(вспомогательный для ближнего боя)
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

    //ближний бой, центр окр, радиус окр, слой игрока, на сколько ударяем
    public static void DoDamage(Vector2 point, float radius, int layerMask, int damage)
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);
        GameObject obj = NearTarget(point, colliders);
        if (obj.GetComponent<Character>())
        {   
            obj.GetComponent<Character>().lives -= damage;
        }
        return;
    }

    private void FixedUpdate()
    {
        //двигаемся
        Move();
    }

    private void Update()
    {
        //проверяем на живучесть
        if ((lives <= 0)&&(die == false))
             { Die(); }
        //вызываем ближний бой
        point = new Vector2(transform.position.x + (Dalnost * napravlenie.x), transform.position.y + 0.5F);//текущая позция удара
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, radius, 1 << layerMask);//для урона по игроку
        if (colliders.Length > 0)
        {
            State = CharState.attack;//анимация удара
            if (Time.time >= TimeToDamage + LastTime - 0.07)//Удар в ближнем бою - задержка(из-за анимации до самого удара)
            {
                DoDamage(point, radius, layerMask, Damage); //точка удара, радиус поражения, слой врага, урон
                LastTime = Time.time;
            }
        }
    }

    public virtual void Move()
    {
        //стенки
        Collider2D[] colliders = Physics2D.OverlapCircleAll(point, 0.02F);
        //пустота
        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(point.x, point.y - 0.3F), 0.2F);
        //условие поворота
        //if ((colliders.Length > 0 && colliders.All(x => !x.GetComponent<Character>())) || (nocolliders.Length < 1)) napravlenie *= -1.0F;
        if ((colliders.Length > 0 && colliders.Any(x => x.CompareTag("Platform"))) || (nocolliders.Length < 1)) napravlenie *= -1.0F;//перевернуть при условии появления в области каких либо коллайдеров или пропасти, игнорирование персонажа, и огоньков
        if (!(colliders.Length > 0 && colliders.Any(x => x.GetComponent<Character>())))//идет если не врежется в персонажа
        {
            rb.velocity = new Vector2(speed * napravlenie.x, rb.velocity.y);
            GetComponent<SpriteRenderer>().flipX = napravlenie.x * SpriteSeeRight < 0.0F;//поворот}
            State = CharState.walk;
        }
    }

    public enum CharState
    {
        walk,
        attack
    }
}
