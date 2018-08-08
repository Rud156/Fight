using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAndQuit : MonoBehaviour
{
    public void PlayGame()
    {
        NextSceneToLoad.nextSceneIndex = 1;
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
