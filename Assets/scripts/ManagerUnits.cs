using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUnits : MonoBehaviour {


    [SerializeField]
    ForGen forgen; //ссылка на генератор
    float GenPos;//текущая позиция генерации

    [SerializeField]
    GameObject[] monsters = new GameObject[3];//префабы монстров
    [SerializeField]
    int[] chance = new int[3];//массив шансов выпадения определенного вида монстра
    [SerializeField]
    float[] Hights=new float[3];//позиции генерации по Y для конкретного уровня

    int RndStep;//случайный шаг для генерации следующего монстра
    int RndPack;//число для выбора конкретного монстра (взаимодействует с массивом шансов их выпадения)
    int RndY;//текущая случайная высота генерации монстра
    float LastPos;//последняя позиция генерации монстра

    //счетчики
    int i;
    int j;
    int sum;

    Collider2D[] colliders;//для опредения объектов в месте генерации монстра

    void Start()
    {
        GameObject player = GameObject.FindWithTag("Player");
        forgen = player.transform.Find("Generator").GetComponent<ForGen>();
        RndStep = 0;
        GenPos = Static.ForgenPosition+ Static.StepGenMonster;
        LastPos = GenPos;
    }

    void Update()
    {
        GenPos = forgen.transform.position.x + Static.StepGenMonster;
        if (GenPos > RndStep + LastPos)
        {
            RndPack = Random.Range(0,100);
            RndY = Random.Range(0, Hights.Length);//выбор случайной высоты
            i = -1;
            sum=0;
            //ищем в диапазоне какокого элемента массива шансов выпало рандомное значение
            for (j=0;j< chance.Length;)
            {
                if (RndPack - chance[j] - sum < 0)
                {
                    i = j;                    
                    colliders = Physics2D.OverlapCircleAll(new Vector3((RndStep + LastPos), Hights[RndY]), 0.3F, 1 << 13);
                    j = chance.Length;
                }
                else
                {                 
                    sum += chance[j];
                    j++;
                }
            }
            if ((i != -1)&&(colliders.Length==0))//если рандом попал в диапазон  и на месте генерации нет объектов
            { 
                GameObject monster = PoolManager.GetObject(monsters[i].name, new Vector3((RndStep + LastPos), Hights[RndY]), monsters[i].transform.rotation);
            }
            RndStep = Random.Range(5,20);
            LastPos = GenPos;          
        }
    }
}