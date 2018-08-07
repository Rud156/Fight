using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetHitFromEnemy : MonoBehaviour
{
    [Header("Audio")]
    public AudioSource hitAudioSource;
    public AudioClip hitAudio;

    private Animator playerAnimator;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
        hitAudioSource.clip = hitAudio;
    }

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag(TagsManager.HandContact))
        {
            playerAnimator.SetTrigger(PlayerControlsManager.HitParam);
            PlayerData.currentHealthLeft -= other.GetComponent<DamagePlayerOnHandContact>().damageAmount;

            hitAudioSource.Play();
        }

        if (other.CompareTag(TagsManager.Missile))
        {
            playerAnimator.SetTrigger(PlayerControlsManager.HitParam);
            PlayerData.currentHealthLeft -= other.GetComponent<ExplodeMissile>().missileDamage;

            hitAudioSource.Play();
        }
    }

    public void GetExternalDamage(float damageAmount)
    {
        playerAnimator.SetTrigger(PlayerControlsManager.HitParam);
        PlayerData.currentHealthLeft -= damageAmount;

        hitAudioSource.Play();
    }
}
