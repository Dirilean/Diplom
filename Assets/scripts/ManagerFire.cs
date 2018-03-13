using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFire : MonoBehaviour {


    [SerializeField]
    FireSphere FireSpherePrefab;
    [SerializeField]
    ForGen forgen;

    System.Random rnd = new System.Random();
    int RndCol;//количество огоньков на одной высоте
    int RndY;//рандомная высота
    float LastPos;
    float YPos;
    float XPos;
    int zaderzka=5;

    // Use this for initialization
    void Start()
    {
        LastPos = 20;
        RndCol = 3;
        YPos = 0.5F;
    }

    // Update is called once per frame
    void Update()
    {

        if (forgen.transform.position.x-zaderzka > LastPos+0.5)
        {
            if (RndCol <1)
            {
                RndY = rnd.Next(5);//5 вида высоты
                switch (RndY)//выбор высоты генерации
                {
                    case 0: YPos = 0F; break;
                    case 1: YPos = 2F; break;
                    case 2: YPos = 4F; break;
                    case 3: YPos = 6F; break;
                    case 4: YPos = -1F; break;//будет пусто
                }
                YPos =YPos+(float)(rnd.NextDouble()) / 2 + 0.25F;//от 0,25 до 0,75
                RndCol = rnd.Next(4)+2;//максимально на 1 высоте
            }
            RndCol--;
            //Debug.Log(RndCol);

            XPos = forgen.transform.position.x-zaderzka;
            //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos-0.5F), 0.2F);
            Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos), 0.2F);//чтобы на месте создания не было коллайдеров
            //Debug.Log("положение " + XPos + ", " + YPos + " col:" + colliders.Length+ " nocol:" + nocolliders.Length);

            if ((colliders.Length > 0) && (nocolliders.Length == 0)&&(YPos>0))//проверка на близость других коллайдеров
            {
                FireSphere FireSphere = Instantiate<FireSphere>(FireSpherePrefab, new Vector2(XPos, YPos), FireSpherePrefab.transform.rotation);
            }
            LastPos =forgen.transform.position.x-zaderzka;

        }
    }
}
