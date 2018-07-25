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

    private bool isJumping = false;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        arcShotStarted = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MakePlayerAttack();
        MakePlayerShootArc();
        // MakePlayerJump();
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        MakePlayerFall();
    }

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!isJumping)
            return;

        print("On Collision Entered");
        playerAnimator.SetBool(PlayerControlsManager.Fall, false);
        isJumping = false;
    }

    void MovePlayer()
    {
        if (!playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.Idle) &&
            !playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.Run))
        {
            playerRB.velocity = Vector3.zero;
            return;
        }


        float moveZ = isJumping ? 0 : Input.GetAxis(PlayerControlsManager.Vertical);
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
        if (isJumping)
            return;

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
        !arcShotStarted)
        {
            playerAnimator.SetTrigger(PlayerControlsManager.Fire);
            arcShotStarted = true;
        }
    }

    void MakePlayerJump()
    {
        if (Input.GetKeyDown(PlayerControlsManager.JumpControl) && !isJumping)
        {
            playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerAnimator.SetTrigger(PlayerControlsManager.Jump);
            isJumping = true;
        }
    }

    void MakePlayerFall()
    {
        float yVelocity = playerRB.velocity.y;
        print(yVelocity);
        if (yVelocity < 0 && !isJumping)
        {
            playerAnimator.Play(PlayerControlsManager.FallAnimation);
            playerAnimator.SetBool(PlayerControlsManager.Fall, true);
            isJumping = true;
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
