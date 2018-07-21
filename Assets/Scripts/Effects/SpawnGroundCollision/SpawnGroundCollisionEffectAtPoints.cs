using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundCollisionEffectAtPoints : MonoBehaviour
{
    [Header("General Stats")]
    public GameObject groundExplosionEffect;
    public float heightAboveGroundToSpawn = 0.2f;

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
        }
    }

}
