using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public float damping = 5f;
    public Vector2 offset = new Vector2(2f, 3f);
    public bool faceLeft;
    private int lastX;
    [SerializeField]
    Character player;

    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        FindPlayer(faceLeft);
        faceLeft=false;
    }

    public void FindPlayer(bool playerFaceLeft)
    {
        lastX = Mathf.RoundToInt(player.transform.position.x);
        if (playerFaceLeft)
        {
            transform.position = new Vector3(player.transform.position.x - offset.x, offset.y, transform.position.z);
        }
        else
        {
            transform.position = new Vector3(player.transform.position.x + offset.x, offset.y, transform.position.z);
        }
    }

    void Update()
    {
        if (player)
        {
            int currentX = Mathf.RoundToInt(player.transform.position.x);
          //  if (currentX > lastX) faceLeft = false; else if (currentX < lastX) faceLeft = true;
            lastX = Mathf.RoundToInt(player.transform.position.x);

            Vector3 target;
            if (faceLeft)
            {
                target = new Vector3(player.transform.position.x - offset.x, offset.y, transform.position.z);
            }
            else
            {
                target = new Vector3(player.transform.position.x + offset.x, offset.y, transform.position.z);
            }
            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }


    //_____________________________

    //[SerializeField]
    //private float speed;

    //[SerializeField]
    //private Transform target;

    //Vector3 position;

    //private void Start()
    //{
    //    speed = 3.0F;      
    //    position.z = -10.0F;
    //}

    //private void Awake()
    //{
    //    if (!target) target = FindObjectOfType<Character>().transform; //поставить в таргет игрока
    //}

    //private void Update()
    //{
    //    Vector3 position = new Vector3(target.position.x+4,4);
    //    position.z = -10.0F;
    //   transform.position =position;
    //    //Vector3.Lerp(new Vector3(transform.position.x, 3, 0), position,speed*Time.deltaTime);//lerp для плавного движения
    //}


}
