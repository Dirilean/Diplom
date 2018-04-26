using UnityEngine;
using System.Collections;

// Создаем экземпляр класса для звука на основе кода без усилий
public class SoundEffectsHelper : MonoBehaviour
{

    // Синглтон
    public static SoundEffectsHelper Instance;

    private void Start()
    {
        i = 0;
        j = 0;
        checkaudio = true;
    }

    void Awake()
    {
        // регистрируем синглтон
        if (Instance != null)
        {
            Debug.LogError("Несколько экземпляров SoundEffectsHelper!");
        }
        Instance = this;
    }

    #region run sounds
    public AudioClip[] RunSound=new AudioClip[7];//звуки шагов
    float[] times = new float[4] {0.12F,0.03F,0.36F,0.03F};//тайминг шагов
    int i,j;
    bool checkaudio;
    

    public void MakeRunSound()
    {
        if (checkaudio)//воспроизведение по очереди
        {
            StartCoroutine(AudioWalk(RunSound[i],times[j]));
            i++;
            j++;
            if (i == RunSound.Length) i = 0;
            if (j == times.Length) j = 0;
        }
    }

    IEnumerator AudioWalk(AudioClip runsound, float time)// для задержки 
    {
        checkaudio = false;//нельзя воспроизводить другую музыку
        yield return new WaitForSeconds(time);
        MakeSound(runsound);
        checkaudio = true;
    }
    #endregion


    #region jump

    #endregion
    // Играть данный звук
    private void MakeSound(AudioClip originalClip)
    {
       AudioSource.PlayClipAtPoint(originalClip, transform.position);
    }
}