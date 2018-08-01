﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Control Stats")]
    public float movementSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    [Header("Sword and Foot Colliders")]
    public GameObject swordContact;
    public GameObject footContact;

    private Rigidbody playerRB;
    private Animator playerAnimator;

    private bool isFalling;
    private bool jumped;

    private bool forwardMovementKeyRemoved;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();

        isFalling = false;
        jumped = false;

        forwardMovementKeyRemoved = false;

        PlayerData.yaw = gameObject.transform.rotation.eulerAngles.y;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        EnableAndDisableColliders();

        MakePlayerAttack();

        MovePlayer();

        MakePlayerJump();
        MakePlayerFall();
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!isFalling)
            return;

        playerAnimator.SetBool(PlayerControlsManager.FallParam, false);
        isFalling = false;
    }

    void EnableAndDisableColliders()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack))
            footContact.SetActive(true);
        else
            footContact.SetActive(false);

        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack))
            swordContact.SetActive(true);
        else
            swordContact.SetActive(false);
    }

    void MovePlayer()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack) ||
        playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack) ||
        playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.ThirdAttack))
        {
            float yVelocity = playerRB.velocity.y;
            playerRB.velocity = new Vector3(0, yVelocity, 0);
            return;
        }

        if (isFalling || jumped)
            return;


        float moveZ = isFalling ? 0 : Input.GetAxis(PlayerControlsManager.Vertical);
        if (moveZ > 0)
        {
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, true);
            playerRB.velocity = gameObject.transform.forward * moveZ * movementSpeed * Time.deltaTime;
            forwardMovementKeyRemoved = false;
        }
        else
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, false);

        if (moveZ == 0 && !forwardMovementKeyRemoved)
        {
            playerRB.velocity = Vector3.zero;
            forwardMovementKeyRemoved = true;
        }

        float moveX = Input.GetAxis(PlayerControlsManager.Horizontal);
        PlayerData.yaw += moveX * rotationSpeed * Time.deltaTime;
        gameObject.transform.eulerAngles = Vector3.up * PlayerData.yaw;

    }

    void MakePlayerAttack()
    {
        if (Input.GetMouseButton(0))
        {
            if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack))
                playerAnimator.SetInteger(PlayerControlsManager.AttackParam, 2);
            else if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack))
                playerAnimator.SetInteger(PlayerControlsManager.AttackParam, 1);

            else
                playerAnimator.SetInteger(PlayerControlsManager.AttackParam, 1);
        }
        else
        {
            int upcomingAttack = playerAnimator.GetInteger(PlayerControlsManager.AttackParam);
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
                playerAnimator.SetInteger(PlayerControlsManager.AttackParam, 0);
        }
    }

    void MakePlayerJump()
    {
        if (Input.GetKeyDown(PlayerControlsManager.JumpKeyboard) && !isFalling && !jumped)
        {
            playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerAnimator.SetTrigger(PlayerControlsManager.JumpParam);
            jumped = true;
        }
    }

    void MakePlayerFall()
    {
        float yVelocity = playerRB.velocity.y;
        if (yVelocity < -1 && !isFalling)
        {
            playerAnimator.SetBool(PlayerControlsManager.FallParam, true);
            isFalling = true;
            jumped = false;
        }
    }
}
