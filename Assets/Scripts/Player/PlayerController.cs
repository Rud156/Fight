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

    private int upcomingAttack;
    private Rigidbody playerRB;
    private Animator playerAnimator;

    // Use this for initialization
    void Start()
    {
        upcomingAttack = 0;
        playerRB = gameObject.GetComponent<Rigidbody>();
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
        MakePlayerAttack();
    }

    void MovePlayer()
    {
        float moveZ = Input.GetAxis(PlayerControlsManager.Vertical);
        if (moveZ > 0)
        {
            playerAnimator.SetBool(PlayerControlsManager.Movement, true);
            playerRB.velocity = Vector3.forward * moveZ * movementSpeed * Time.deltaTime;
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
}
