using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    [SerializeField]
    private float speed;

    [SerializeField]
    private Transform target;

    Vector3 position;

    private void Start()
    {
        speed = 3.0F;      
        position.z = -10.0F;
    }

    private void Awake()
    {
        if (!target) target = FindObjectOfType<Character>().transform; //поставить в таргет игрока
    }

    private void Update()
    {
        Vector3 position = new Vector3(target.position.x+4,4);
        position.z = -10.0F;
       transform.position =position;
        //Vector3.Lerp(new Vector3(transform.position.x, 3, 0), position,speed*Time.deltaTime);//lerp для плавного движения
    }
	

}
