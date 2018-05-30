using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    private Character player;
    public Image Ognesvet;
    public Image Pero;
    public Image Ogon;
    public Image Lives;
    public Text kolvoOgnya;
    public Image ReciveDamage;
    public Image EndFly;
    public float currentAlfa;//текущиая альфа
    float LastLives;
    float NextAlfa;

    private void Start()
    {
        player = GameObject.Find("Player").GetComponent<Character>();
        Pero.color = new Color(1, 1, 1, 0);//изначально прозрачные
        Ogon.color= new Color(1, 1, 1, 0);
        Lives.fillAmount = player.lives / 100.0F;
        Ognesvet.fillAmount =player.FireColb / Static.LevelUp;
        LastLives = player.lives;
    }

    void Update ()
    {
        if (player.isActiveAndEnabled == false)
        {
            if (player.name == "Player") player = GameObject.Find("Player2(Clone)").GetComponent<Character>();
            else if (player.name == "Player2(Clone)") player = GameObject.Find("Player3(Clone)").GetComponent<Character>();
        }

        Lives.fillAmount = Mathf.Lerp(Lives.fillAmount, (player.lives / 100.0F),Time.deltaTime*5);
        Ognesvet.fillAmount = Mathf.Lerp(Ognesvet.fillAmount,(player.FireColb / Static.LevelUp), Time.deltaTime);
        kolvoOgnya.text = player.FireColb.ToString();
        Pero.color = new Color(Pero.color.r,Pero.color.g,Pero.color.b,player.FlyResourse/100F);

        //изменение прозрачности при перезарядке конвертации жизней
        if (Time.time < player.LastTimeToPlusLives + player.TimeToPlusLives)//если идет перезарядка
        {
            Ognesvet.color = new Color(Ognesvet.color.r, Ognesvet.color.g, Ognesvet.color.b, 0.2F);
        }
        else //возвращение цвета
        {
            Ognesvet.color = new Color(Ognesvet.color.r, Ognesvet.color.g, Ognesvet.color.b, 1.0F);
        }

      //  Debug.Log(LastLives+"> "+player.lives);
        //отображение экрана получения урона
        if (LastLives>player.lives)
        {
            
            ReciveDamage.GetComponent<Animation>().Play();
        }
        LastLives = player.lives;


        //отображение экрана заканчивающегося ресурса полета

        if (player.FlyResourse<40F && player.FlyResourse>=0)
        {
            Debug.Log("полетка кончается");
            EndFly.GetComponent<Animation>().Play();
        }
    }

}
