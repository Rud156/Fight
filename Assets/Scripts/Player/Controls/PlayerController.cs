using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    [Header("Player Control Stats")]
    public float movementSpeed;
    public float jumpSpeed;
    public float fallThreshold = -1.5f;
    public float movementThreshold = 1f;

    [Header("Sword and Foot Colliders")]
    public GameObject swordContact;
    public GameObject footContact;

    [Header("After Player Dead")]
    public GameObject bossEnemy;
    public GameObject minionHolder;
    public Animator textAnimator;
    public Text majorText;

    private Rigidbody playerRB;
    private Animator playerAnimator;

    private bool isFalling;
    private bool jumped;
    private bool jumpedFromZeroVelocity;

    private bool forwardMovementKeyRemoved;
    private bool disablePlayerMovement;
    private bool playerDeadInvoked;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();

        isFalling = false;
        jumped = false;

        forwardMovementKeyRemoved = false;
        disablePlayerMovement = true;
        playerDeadInvoked = false;

        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        if (disablePlayerMovement)
            return;

        CheckAndKillPlayer();

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
        jumpedFromZeroVelocity = false;
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
            playerAnimator.GetCurrentAnimatorStateInfo(0).IsName(PlayerControlsManager.ThirdAttack) ||
            jumpedFromZeroVelocity)
        {
            float yVelocity = playerRB.velocity.y;
            playerRB.velocity = new Vector3(0, yVelocity, 0);
            return;
        }

        float moveZ = Input.GetAxis(PlayerControlsManager.Vertical);

        if (moveZ > 0)
        {
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, true);

            Vector3 velocity = gameObject.transform.forward * moveZ * movementSpeed * Time.deltaTime;
            playerRB.velocity = new Vector3(velocity.x, playerRB.velocity.y, velocity.z);

            forwardMovementKeyRemoved = false;
        }
        else
            playerAnimator.SetBool(PlayerControlsManager.MoveParam, false);

        if (moveZ == 0 && !forwardMovementKeyRemoved)
        {
            float yVelocity = playerRB.velocity.y;
            playerRB.velocity = new Vector3(0, yVelocity, 0);

            forwardMovementKeyRemoved = true;
        }

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
            Vector3 velocity = playerRB.velocity;

            if (Mathf.Abs(velocity.z) <= movementThreshold)
                jumpedFromZeroVelocity = true;
            else
                jumpedFromZeroVelocity = false;

            playerRB.AddForce(Vector3.up * jumpSpeed, ForceMode.Impulse);
            playerAnimator.SetTrigger(PlayerControlsManager.JumpParam);
            jumped = true;
        }
    }

    void MakePlayerFall()
    {
        float yVelocity = playerRB.velocity.y;
        if (yVelocity < fallThreshold && !isFalling)
        {
            playerAnimator.SetBool(PlayerControlsManager.FallParam, true);
            isFalling = true;
            jumped = false;
        }
    }

    public void ActivatePlayerMovement()
    {
        disablePlayerMovement = false;
    }

    void CheckAndKillPlayer()
    {
        if (PlayerData.currentHealthLeft <= 0 && !playerDeadInvoked)
        {
            Destroy(bossEnemy);
            Destroy(minionHolder);

            majorText.text = "You are dead !!!";
            majorText.color = Color.red;
            textAnimator.Play("TextZoomIn");

            disablePlayerMovement = true;
            playerDeadInvoked = true;
            playerAnimator.SetBool(PlayerControlsManager.DeadParam, true);

            Invoke("LoadMainScene", 1.3f);
        }
    }

    void LoadMainScene()
    {
        SceneManager.LoadScene(0);
    }
}
