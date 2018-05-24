﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{

    //ссылочная переменная на игрока
    public CameraController camera;
    //позиция игрока в предыдущем кадре
    //и разница в позиции между кадрами
    [SerializeField]
    float lastPos, offset;
    //скорость движения фона
    public float speed;
    public float lenghtSprite;
    public GameObject fon1;
    public GameObject fon2;
    float currentPos1;
    float currentPos2;
    public float deactivateY;

    //выполнится один раз при запуске скрипта
    void Start()
    {
        lastPos = camera.transform.position.x;
        currentPos1 = fon1.transform.position.x;
        currentPos2 = fon2.transform.position.x;
    }

    //выполняется каждый кадр
    void Update()
    {
        //движение
        if (camera.transform.position.y > deactivateY) gameObject.SetActive(false);

        offset = camera.transform.position.x - lastPos;
        lastPos = camera.transform.position.x;

        fon1.transform.Translate(offset * speed * Time.deltaTime, 0, 0);
        fon2.transform.Translate(offset * speed * Time.deltaTime, 0, 0);

        //вперед
        if (camera.transform.position.x >= fon1.transform.position.x + lenghtSprite * 1.5F)
        {
            Debug.Log(gameObject.name +", длина: "+lenghtSprite+ ", 1: " + fon1.transform.position.x + ", " + (fon1.transform.position.x + lenghtSprite * 2));
            fon1.transform.position = new Vector3(fon1.transform.position.x + lenghtSprite*2, fon1.transform.position.y, fon1.transform.position.z);
            
        }

        if (camera.transform.position.x >= fon2.transform.position.x + lenghtSprite * 1.5F)
        {
            Debug.Log(gameObject.name+", длина: " + lenghtSprite + ", 2: " + fon2.transform.position.x + ", " + (fon2.transform.position.x + lenghtSprite * 2));
            fon2.transform.position = new Vector3(fon2.transform.position.x + lenghtSprite*2, fon2.transform.position.y, fon2.transform.position.z);
            
        }

        //назад
        if (camera.transform.position.x <= fon1.transform.position.x - lenghtSprite * 1.5F)
        {
            fon1.transform.position = new Vector3(fon1.transform.position.x - lenghtSprite*2, fon1.transform.position.y, fon1.transform.position.z);
        }

        if (camera.transform.position.x <= fon2.transform.position.x - lenghtSprite * 1.5F)
        {
            fon2.transform.position = new Vector3(fon2.transform.position.x - lenghtSprite*2, fon2.transform.position.y, fon2.transform.position.z);
        }

    }
}