using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerFire : MonoBehaviour {


    [SerializeField]
    FireSphere FireSpherePrefab;
    ForGen forgen;

    System.Random rnd = new System.Random();
    int RndCol;//количество огоньков на одной высоте
    int RndY;//случайная высота
    float LastPos;//последняя позиция генерации
    //текущая позиция генерации по оси х и по оси у
    float YPos;
    float XPos;
    //текущий уровень локации
    [SerializeField]
    int lvl;
    [SerializeField]
    float minY;//самая низкая допустимая высота для этой локации
    [SerializeField]
    float maxY;//самая высокая точка генерации для этой локации
    int zaderzka=6;//расстояние, на котором генерируются огоньки, относительно генератора

    void Start()
    {       
        LastPos = 20;
        RndCol = 3;
        YPos = 0.5F;
    }

    void Update()
    {
        if (forgen == null)
        {
            if (GameObject.Find("Player"))
            {
                forgen = GameObject.Find("Player").GetComponent<Character>().transform.Find("Generator").GetComponent<ForGen>();
            }
            else if (GameObject.Find("Player2(Clone)"))
            {
                forgen = GameObject.Find("Player2(Clone)").GetComponent<Character>().transform.Find("Generator").GetComponent<ForGen>();
            }
            else if (GameObject.Find("Player3(Clone)"))
            {
                forgen = GameObject.Find("Player3(Clone)").GetComponent<Character>().transform.Find("Generator").GetComponent<ForGen>();
            }
            else Debug.Log("error!");
        }

        switch (lvl)
        {
            case 1:
                {
                    if (forgen.transform.position.x - zaderzka > LastPos + 0.5)
                    {
                        if (RndCol < 1)
                        {
                            RndY = rnd.Next(7);//7 вида высоты
                            switch (RndY)//выбор высоты генерации
                            {
                                case 0: YPos = 0F; break;
                                case 1: YPos = 2F; break;
                                case 2: YPos = 4F; break;
                                case 3: YPos = 6F; break;
                                case 4: YPos = -1F; break;//будет пусто
                                case 5: YPos = -1F; break;//будет пусто
                                case 6: YPos = -1F; break;//будет пусто
                            }
                            YPos = YPos + (float)(rnd.NextDouble()) / 2 + 0.25F;//от 0,25 до 0,75
                            RndCol = rnd.Next(4) + 2;//максимально на 1 высоте
                        }
                        RndCol--;

                        XPos = forgen.transform.position.x - zaderzka;
                        //круг вокруг нижней линии персонажа. если в него попадают колайдеры то массив заполняется ими
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos - 0.5F), 0.2F);
                        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos), 0.2F);//чтобы на месте создания не было коллайдеров

                        if ((colliders.Length > 0) && (nocolliders.Length == 0) && (YPos > minY) && (YPos < maxY))//проверка на близость других коллайдеров и существование высоты
                        {
                            FireSphere firesphere = PoolManager.GetObject(FireSpherePrefab.name, new Vector2(XPos, YPos), FireSpherePrefab.transform.rotation).GetComponent<FireSphere>();
                        }
                        LastPos = forgen.transform.position.x - zaderzka;
                    }
                    break;
                }
            case 2:
                {
                    if (forgen.transform.position.x - zaderzka > LastPos + 0.5)
                    {
                        if (RndCol < 1)
                        {
                            RndY = rnd.Next(7);//7 вида высоты
                            switch (RndY)//выбор высоты генерации
                            {
                                case 0: YPos = 12F; break;
                                case 1: YPos = 14F; break;
                                case 2: YPos = 16F; break;
                                case 3: YPos = 18F; break;
                                case 4: YPos = -1F; break;//будет пусто
                                case 5: YPos = -1F; break;//будет пусто
                                case 6: YPos = -1F; break;//будет пусто
                            }
                            YPos = YPos + (float)(rnd.NextDouble()) / 2 + 0.25F;//от 0,25 до 0,75
                            RndCol = rnd.Next(4) + 2;//максимально на 1 высоте
                        }
                        RndCol--;
                        XPos = forgen.transform.position.x - zaderzka;

                        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos), 0.4F);

                        if ((nocolliders.Length == 0) && (YPos > minY) && (YPos < maxY))//проверка на близость других коллайдеров и существование высоты
                        {
                            FireSphere firesphere = PoolManager.GetObject(FireSpherePrefab.name, new Vector2(XPos, YPos), FireSpherePrefab.transform.rotation).GetComponent<FireSphere>();
                        }
                        LastPos = forgen.transform.position.x - zaderzka;
                    }
                    break;
                }
            case 3:
                {
                    if (forgen.transform.position.x - zaderzka > LastPos + 0.5)
                    {
                        if (RndCol < 1)
                        {
                            RndY = rnd.Next(7);//7 вида высоты
                            switch (RndY)//выбор высоты генерации
                            {
                                case 0: YPos = 22F; break;
                                case 1: YPos = 24F; break;
                                case 2: YPos = 26F; break;
                                case 3: YPos = 28F; break;
                                case 4: YPos = -1F; break;//будет пусто
                                case 5: YPos = -1F; break;//будет пусто
                                case 6: YPos = -1F; break;//будет пусто
                            }
                            YPos = YPos + (float)(rnd.NextDouble()) / 2 + 0.25F;//от 0,25 до 0,75
                            RndCol = rnd.Next(4) + 2;//максимально на 1 высоте
                        }
                        RndCol--;
                        XPos = forgen.transform.position.x - zaderzka;

                        Collider2D[] nocolliders = Physics2D.OverlapCircleAll(new Vector2(XPos, YPos), 0.4F);

                        if ((nocolliders.Length == 0) && (YPos > minY) && (YPos < maxY))//проверка на близость других коллайдеров и существование высоты
                        {
                            FireSphere firesphere = PoolManager.GetObject(FireSpherePrefab.name, new Vector2(XPos, YPos), FireSpherePrefab.transform.rotation).GetComponent<FireSphere>();
                        }
                        LastPos = forgen.transform.position.x - zaderzka;
                    }
                    break;
                }
        }
    }
}
