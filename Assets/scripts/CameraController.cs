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
    [SerializeField]
    GameObject ManagerForest;
    [SerializeField]
    GameObject ManagerSky;
    [SerializeField]
    GameObject ManagerSpace;

    #region Level settings
    [Header("Level settings")]
    public int to2lvl;
    public int to3lvl;
    public int toWin;
    public int Level;
    #endregion
    public GameObject EndPlatformForest;
    Vector3 target;//центр для камеры

    bool upCamera2lvl;
    bool upCamera3lvl;
    float Y;//для офсета на уровни

    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        ManagerSky.SetActive(false);
        ManagerSpace.SetActive(false);
        Level = 1;
        player = GameObject.FindWithTag("Player").GetComponent<Character>();
    }

    void Update()
    {
        int currentX = Mathf.RoundToInt(player.transform.position.x);
        lastX = Mathf.RoundToInt(player.transform.position.x);
        switch (player.PlayertLevel)//зависит от уровня иргрока
        {
            case 1: Y = offset.y; break;
            case 2:
                    {
                        if (upCamera2lvl==false)//чтоб камера поднялась и больше не опускалась
                        {
                            if (player.transform.position.y > 12.4F) Y=offset.y + 11F;
                            else if (player.transform.position.y < offset.y) { Y=offset.y; }
                            else Y= player.transform.position.y;
                        }
                    }
                    break;
            case 3:
                {
                    if (upCamera3lvl == false)//чтоб камера поднялась и больше не опускалась
                    {
                        if (player.transform.position.y > 21F) Y = offset.y + 21F;
                        else if (player.transform.position.y < offset.y) { Y = offset.y+11;}
                        else Y = player.transform.position.y;
                    }
                }
                break;
        }
        target = new Vector3(player.transform.position.x + offset.x, Y, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
        transform.position = currentPosition;


        switch (Level)//для создания объектов перехода на сл уровень
        {
            case 1:
                if ((player.FireColb > to2lvl)&&(Level==1))//с 1 на 2
                {
                    Level = 2;
                    Vector3 r = new Vector3(ManagerForest.GetComponent<ManagerPlatform>().GenPos.x+Static.StepPlatf, 0F);
                    Debug.Log(r);
                    GameObject endPlatformForest = Instantiate(EndPlatformForest, r, new Quaternion(0,0,0,0));//строим  конечные объекты уровня
                    Debug.Log(endPlatformForest.transform.position);
                    ManagerForest.SetActive(false);//выключаем этот менеджер
                }
                break;
            case 2: if ((player.FireColb > to3lvl)&& (Level == 2)) { Debug.Log("++"); Level = 3; ManagerSpace.SetActive(true); } break;//со 2 на 3
            case 3: if ((player.FireColb > toWin)&& (Level == 3)) { Debug.Log("+++"); Level = 4; Debug.Log("WIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIN"); } break;//выигрыш
        }
        
    }




}
