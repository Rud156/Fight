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
    public float jumpSpeed;

    [Header("Arc Attack Effect")]
    public GameObject arcEffect;
    public float arcAttackMovementSpeed;
    public GameObject arcInstantionPosition;

    private Rigidbody playerRB;
    private Animator playerAnimator;
    private bool arcShotStarted;

    private bool isFalling;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        arcShotStarted = false;
        isFalling = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MakePlayerAttack();
        MakePlayerShootArc();
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

    void MovePlayer()
    {
        if (playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.FirstAttack) ||
        playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.SecondAttack) ||
        playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.ThirdAttack))
        {
            playerRB.velocity = Vector3.zero;
            return;
        }


        float moveZ = isFalling ? 0 : Input.GetAxis(PlayerControlsManager.Vertical);
        if (moveZ > 0)
        {
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, true);
            playerRB.velocity = gameObject.transform.forward * moveZ * movementSpeed * Time.deltaTime;
        }
        else
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, false);

        float moveX = Input.GetAxis(PlayerControlsManager.Horizontal);
        playerRB.transform.Rotate(Vector3.up * moveX * rotationSpeed * Time.deltaTime);
    }

    void MakePlayerAttack()
    {
        if (isFalling)
            return;

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

    void MakePlayerShootArc()
    {
        if (Input.GetMouseButtonDown(1) &&
        !arcShotStarted)
        {
            playerAnimator.SetTrigger(PlayerControlsManager.FireParam);
            arcShotStarted = true;
        }
    }

    void MakePlayerJump()
    {
        if (Input.GetKeyDown(PlayerControlsManager.JumpKeyboard) && !isFalling)
        {
            playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerAnimator.SetTrigger(PlayerControlsManager.JumpParam);
        }
    }

    void MakePlayerFall()
    {
        float yVelocity = playerRB.velocity.y;
        if (yVelocity < -1 && !isFalling)
        {
            playerAnimator.Play(PlayerControlsManager.FallAnimation);
            playerAnimator.SetBool(PlayerControlsManager.FallParam, true);
            isFalling = true;
        }
    }

    void ShootArc()
    {
        GameObject arcAttackEffect = Instantiate(arcEffect, arcInstantionPosition.transform.position,
                gameObject.transform.rotation) as GameObject;

        Rigidbody arcRigidBody = arcAttackEffect.GetComponent<Rigidbody>();
        arcRigidBody.velocity = gameObject.transform.forward * arcAttackMovementSpeed * Time.deltaTime;
        arcShotStarted = false;
    }
}
