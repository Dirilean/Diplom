using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{


    public float damping = 5f;
    public Vector2 offset;
    public bool faceLeft;
    private int lastX;
    [SerializeField]
    Character player;

    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
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
            lastX = Mathf.RoundToInt(player.transform.position.x);

            Vector3 target = new Vector3(player.transform.position.x + offset.x, offset.y, transform.position.z);

            Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
            transform.position = currentPosition;
        }
    }




}
