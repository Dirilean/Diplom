using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Learning : MonoBehaviour {

    [SerializeField]
    Image LearningImage;
    float deltaColor = 0F;
    float color;

    private void Start()
    {
        //Debug.Log(LearningImage.name);
        
        //LearningImage= GameObject.Find("Обучение/"+LearningImage.name).GetComponent<Image>();
    }
    //входим в зону триггера, запускаем плавное появление картинки
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.GetComponent<Character>())
        {
            LearningImage.gameObject.SetActive(true);
            color = 1F;
        }
    }

    //вызодим из зоны триггера, запускаем коррутину
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.GetComponent<Character>())
        {
            StartCoroutine(LiveTime());
        }
    }

    //меняем прозрачность картинки
    private void Update()
    {
        deltaColor = Mathf.Lerp(deltaColor, color, Time.deltaTime*2F);
        LearningImage.color = new Color(255,255,255, deltaColor);
    }

    //через 8 секунд плавно скрываем картинку
    IEnumerator LiveTime()
    {
        yield return new WaitForSeconds(1F);
        color = 0F;
        yield return new WaitForSeconds(8F);
        LearningImage.gameObject.SetActive(false);
    }
}
