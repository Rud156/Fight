using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    private Animator textAnimator;
    private Text majorText;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        GameObject majorTextObject = GameObject.FindGameObjectWithTag(TagsManager.DisplayText);
        textAnimator = majorTextObject.GetComponent<Animator>();
        majorText = majorTextObject.GetComponent<Text>();
    }

    void OnParticleSystemStopped()
    {
        DisplayText();
    }

    void DisplayText()
    {
        majorText.text = "You are dead !!!";
        majorText.color = Color.green;
        textAnimator.Play("TextZoomIn");

        Invoke("LoadMainScene", 1.3f);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
