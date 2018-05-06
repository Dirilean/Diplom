using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UI : MonoBehaviour {

    public Character Player;
    public Image Ognesvet;
    public Image Pero;
    public Image Ogon;
    public Image Lives;
    public Text text;
    public float currentAlfa;//текущиая альфа

    private void Start()
    {
        Pero.color = new Color(1, 1, 1, 0);//изначально прозрачные
        Ogon.color= new Color(1, 1, 1, 0);
        Lives.fillAmount = Player.lives / 100.0F;
        Ognesvet.fillAmount =Player.FireColb / Static.LevelUp;
        
    }

    void Update ()
    {
        if ((Player == null)||(Player.enabled==false))
        {
            Player = GameObject.FindWithTag("Player").GetComponent<Character>();
        }

        Lives.fillAmount = Mathf.Lerp(Lives.fillAmount, (Player.lives / 100.0F),Time.deltaTime*5);
        Ognesvet.fillAmount = Mathf.Lerp(Ognesvet.fillAmount,(Player.FireColb / Static.LevelUp), Time.deltaTime);
        text.text = Player.FireColb.ToString();

        Pero.color = new Color(Pero.color.r,Pero.color.g,Pero.color.b,Player.FlyResourse/100F);

        //изменение прозрачности при перезарядке конвертации жизней
        if (Time.time < Player.LastTimeToPlusLives + Player.TimeToPlusLives)//если идет перезарядка
        {
            Ognesvet.color = new Color(Ognesvet.color.r, Ognesvet.color.g, Ognesvet.color.b, 0.2F);
        }
        else //возвращение цвета
        {
            Ognesvet.color = new Color(Ognesvet.color.r, Ognesvet.color.g, Ognesvet.color.b, 1.0F);
        }

    }
}
