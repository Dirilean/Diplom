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

    System.Random rnd = new System.Random();
    int RndStep;
    int RndPack;
    float LastPos;

    // Use this for initialization
    void Start () {
        RndStep = 0;
        LastPos = 20;
	}
	
	// Update is called once per frame
	void Update () {

		if (forgen.transform.position.x>RndStep+LastPos)
        {
            RndPack = rnd.Next(4);
            switch (RndPack)
            {
                case 0: Wolf Wolf = Instantiate(WolfPrefab, new Vector3((RndStep + LastPos), 10), WolfPrefab.transform.rotation); break;
                case 1: PassiveEnemy BlackHole = Instantiate(HolePrefab, new Vector3((RndStep + LastPos), 10), HolePrefab.transform.rotation); break;
                case 2: Bear Bear = Instantiate(BearPrefab, new Vector3((RndStep + LastPos), 10), BearPrefab.transform.rotation); break;
                case 3: Ezh Ezh = Instantiate(EzhPrefab, new Vector3((RndStep + LastPos), 10), EzhPrefab.transform.rotation); break;
            }
            RndStep =rnd.Next(15)+5;
            LastPos = forgen.transform.position.x;
        }
	}
}
