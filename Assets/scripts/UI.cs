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
        Lives.fillAmount = Mathf.Lerp(Lives.fillAmount, (Player.lives / 100.0F),Time.deltaTime*5);
        Ognesvet.fillAmount = Mathf.Lerp(Ognesvet.fillAmount,(Player.FireColb / Static.LevelUp), Time.deltaTime);

        //изменение прозрачности при перезарядке конвертации жизней
        if (Time.time - Player.LastTimeToPlusLives < 0.1F) currentAlfa = 0.3F;//сбрасывание цвета
        if (Time.time < Player.LastTimeToPlusLives + Player.TimeToPlusLives)//если идет перезарядка
        {
            currentAlfa = Mathf.Lerp(currentAlfa, 1, Time.deltaTime / Player.TimeToPlusLives*3);
            Ognesvet.color = new Color(Ognesvet.color.r, Ognesvet.color.g, Ognesvet.color.b, currentAlfa);
        }
        
    }
}
