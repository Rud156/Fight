using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShootArc : MonoBehaviour
{
    [Header("Arc UI")]
    public Color minWaitColor;
    public Color halfWaitColor;
    public Color maxWaitColor;
    public Slider arcWaitSlider;
    public Image arcWaitFiller;

    [Header("Arc Attack Effect")]
    public GameObject arcEffect;
    public float arcAttackMovementSpeed = 3000;
    public GameObject arcInstantionPosition;
    public int waitForTotalFrames = 120;

    private bool arcShotStarted;
    private int currentFrameCount;

    private Animator playerAnimator;

    // Use this for initialization
    void Start()
    {
        arcShotStarted = false;
        currentFrameCount = waitForTotalFrames;

        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MakePlayerShootArc();

        int maxWaitTime = waitForTotalFrames;
        int currentWaitTime = currentFrameCount;
        float waitRatio = (float)currentWaitTime / maxWaitTime;

        if (waitRatio <= 0.5)
            arcWaitFiller.color = Color.Lerp(minWaitColor, halfWaitColor, waitRatio * 2);
        else
            arcWaitFiller.color = Color.Lerp(halfWaitColor, maxWaitColor, (waitRatio - 0.5f) * 2);
        arcWaitSlider.value = waitRatio;
    }

    void MakePlayerShootArc()
    {
        if (currentFrameCount < waitForTotalFrames)
            currentFrameCount += 1;

        if (Input.GetMouseButtonDown(1) &&
        !arcShotStarted && currentFrameCount >= waitForTotalFrames)
        {
            playerAnimator.SetTrigger(PlayerControlsManager.FireParam);
            arcShotStarted = true;
            currentFrameCount = waitForTotalFrames;
        }
    }

    void InstantiateArc()
    {
        GameObject arcAttackEffect = Instantiate(arcEffect, arcInstantionPosition.transform.position,
                gameObject.transform.rotation) as GameObject;

        Rigidbody arcRigidBody = arcAttackEffect.GetComponent<Rigidbody>();
        arcRigidBody.velocity = gameObject.transform.forward * arcAttackMovementSpeed * Time.deltaTime;
        arcShotStarted = false;
    }
}
