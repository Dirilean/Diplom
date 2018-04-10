using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {


    private void Start()
    {
        Destroy(gameObject, 4F); //уничтожить объект с задержкой в 0.5сек
    }
}
