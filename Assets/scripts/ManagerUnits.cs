using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUnits : MonoBehaviour {


    [SerializeField]
    ForGen forgen;
    [SerializeField]
    GameObject[] monsters = new GameObject[3];
    [SerializeField]
    int[] chance = new int[3];
    [SerializeField]
    float[] Hights=new float[3];//позиция генерации по Y

    int RndStep;
    int RndPack;
    int RndY;
    float LastPos;
    //счетчики
    int i;
    int j;
    int sum;
    Collider2D[] colliders;

    float GenPos;//текущая позиция генерации

    // Use this for initialization
    void Start() {
        GameObject player = GameObject.FindWithTag("Player");
        forgen = player.transform.Find("Generator").GetComponent<ForGen>();
        RndStep = 0;
        GenPos = Static.ForgenPosition+ Static.StepGenMonster;
        LastPos = GenPos;
    }

    // Update is called once per frame
    void Update() {
        GenPos = forgen.transform.position.x + Static.StepGenMonster;
        if (GenPos > RndStep + LastPos)
        {
            RndPack = Random.Range(0,100);
            RndY = Random.Range(0, Hights.Length);//выбор случайной высоты
            i = -1;
            sum=0;
         //   Debug.Log("rnd=" + RndPack +"sum="+sum);
            //ищем в диапазоне какокого элемента массива шансов выпало рандомное значение
            for (j=0;j< chance.Length;)
            {
                if (RndPack - chance[j] - sum < 0)
                {
                    i = j;                    
                    colliders = Physics2D.OverlapCircleAll(new Vector3((RndStep + LastPos), Hights[RndY]), 0.3F, 1 << 13);
                 //   Debug.Log((RndPack - chance[j] + sum)+"RndPack - chance[j] + sum " +RndPack+"-"+chance[j]+"+"+sum+"<0"+", i " + i);
                    j = chance.Length;
                }
                else
                {                 
                    sum += chance[j];
                    j++;
                    //Debug.Log("sum "+sum+", j"+j);
                }
            }
            if ((i != -1)&&(colliders.Length==0))//если рандом попал в диапазон  и на месте генерации нет объектов
            { 
                GameObject monster = PoolManager.GetObject(monsters[i].name, new Vector3((RndStep + LastPos), Hights[RndY]), monsters[i].transform.rotation);
              //  Debug.Log("генерация монстра в " + (RndStep + LastPos) + "на высоте: " + Hights[RndY] + ", время: " + Time.time);
            }
            RndStep = Random.Range(5,20);
            LastPos = GenPos;
            
        }
    }


}