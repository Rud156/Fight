using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnCircularGroundCollisionEffect : SpawnGroundCollisionEffectAtPoints
{
    public int totalGroundEffectsToSpawn = 30;

    private ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider.
    /// </summary>
    /// <param name="other">The GameObject hit by the particle.</param>
    void OnParticleCollision(GameObject other)
    {
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

        if (!particleSystem)
            return;

        particleSystem.GetParticles(particles);
        particleSystem.GetCollisionEvents(gameObject, collisionEvents);

        float currentRadius = particles[0].GetCurrentSize(particleSystem) / 4;
        Vector3 centerPoint = collisionEvents[0].intersection;

        float angleDiv = 360 / totalGroundEffectsToSpawn;

        for (float i = 0; i <= 360; i += angleDiv)
        {
            float x = centerPoint.x + currentRadius * Mathf.Cos(i);
            float z = centerPoint.z + currentRadius * Mathf.Sin(i);
            Instantiate(groundExplosionEffect, new Vector3(x, heightAboveGroundToSpawn, z),
                groundExplosionEffect.transform.rotation);
        }
    }
}
