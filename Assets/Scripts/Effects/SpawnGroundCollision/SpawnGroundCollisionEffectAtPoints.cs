using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundCollisionEffectAtPoints : MonoBehaviour
{
    [Header("General Stats")]
    public GameObject groundExplosionEffect;
    public float heightAboveGroundToSpawn = 0.2f;


    [Header("Explosion Stats")]
    public float affectRadius = 5f;
    public float damagePower = 10f;

    [Header("Player Damage Stats")]
    public float damageAmount = 20;

    protected List<ParticleCollisionEvent> collisionEvents;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    public void SpawnEffectAtPoints(List<Vector3> positions)
    {
        foreach (var position in positions)
        {
            Instantiate(groundExplosionEffect,
                new Vector3(position.x, heightAboveGroundToSpawn, position.z),
                groundExplosionEffect.transform.rotation);

            Collider[] colliders = Physics.OverlapSphere(position, affectRadius);
            foreach (Collider collider in colliders)
            {
                Rigidbody rb = collider.GetComponent<Rigidbody>();
                if (!rb || (!rb.CompareTag(TagsManager.Player) && !rb.CompareTag(TagsManager.Enemy)))
                    continue;

                if (rb.CompareTag(TagsManager.Player))
                    collider.GetComponent<GetHitFromEnemy>().GetExternalDamage(damageAmount);
                rb.AddExplosionForce(damagePower, position, affectRadius, 3f, ForceMode.Impulse);
            }
        }
    }

}
