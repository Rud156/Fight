using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class SimplePlayerController : MonoBehaviour
{
    [Header("Player Control Stats")]
    public float movementSpeed;
    public float rotationSpeed;
    public float jumpSpeed;

    private Rigidbody playerRB;
    private Animator playerAnimator;
    private bool isJumping;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
        isJumping = false;
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
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
        playerAnimator.SetBool(PlayerControlsManager.FallParam, false);
        isJumping = false;
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


        float moveZ = isJumping ? 0 : Input.GetAxis(PlayerControlsManager.Vertical);
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

    void MakePlayerJump()
    {
        if (Input.GetKeyDown(PlayerControlsManager.JumpKeyboard) && !isJumping)
        {
            playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerAnimator.SetTrigger(PlayerControlsManager.JumpParam);
            isJumping = true;
        }
    }

    void MakePlayerFall()
    {
        float yVelocity = playerRB.velocity.y;
        print(yVelocity);
        if (yVelocity < -1 && !isJumping)
        {
            playerAnimator.Play(PlayerControlsManager.FallAnimation);
            playerAnimator.SetBool(PlayerControlsManager.FallParam, true);
            isJumping = true;
        }
    }
}
