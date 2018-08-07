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
    public Animator screenAnimator;
    public float changeSceneAfterTime = 2.1f;

    [Header("Objects to Affect")]
    public GameObject bossEnemy;
    public GameObject minionEnemyHolder;

    public void PlayerDead()
    {
        Destroy(bossEnemy);
        Destroy(minionEnemyHolder);

        majorText.text = "You are Dead !!!";
        majorText.color = Color.red;

        textAnimator.SetTrigger(UIControlsManager.TextParam);
        screenAnimator.SetTrigger(UIControlsManager.FadeOutParam);

        Invoke("LoadMainScene", changeSceneAfterTime);
    }

    public void BossEnemyDead()
    {
        Destroy(minionEnemyHolder);

        majorText.text = "You Won !!!";
        majorText.color = Color.green;

        textAnimator.SetTrigger(UIControlsManager.TextParam);
        screenAnimator.SetTrigger(UIControlsManager.FadeOutParam);

        Invoke("LoadMainScene", changeSceneAfterTime);
    }

    void LoadMainScene()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        SceneManager.LoadScene(0);
    }
}
