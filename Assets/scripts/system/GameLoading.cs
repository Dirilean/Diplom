using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class GameLoading : MonoBehaviour
{

    public GameObject loadingInfo, loadingIcon;
    private AsyncOperation async;

    IEnumerator Start()
    {
        async = SceneManager.LoadSceneAsync("Demo");
        loadingIcon.SetActive(true);
        loadingInfo.SetActive(false);
        Debug.Log("load...");
        yield return true;
        async.allowSceneActivation = false;
        loadingIcon.SetActive(false);
        loadingInfo.SetActive(true);
        Debug.Log("Load complete");
    }

    void Update()
    {
        if (Input.anyKeyDown)
        {
            async.allowSceneActivation = true;
            Debug.Log(async+", "+ async.allowSceneActivation);
        }
    }
}