using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(JumpOnTarget))]
public class SimplePlayerController : MonoBehaviour
{

    private JumpOnTarget jumpOnTarget;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        jumpOnTarget = gameObject.GetComponent<JumpOnTarget>();
    }

    /// <summary>
    /// Update is called every frame, if the MonoBehaviour is enabled.
    /// </summary>
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            jumpOnTarget.TriggerJump();
        }
    }
}
