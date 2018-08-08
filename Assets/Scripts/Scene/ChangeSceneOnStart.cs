using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ChangeSceneOnStart : MonoBehaviour
{
    public Slider loadingSlider;

    // Use this for initialization
    void Start()
    {
        StartCoroutine(LoadNextSceneAsync());
    }

    IEnumerator LoadNextSceneAsync()
    {
        int sceneIndex = NextSceneToLoad.nextSceneIndex;
        AsyncOperation operation = SceneManager.LoadSceneAsync(sceneIndex);

        while (!operation.isDone)
        {
            loadingSlider.value = operation.progress;
            yield return null;
        }
    }
}
