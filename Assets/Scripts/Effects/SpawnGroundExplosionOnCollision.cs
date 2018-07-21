using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundExplosionOnCollision : MonoBehaviour
{

    public GameObject groundExplosionEffect;

    private List<ParticleCollisionEvent> collisionEvents;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        collisionEvents = new List<ParticleCollisionEvent>();
    }

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider.
    /// </summary>
    /// <param name="other">The GameObject hit by the particle.</param>
    void OnParticleCollision(GameObject other)
    {
        ParticleSystem particles = other.GetComponent<ParticleSystem>();

        if (!particles)
            return;

        particles.GetCollisionEvents(gameObject, collisionEvents);

        foreach (var item in collisionEvents)
        {
            Vector3 position = item.intersection;
            Instantiate(groundExplosionEffect,
                new Vector3(position.x, 0.2f, position.z), groundExplosionEffect.transform.rotation);
        }
    }
}
