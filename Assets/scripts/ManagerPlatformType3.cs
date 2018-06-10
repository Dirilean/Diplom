using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPlatformType3 : MonoBehaviour
{

    public GameObject[] space = new GameObject[13]; //все платформы
    public GameObject gr;//границы уровня
    Vector3 GenPos;//текущая позиция генерации
    Quaternion GenQ = new Quaternion(0, 0, 0, 0);//текущий разворот
    [SerializeField]
    ForGen forgen;//ссылка на генератор
    bool EndStartPack = false; //проверка на то, нужно ли начинать генерацию

    float y1 = 13F;//высота платформ 1
    float y2 = 17F;//высота платформ 2
    float x1;//расстояние между платформами на высоте 1
    float x2;//расстояние между платформами на высоте 2
    float x1last;//расстояние между платформами предыдущее для высоты 1
    float x2last;//расстояние между платформами предыдущее для высоты 2

    public float maxstep = 10;//максимальный шаг между платформами
    int i; //счетчик
    float lastGroundPos;//последняя позиция генерации земли
    public float GroundLenght = 4F; //длина объекта земля

    void Start()
    {
        GameObject player = GameObject.Find("Player3(Clone)");
        forgen = player.transform.Find("Generator").GetComponent<ForGen>();
    }

    void Update()
    {
        //начинать ли генерацию платформ?
        if (EndStartPack == false)
        {//код когда генерация еще не началась
            Collider2D[] colliders = Physics2D.OverlapCircleAll(new Vector3(forgen.transform.position.x - GroundLenght, 19.5F), 0.1F, 1 << 12);
            if (colliders.Length < 1)
            {
                EndStartPack = true;
                GenPos = new Vector3((forgen.transform.position.x + Static.StepPlatf), 0);
                lastGroundPos = forgen.transform.position.x;
            }
        }
        else
        {//код генерации
            GenPos.x = forgen.transform.position.x + Static.StepPlatf;

            if (lastGroundPos + GroundLenght < GenPos.x)
            {
                //генерирование границ
                GameObject Gran = PoolManager.GetObject(gr.name, new Vector3(lastGroundPos, 19), GenQ);
                //генерирование платформ
                GenPlat(ref x1, ref y1, ref x1last);
                GenPlat(ref x2, ref y2, ref x2last);
                lastGroundPos += GroundLenght;
            }
        }
    }

    void GenPlat(ref float x, ref float y, ref float xlast)//метод, генерирующий платформу
    {
        if (GenPos.x >= xlast + x)
        {
            xlast = GenPos.x;
            i = Random.Range(0, space.Length);//выбираем любую платформу
            GameObject Platform = PoolManager.GetObject(space[i].name, new Vector3(GenPos.x, Random.Range(-1, 2) + y), new Quaternion(0,0,0, Random.Range(0, 1F)));//создаем ее
            x = Random.Range(2, maxstep);//задаем через сколько нужно будет сделать следующую платформу на этом уровне высоты
        }
    }
}