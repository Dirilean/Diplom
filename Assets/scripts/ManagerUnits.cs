using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUnits : MonoBehaviour {


    [SerializeField]
    ForGen forgen;
    [SerializeField]
    Wolf WolfPrefab;
    [SerializeField]
    PassiveEnemy MuskiPrefab;
    [SerializeField]
    Bear BearPrefab;
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
        Wolf Wolf = Instantiate(WolfPrefab, new Vector3((RndStep + LastPos), 10), WolfPrefab.transform.rotation);
    }
    void bear()
    {
        Bear Bear = Instantiate(BearPrefab, new Vector3((RndStep + LastPos), 10), BearPrefab.transform.rotation);
    }
    void lynx()
    {
        Lynx Lynx = Instantiate(LynxPrefab, new Vector3((RndStep + LastPos), 10), BearPrefab.transform.rotation);
    }
    void ezh()
    {
        Ezh Ezh = Instantiate(EzhPrefab, new Vector3((RndStep + LastPos), 10), LynxPrefab.transform.rotation);
    }
    void moski()
    {
        Debug.Log("+");
        PassiveEnemy Moski = Instantiate(MuskiPrefab, new Vector3((RndStep + LastPos), 10), MuskiPrefab.transform.rotation);
    }
}