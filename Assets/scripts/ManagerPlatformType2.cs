using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPlatformType2 : MonoBehaviour {

    public GameObject[] sky = new GameObject[12]; //все платформы
    public float[] lenghts = new float[12]; //длины платформ
    public GameObject gr;//смертельная область
    Vector3 GenPos;//текущая позиция генерации
    Quaternion GenQ = new Quaternion(0, 0, 0, 0);//текущий разворот
    [SerializeField]
    ForGen forgen;
    float LastFonPos;//координата последнего фонового рисунка
    //[SerializeField]
    //GameObject Fon;

    //________________
    float y1;
    float y2;
    float y3;
    float y4;
    public float maxstep;//максимальный шаг между платформами
    int i;



    void Start()
    {
        maxstep = 10;
        GenPos = new Vector3((forgen.transform.position.x + Static.StepPlatf), 0);
        LastFonPos = 16.0F;
    }


    void Update()//генерация !главный метод, вызывающий остальные!
    {
        GenPos.x = forgen.transform.position.x + Static.StepPlatf;

        //if ((LastFonPos + 30.0F) <= GenPos.x)//генерация фоновых деревьев
        //{
        //    LastFonPos += 30.0F;
        //    GameObject forestFon = PoolManager.GetObject(ForestFon.name, new Vector3(LastFonPos, 0), GenQ);
        //}

        if (forgen.busy ==false)//вызывается примерно каждые 2 шага
        {
            GameObject DieArea = PoolManager.GetObject(gr.name, new Vector3(Mathf.Round(forgen.transform.position.x), 10), GenQ);//нижняя граница
            GameObject DieArea2 = PoolManager.GetObject(gr.name, new Vector3(Mathf.Round(forgen.transform.position.x), 20), GenQ);//верхняя граница

            GenPlat(y1);
            GenPlat(y2);
            GenPlat(y3);
            GenPlat(y4);
        }
    }

    void GenPlat(float y)
    {
        if (y <= 0)//уровень высоты
        {
            i = Random.Range(0, sky.Length);//выбираем любую платформу
            GameObject ForestPlatform = PoolManager.GetObject(sky[i].name, new Vector3(GenPos.x, 12F), GenQ);//ставим ее
            y = Random.Range(0, maxstep + 1) + lenghts[i] + 2F;//задаем через скольлко нужно будет сделать следующую платформу на этом уровне высоты
        }
        else { y -= 2; }
    }

}
