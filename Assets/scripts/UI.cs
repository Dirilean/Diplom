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


    void Update ()
    {
        Ognesvet.fillAmount = Player.FireColb/ Static.LevelUp;
        Lives.fillAmount = Player.lives/100.0F;
        
	}
}
