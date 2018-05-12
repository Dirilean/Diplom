using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireSphere : MonoBehaviour {




    //transform.Translate(new Vector2(1,0)*Time.deltaTime);
    public Vector2 target;//точка куда сейчас движется огонек (верх-низ)
    float smoothTime;
    float smoothTime1;
    private float Velocity;
    bool once;
    public Vector3 Niz;//самая нижняя точка
    public Vector3 Verh;//самая верхняя точка 
    public Vector3 Now;
    System.Random rnd = new System.Random();
    GameObject Player;
    public float TimeToPlayer;
    public bool CheckPlayer;

    private void Start()
    {
        Player = GameObject.FindWithTag("Player");
        smoothTime = rnd.Next(3) + 1;
        smoothTime1 = smoothTime;
        target = new Vector3(0, 1);
        TimeToPlayer = 8f;
        once = false;
        CheckPlayer = false;//изначально к игроку не летим
        Velocity = 0.0F;
        
    }

    private void OnEnable()
    {
        Verh = new Vector3(0, 0.15F) + transform.position;
        Niz = -new Vector3(0, 0.1F) + transform.position;
        CheckPlayer = false;//изначально к игроку не летим
        Now = Verh;
        //Debug.Log("OnEnable "+CheckPlayer);
    }

    private void OnDisable()
    {
        Velocity = 0.0F;
        transform.position = Vector3.zero;
        CheckPlayer = false;
       // Debug.Log("OnDisnable " + CheckPlayer);
    }

    void Update()
    {
        //игрок близко? начинать движение к игроку?
        if ((Mathf.Abs(Player.transform.position.x - transform.position.x) < 1.5F) && (Mathf.Abs(Player.transform.position.y+0.5F - transform.position.y) < 1.5F))//если игрок ближе чем 1.5F
        {
            CheckPlayer = true;
        }

        if(CheckPlayer)//если видит игрока
        {
            transform.position = Vector3.Lerp(transform.position, Player.transform.position + 0.5F * Vector3.up, TimeToPlayer * Time.deltaTime);
        }
        else//если не видит игрока
        {
            Pokoy();
        }
    }


    //висение в невесомости, бездействие
    private void Pokoy()
    {
        float newPosition = Mathf.SmoothDamp(transform.position.y, Now.y, ref Velocity, smoothTime1);
        transform.position = new Vector3(transform.position.x, newPosition, transform.position.z);
        if (((Mathf.Abs(Verh.y - transform.position.y) < 0.02) || (Mathf.Abs(Niz.y - transform.position.y) < 0.02)) && (once == true))//если близко к верху, поменять таргет
        {
            once = false;
            smoothTime1 = smoothTime;
            Velocity = 0.0F;
            target *= -1;
            Now = (Now == Verh) ? Niz : Verh;
        }
        if (((Mathf.Abs(Verh.y - transform.position.y) > 0.02) || (Mathf.Abs(Niz.y - transform.position.y) > 0.02)) && (once == false))
        {
            once = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
     if (collision== Player)
        {
            CheckPlayer = false;
        }
    }
}



