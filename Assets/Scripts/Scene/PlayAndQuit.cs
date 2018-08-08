using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayAndQuit : MonoBehaviour
{
    public void PlayGame()
    {
        NextSceneToLoad.nextSceneIndex = 1;
        // Reset Health
        PlayerData.currentHealthLeft = PlayerData.maxHealth;
        SceneManager.LoadScene(2);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
