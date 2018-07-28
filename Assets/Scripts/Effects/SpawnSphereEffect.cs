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


    public void SpawnSphereEffectAtPoint(Vector3 position)
    {
        Instantiate(sphereEffect, position, sphereEffect.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(position, affectRadius);

        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (!rb || !rb.CompareTag(TagsManager.Enemy))
                continue;

            rb.AddExplosionForce(damagePower, position, affectRadius, 3f, ForceMode.Impulse);
        }

        Destroy(gameObject);
    }
}
