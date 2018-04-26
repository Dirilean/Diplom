using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUnits : MonoBehaviour {


    [SerializeField]
    ForGen forgen;
    [SerializeField]
    Monster WolfPrefab;
    [SerializeField]
    PassiveEnemy MuskiPrefab;
    [SerializeField]
    Monster BearPrefab;
    [SerializeField]
    Ezh EzhPrefab;
    [SerializeField]
    Lynx LynxPrefab;

    System.Random rnd = new System.Random();
    int RndStep;
    int RndPack;
    float LastPos;

    float GenPos;//текущая позиция генерации

    // Use this for initialization
    void Start() {
        RndStep = 0;
        GenPos = Static.ForgenPosition+ Static.StepGenMonster;
        LastPos = GenPos;
    }

    // Update is called once per frame
    void Update() {
        GenPos = forgen.transform.position.x + Static.StepGenMonster;
        if (GenPos > RndStep + LastPos)
        {
            RndPack = rnd.Next(14);
            switch (RndPack)
            {
                case 0: wolf(); break;
                case 1: wolf(); break;
                case 2: wolf(); break;
                case 3: wolf(); break;

                case 4: bear(); break;
                case 5: bear(); break;
                case 6: bear(); break;

                case 7: lynx(); break;
                case 8: lynx(); break;

                case 9: ezh(); break;
                case 10: ezh(); break;

                case 11: moski(); break;
                case 12: moski(); break;
                case 13: moski(); break;
            }
            RndStep = rnd.Next(15) + 5;
            LastPos = GenPos;
            //Debug.Log("генерация монстра в "+ (RndStep + LastPos)+", время: "+Time.time);
        }
    }


    void wolf()
    {
        Monster Wolf = PoolManager.GetObject(WolfPrefab.name, new Vector3((RndStep + LastPos), 10), WolfPrefab.transform.rotation).GetComponent<Monster>();
    }
    void bear()
    {
        Monster Bear = PoolManager.GetObject(BearPrefab.name, new Vector3((RndStep + LastPos), 10), BearPrefab.transform.rotation).GetComponent<Monster>();
    }
    void lynx()
    {
        Lynx Lynx = PoolManager.GetObject(LynxPrefab.name, new Vector3((RndStep + LastPos), 10), LynxPrefab.transform.rotation).GetComponent<Lynx>();
    }
    void ezh()
    {
        Ezh Ezh = PoolManager.GetObject(EzhPrefab.name, new Vector3((RndStep + LastPos), 10), EzhPrefab.transform.rotation).GetComponent<Ezh>();
    }
    void moski()
    {
        PassiveEnemy Muski = PoolManager.GetObject(MuskiPrefab.name, new Vector3((RndStep + LastPos), 10), MuskiPrefab.transform.rotation).GetComponent<PassiveEnemy>();
    }
}