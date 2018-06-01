using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraController : MonoBehaviour
{


    public float damping = 5f;
    public Vector2 offset;
    public bool faceLeft;
    private int lastX;
    [SerializeField]
    Character player2;
    [SerializeField]
    Character player3;
    public Character player;
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
    [SerializeField]
    GameObject Ligting;
    #endregion
    public GameObject EndPlatformForest;
    Vector3 target;//центр для камеры

    bool upCamera2lvl;
    bool upCamera3lvl;
    float Y;//для офсета на уровни

    #region Menu Methods
    public GameObject SubMenu;
    public GameObject LoseMenu;

    public void SetLoseMenu()
    {
        LoseMenu.SetActive(true);
        Time.timeScale = 0;
    }

    //Метод для нажатия на кнопку resume
    public void SetMenuOff()
    {
        //Выключаем меню
        SubMenu.SetActive(false);
        //Запускаем игру дальше
        Time.timeScale = 1;
    }

    public void SetMeinMenu()
    {
        SceneManager.LoadScene("Menu");
    }

    //Метод для нажатия на кнопку exit
    public void CloseGame()
    {
        //Выключаем игру
        Application.Quit();
    }
    #endregion


    void Start()
    {
        offset = new Vector2(Mathf.Abs(offset.x), offset.y);
        ManagerSky.SetActive(false);
        ManagerSpace.SetActive(false);
        Level = 1;
        player = GameObject.Find("Player").GetComponent<Character>();
        Y = offset.y;
    }

    void Update()
    {
        #region Play SubMenu
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            //включаем меню
            SubMenu.SetActive(true);
            //останавливаем все, что зависит от времени
            Time.timeScale = 0;
        }
        #endregion

        #region Play LoseMenu
        if (player.lives <= 0) SetLoseMenu();
        #endregion

        //#region Set Player
        //if (player.isActiveAndEnabled == false)
        //{
        //    if (player==player1) player =player2;
        //    else if (player==player2) player = player3;
        //}
        //#endregion


        #region Camera go
        int currentX = Mathf.RoundToInt(player.transform.position.x);
        lastX = Mathf.RoundToInt(player.transform.position.x);
        //if (player.PlayertLevel != player.PrefabLevel)//значит переходим на уровень выше
        //{
        //    switch (player.PlayertLevel)//зависит от уровня иргрока
        //    {
        //        case 1: Y = offset.y; break;
        //        case 2:
        //            {
        //                if (upCamera2lvl == false)//чтоб камера поднялась и больше не опускалась
        //                {
        //                    if (player.transform.position.y > 12.4F) Y = offset.y + 11F;
        //                    else if (player.transform.position.y < offset.y) { Y = offset.y; }
        //                    else Y = player.transform.position.y;
        //                }
        //            }
        //            break;
        //        case 3:
        //            {
        //                if (upCamera3lvl == false)//чтоб камера поднялась и больше не опускалась
        //                {
        //                    if (player.transform.position.y > 21F) Y = offset.y + 21F;
        //                    else if (player.transform.position.y < offset.y) { Y = offset.y + 11; }
        //                    else Y = player.transform.position.y;
        //                }
        //            }
        //            break;
        //    }
        //}
        //else//не переходим на уровень выше
        //{
        //    if ((ManagerSky.activeInHierarchy == false) && (player.PrefabLevel == 2))
        //    {
        //        Destroy(GameObject.Find("Pool"));
        //        ManagerSky.SetActive(true);
        //    }
        //    else if ((ManagerSpace.activeInHierarchy == false) && (player.PrefabLevel == 3))
        //    {
        //        Destroy(GameObject.Find("Pool"));
        //        ManagerSpace.SetActive(true);
        //    }

        //switch (player.PrefabLevel)
        //{
        //    case 1: Y = offset.y; break;
        //    case 2: Y = offset.y + 11; break;
        //    case 3: Y = offset.y + 21; break;
        //}
        //}
        target = new Vector3(player.transform.position.x + offset.x, Y, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
        transform.position = currentPosition;


        switch (Level)//для создания объектов перехода на сл уровень ()
        {
            case 1:
                if ((player.FireColb > to2lvl))//с 1 на 2
                {
                    Debug.Log("Уровень 2");
                    ManagerForest.SetActive(false);//выключаем текущий менеджер
                    Level = 2;
                    EndPlatformForest.transform.position = new Vector3(transform.position.x - 5F, 0F);//строим  конечные объекты уровня
                    EndPlatformForest.SetActive(true);                    
                    Instantiate(Ligting, player.transform.position + 0.5F * Vector3.up, new Quaternion(0, 0, 0, 0));
                    player.gameObject.SetActive(false);
                    player = Instantiate(player2, player.transform.position, new Quaternion(0, 0, 0, 0));
                    player.FireColb = 0;
                    ManagerSky.SetActive(true);
                }
                break;
            case 2:
                {
                    //движение камеры до уровня
                    if (transform.position.y < offset.y + 10F) { Y = offset.y + player.transform.position.y; }
                    else { Y = offset.y + 11F; }

                    //перехзод на 3 ур
                    if ((player.FireColb > to3lvl) && (Level == 2)) { Debug.Log("++"); Level = 3; ManagerSpace.SetActive(true); }
                    break;//со 2 на 3
                }
            case 3: if ((player.FireColb > toWin)&& (Level == 3)) { Debug.Log("+++"); Level = 4; Debug.Log("WIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIIN"); } break;//выигрыш
        }
        #endregion

    }




}
