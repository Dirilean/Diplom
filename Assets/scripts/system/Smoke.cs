﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Smoke : MonoBehaviour {


    IEnumerator ForDeactivate()
    {
        yield return new WaitForSeconds(5F);
        GetComponent<PoolObject>().ReturnToPool();//"удаление" объекта
    }

    private void OnEnable()
    {
        StartCoroutine(ForDeactivate());
    }
}
