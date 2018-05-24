﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerPlatformType2 : MonoBehaviour {

    public GameObject[] sky = new GameObject[13]; //все платформы
    public float[] lenghts = new float[13]; //длины платформ
    public GameObject gr;//смертельная область
    Vector3 GenPos;//текущая позиция генерации
    Quaternion GenQ = new Quaternion(0, 0, 0, 0);//текущий разворот
    [SerializeField]
    ForGen forgen;
    float LastFonPos;//координата последнего фонового рисунка
    //[SerializeField]
    //GameObject Fon;

    //________________
    float x1;//расстояние между платформами
    float x2;
    float x1last;//расстояние между платформами предыдущее
    float x2last;
    float y1=13F;//высота платформ
    float y2=17F;
    public float maxstep=10;//максимальный шаг между платформами
    int i;
    float lastGroundPos;
    public float GroundLenght = 4F;



    void Start()
    {
        GenPos = new Vector3((forgen.transform.position.x + Static.StepPlatf), 0);
        LastFonPos = 16.0F;
        GameObject player = GameObject.Find("Player2(Clone)");
      //  forgen = player.transform.parent.gameObject.transform.Find("Generator").GetComponent<ForGen>();
        forgen = player.transform.Find("Generator").GetComponent<ForGen>();
        lastGroundPos = forgen.transform.position.x + 20F;
    }


    void Update()//генерация !главный метод, вызывающий остальные!
    {
        GenPos.x = forgen.transform.position.x + Static.StepPlatf;

        if (lastGroundPos + GroundLenght < forgen.transform.position.x)
        { 
            GameObject DieArea = PoolManager.GetObject(gr.name, new Vector3(Mathf.Round(forgen.transform.position.x), 9), GenQ);//нижняя граница

            GenPlat(ref x1, ref y1,ref x1last);
            GenPlat(ref x2,ref y2,ref x2last);
            lastGroundPos = forgen.transform.position.x;
        }
    }

    void GenPlat(ref float x,ref float y,ref float xlast)
    {
        if (GenPos.x >=xlast+x)
        {
            xlast = GenPos.x;
            i = Random.Range(0, sky.Length);//выбираем любую платформу
            GameObject ForestPlatform = PoolManager.GetObject(sky[i].name, new Vector3(GenPos.x, Random.Range(-1,2)+y), GenQ);//ставим ее
            x = Random.Range(2, maxstep-4) + lenghts[i];//задаем через скольлко нужно будет сделать следующую платформу на этом уровне высоты
           // Debug.Log("posX "+GenPos.x+" lengh="+lenghts[i]+" next random ="+x);
        }
    }

}
