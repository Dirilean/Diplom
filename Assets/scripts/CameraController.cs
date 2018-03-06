using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed = 2.0F;

    [SerializeField]
    private Transform target;

    private void Awake()
    {
        if (!target) target = FindObjectOfType<Character>().transform; //поставить в таргет игрока
    }

    private void Update()
    {
        Vector3 position = new Vector3(target.position.x+4,4);
        position.z = -10.0F;
        transform.position = position; //Vector3.Lerp(new Vector3(transform.position.x, 5, 0), position,speed*Time.deltaTime);//lerp для плавного движения
    }
	

}
