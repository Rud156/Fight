using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSphereEffect : MonoBehaviour
{
    [Header("Sphere Effect")]
    public GameObject sphereEffect;

    [Header("Effect Stats")]
    public float affectRadius = 5f;
    public float damagePower = 10f;

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Rigidbody target = other.GetComponent<Rigidbody>();

        if (!target || !other.CompareTag(TagsManager.Enemy))
            return;

        Vector3 position = gameObject.transform.position;

        Instantiate(sphereEffect, position, sphereEffect.transform.rotation);
        Collider[] colliders = Physics.OverlapSphere(position, affectRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (!rb || !rb.CompareTag(TagsManager.Enemy))
                return;

            rb.AddExplosionForce(damagePower, position, affectRadius, 3f, ForceMode.Impulse);
        }
        Destroy(gameObject);
    }
}
