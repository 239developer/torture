using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    [SerializeField] Image _progressBar;
    public static int sceneToLoad = 1;

    void Start()
    {
        StartCoroutine(LoadSceneAsync());
    }

    IEnumerator LoadSceneAsync()
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneToLoad);
        operation.allowSceneActivation = false;

        while(!operation.isDone)
        {
            _progressBar.fillAmount = operation.progress;

            if(operation.progress >= 0.9f)
            {
                operation.allowSceneActivation = true;
            }
            yield return null;
        } 
    }
}
