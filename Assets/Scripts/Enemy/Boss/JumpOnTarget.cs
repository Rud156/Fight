﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class JumpOnTarget : MonoBehaviour
{

    [Header("Target to Target")]
    public GameObject target;

    [Header("Jump Fall Effect")]
    public GameObject landEffect;
    public float heightAboveGroundToSpawn = 0.2f;

    [Header("Land Force Stats")]
    public float affectRadius = 5f;
    public float damagePower = 10f;

    [Header("Player Damage Stats")]
    public float damageAmount = 50;

    [Header("Jump Stats")]
    public float minLaunchAngle = 30f;
    public float maxLaunchAngle = 60f;

    private Rigidbody gameObjectRigidbody;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        gameObjectRigidbody = gameObject.GetComponent<Rigidbody>();
    }

    public void LandOnGround()
    {
        Vector3 explosionPoint = gameObject.transform.position;
        Collider[] colliders = Physics.OverlapSphere(explosionPoint, affectRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (!rb || !collider.CompareTag(TagsManager.Player))
                continue;

            collider.GetComponent<GetHitFromEnemy>().GetExternalDamage(damageAmount);
            rb.AddExplosionForce(damagePower, explosionPoint, affectRadius, 3f, ForceMode.Impulse);
        }

        Instantiate(landEffect,
            new Vector3(explosionPoint.x, heightAboveGroundToSpawn, explosionPoint.z),
            landEffect.transform.rotation);

        gameObjectRigidbody.isKinematic = true;
    }

    public void TriggerJump()
    {
        gameObjectRigidbody.isKinematic = false;

        float randomAngle = Random.Range(minLaunchAngle, maxLaunchAngle);
        gameObjectRigidbody.velocity = BallisticVelocity(randomAngle);
    }

    private Vector3 BallisticVelocity(float launchAngle)
    {
        Vector3 dir = target.transform.position - gameObject.transform.position;  // get target direction
        float h = dir.y;  // get height difference
        dir.y = 0;  // retain only the horizontal direction
        float dist = dir.magnitude;  // get horizontal distance
        float a = launchAngle * Mathf.Deg2Rad;  // convert angle to radians
        dir.y = dist * Mathf.Tan(a);  // set dir to the elevation angle
        dist += h / Mathf.Tan(a);  // correct for small height differences

        // Calculate the velocity magnitude
        float vel = Mathf.Sqrt(dist * Physics.gravity.magnitude / Mathf.Sin(2 * a));
        return vel * dir.normalized;
    }
}
