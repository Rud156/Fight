using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundExplosionOnCollision : SpawnGroundCollisionEffectAtPoints
{
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
        List<Vector3> collisionPoints = new List<Vector3>();

        foreach (var item in collisionEvents)
            collisionPoints.Add(item.intersection);

        SpawnEffectAtPoints(collisionPoints);
    }
}
