using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagePlayerOnHandContact : MonoBehaviour
{

    public float damageAmount = 10;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Rigidbody playerRb = other.GetComponent<Rigidbody>();
        if (!playerRb || !other.CompareTag(TagsManager.Player))
            return;

        PlayerData.currentHealthLeft -= damageAmount;
    }

}
