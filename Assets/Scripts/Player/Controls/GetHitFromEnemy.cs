using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitFromEnemy : MonoBehaviour
{

    private Animator playerAnimator;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag(TagsManager.HandContact))
            playerAnimator.SetTrigger(PlayerControlsManager.HitParam);
    }
}
