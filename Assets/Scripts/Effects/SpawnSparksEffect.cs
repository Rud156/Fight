using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSparksEffect : MonoBehaviour
{

    [Header("Sparks Effect")]
    public GameObject sparksEffect;

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        if (!other.gameObject.CompareTag(TagsManager.SwordContact))
            return;

        ContactPoint contactPoint = other.contacts[0];
        Vector3 position = contactPoint.point;

        Instantiate(sparksEffect, position, sparksEffect.transform.rotation);
    }
}
