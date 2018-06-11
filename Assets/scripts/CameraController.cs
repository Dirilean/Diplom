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
    public Character player;
    [SerializeField]
    Character player2;
    [SerializeField]
    Character player3;
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
    [SerializeField]
    GameObject Ligting2;
    #endregion
    public GameObject StartSky;
    public GameObject StartSpace;
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
        if (player.lives <= -100) SetLoseMenu();
        #endregion

        #region Camera go
        int currentX = Mathf.RoundToInt(player.transform.position.x);
        lastX = Mathf.RoundToInt(player.transform.position.x);
      
        target = new Vector3(player.transform.position.x + offset.x, Y, transform.position.z);
        Vector3 currentPosition = Vector3.Lerp(transform.position, target, damping * Time.deltaTime);
        transform.position = currentPosition;
        #endregion

        #region Level

        switch (Level)//для создания объектов перехода на сл уровень ()
        {
            case 1:
                {
                    if (player.FireColb > to2lvl)//с 1 на 2
                    {
                        Debug.Log("Уровень 2");
                        ManagerForest.SetActive(false);//выключаем текущий менеджер   
                        StartSky.transform.position = new Vector3(transform.position.x - 5F, 0F);//строим  конечные объекты уровня
                        StartSky.SetActive(true);
                        Ligting2.SetActive(true);
                        Ligting2.transform.localPosition = new Vector3(-8F, -4F);
                        player.FireColb = 0;
                        Level = 2;
                    }
                    break;
                }
            case 2:
                {
                    //движение камеры до уровня
                    if (transform.position.y < offset.y + 10F)
                    {
                        if (Ligting.activeInHierarchy == false&&(Mathf.Abs(player.transform.position.x - Ligting2.transform.position.x) < 0.05F))
                        {
                            Ligting.transform.position = player.transform.position + 0.5F * Vector3.up;
                            Ligting.SetActive(true);
                            player.gameObject.SetActive(false);
                            player = Instantiate(player2, player.transform.position, new Quaternion(0, 0, 0, 0));
                            Ligting2.SetActive(false);
                            Destroy(GameObject.Find("Pool"));
                            ManagerSky.SetActive(true);
                            
                        }
                        Y = offset.y + player.transform.position.y;
                        Ligting2.transform.position = Vector3.MoveTowards(Ligting2.transform.position, player.transform.position + 0.5F * Vector3.up, 5F*Time.deltaTime);
                        Ligting.transform.position = player.transform.position + 0.5F * Vector3.up;
                    }
                    else
                    {
                        Ligting.SetActive(false);
                        Y = offset.y + 11F;
                    }// обычное движение
                    if ((player.FireColb > to3lvl))//переход с 2 на 3
                    {
                        Debug.Log("Уровень 3 ");
                        ManagerSky.SetActive(false);//выключаем текущий менеджер
                        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.transform.position, 30F, 1 << 12);
                        for (int i = 0; i < colliders.Length; i++)
                        {
                            if (colliders[i].GetComponent<PoolObject>())
                            {
                                colliders[i].GetComponent<PoolObject>().ReturnToPool();
                            }
                        }
                        StartSpace.transform.position = new Vector3(transform.position.x - 5F, 0F);//строим  конечные объекты уровня
                        StartSpace.SetActive(true);
                        Ligting2.SetActive(true);
                        Ligting2.transform.localPosition = new Vector3(-8F, -4F);
                        player.FireColb = 0;
                        Level = 3;
                    }
                    break;
                }
            case 3:
                {
                    //движение камеры до уровня
                    if (transform.position.y < offset.y + 20F)
                    {
                        if (Ligting.activeInHierarchy == false && (Mathf.Abs(player.transform.position.x - Ligting2.transform.position.x) < 0.05F))
                        {
                            Ligting.transform.position = player.transform.position + 0.5F * Vector3.up;
                            Ligting.SetActive(true);
                            player.gameObject.SetActive(false);
                            player = Instantiate(player3, player.transform.position, new Quaternion(0, 0, 0, 0));
                            Ligting2.SetActive(false);
                            Destroy(GameObject.Find("Pool"));
                            ManagerSpace.SetActive(true);
                        }
                        Y = offset.y + player.transform.position.y;
                        Ligting2.transform.position = Vector3.MoveTowards(Ligting2.transform.position, player.transform.position + 0.5F * Vector3.up, 5F * Time.deltaTime);
                        Ligting.transform.position = player.transform.position + 0.5F * Vector3.up;
                    }
                    else
                    {
                        Ligting.SetActive(false);
                        Y = offset.y + 21F;
                    }// обычное движение
                    if ((player.FireColb > toWin))//переход с 2 на 3
                    {
                        Debug.Log("Победа!!");
                    }
                        break;//выигрыш
                }
        }
        #endregion

    }
    IEnumerator Magic()
    {
        yield return new WaitForSeconds(2F);
        Ligting2.SetActive(false);
        yield return new WaitForSeconds(10F);
        Ligting.SetActive(false);
    }




}
