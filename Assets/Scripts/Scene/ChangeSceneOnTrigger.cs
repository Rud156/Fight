using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ChangeSceneOnTrigger : MonoBehaviour
{
    [Header("UI Effects")]
    public Animator textAnimator;
    public Text majorText;
    public Animation screenAnimation;

    [Header("Objects to Affect")]
    public GameObject bossEnemy;
    public GameObject minionEnemyHolder;

    public void PlayerDead()
    {
        Destroy(bossEnemy);
        Destroy(minionEnemyHolder);

        majorText.text = "You are Dead !!!";
        majorText.color = Color.red;
        textAnimator.Play(UIControlsManager.TextAnimation);

        screenAnimation.Play(UIControlsManager.FadeOutAnimation);
        Invoke("LoadMainScene", 1.3f);
    }

    public void BossEnemyDead()
    {
        Destroy(minionEnemyHolder);

        majorText.text = "You Won !!!";
        majorText.color = Color.green;
        textAnimator.Play(UIControlsManager.TextAnimation);

        screenAnimation.Play(UIControlsManager.FadeOutAnimation);
        Invoke("LoadMainScene", 1.3f);
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
