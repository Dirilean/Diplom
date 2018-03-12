using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManagerUnits : MonoBehaviour {


    [SerializeField]
    ForGen forgen;
    [SerializeField]
    Wolf WolfPrefab;
    [SerializeField]
    PassiveEnemy HolePrefab;
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

    // Use this for initialization
    void Start() {
        RndStep = 0;
        LastPos = 20;
    }

    // Update is called once per frame
    void Update() {

        if (forgen.transform.position.x > RndStep + LastPos)
        {
            RndPack = rnd.Next(11);
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

                case 11: hole(); break;
                case 12: hole(); break;
                case 13: hole(); break;
            }
            RndStep = rnd.Next(15) + 5;
            LastPos = forgen.transform.position.x;
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
    void hole()
    {
        PassiveEnemy BlackHole = Instantiate(HolePrefab, new Vector3((RndStep + LastPos), 10), HolePrefab.transform.rotation);
    }
}