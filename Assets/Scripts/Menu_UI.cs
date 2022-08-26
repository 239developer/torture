using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu_UI : MonoBehaviour
{
    [SerializeField] public int loadingScreenScene = 0; 

    public void ChangeScene(int scene)
    {
        SceneLoader.sceneToLoad = scene;
        SceneManager.LoadScene(loadingScreenScene);
    }
}
