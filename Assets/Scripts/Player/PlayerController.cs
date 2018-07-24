using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Control Stats")]
    public float movementSpeed;
    public float rotationSpeed;

    [Header("Arc Attack Effect")]
    public GameObject arcEffect;
    public float arcAttackMovementSpeed;
    public GameObject arcInstantionPosition;

    private Rigidbody playerRB;
    private Animator playerAnimator;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MakePlayerAttack();
        MakePlayerShootArc();
    }

    void MovePlayer()
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.Idle) &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.Run))
        {
            playerRB.velocity = Vector3.zero;
            return;
        }


        float moveZ = Input.GetAxis(PlayerControlsManager.Vertical);
        if (moveZ > 0)
        {
            playerAnimator.SetBool(PlayerControlsManager.Movement, true);
            playerRB.velocity = gameObject.transform.forward * moveZ * movementSpeed * Time.deltaTime;
        }
        else
            playerAnimator.SetBool(PlayerControlsManager.Movement, false);

        float moveX = Input.GetAxis(PlayerControlsManager.Horizontal);
        playerRB.transform.Rotate(Vector3.up * moveX * rotationSpeed * Time.deltaTime);
    }

    void MakePlayerAttack()
    {
        if (Input.GetMouseButton(0))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack))
                playerAnimator.SetInteger(PlayerControlsManager.Attack, 2);
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack))
                playerAnimator.SetInteger(PlayerControlsManager.Attack, 1);

            else
                playerAnimator.SetInteger(PlayerControlsManager.Attack, 1);
        }
        else
        {
            int upcomingAttack = playerAnimator.GetInteger(PlayerControlsManager.Attack);
            if (upcomingAttack == 2 &&
                playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack))
            {
                // Do Nothing
            }
            else if (upcomingAttack == 1 &&
                playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack))
            {
                // Do Nothing
            }
            else
                playerAnimator.SetInteger(PlayerControlsManager.Attack, 0);
        }
    }

    void MakePlayerShootArc()
    {
        if (Input.GetMouseButtonDown(1) &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.ThirdAttack))
            playerAnimator.SetTrigger(PlayerControlsManager.Fire);
    }

    void ShootArc()
    {
        GameObject arcAttackEffect = Instantiate(arcEffect, arcInstantionPosition.transform.position,
                gameObject.transform.rotation) as GameObject;

        Rigidbody arcRigidBody = arcAttackEffect.GetComponent<Rigidbody>();
        arcRigidBody.velocity = gameObject.transform.forward * arcAttackMovementSpeed * Time.deltaTime;
    }
}
