using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnGroundExplosionOnCollision : SpawnGroundCollisionEffectAtPoints
{
    [Header("Circular Ground Effect")]
    public int totalGroundEffectsToSpawn = 10;
    public float sizeDivisibilityFactor = 5;

    /// <summary>
    /// OnParticleCollision is called when a particle hits a collider.
    /// </summary>
    /// <param name="other">The GameObject hit by the particle.</param>
    void OnParticleCollision(GameObject other)
    {
        ParticleSystem particleSystem = other.GetComponent<ParticleSystem>();

        if (!particleSystem)
            return;

        if (other.CompareTag(TagsManager.CollisionSpawn))
            SpawnEffectAtCollisionPoints(particleSystem);
        else if (other.CompareTag(TagsManager.CircularSpawn))
            SpawnCircularGroundEffect(particleSystem);
    }

    private void SpawnEffectAtCollisionPoints(ParticleSystem particleSystem)
    {
        particleSystem.GetCollisionEvents(gameObject, collisionEvents);
        List<Vector3> collisionPoints = new List<Vector3>();

        foreach (var item in collisionEvents)
            collisionPoints.Add(item.intersection);

        SpawnEffectAtPoints(collisionPoints);
    }

    private void SpawnCircularGroundEffect(ParticleSystem particleSystem)
    {
        ParticleSystem.Particle[] particles = new ParticleSystem.Particle[1];
        particleSystem.GetParticles(particles);
        particleSystem.GetCollisionEvents(gameObject, collisionEvents);

        float currentRadius = particles[0].GetCurrentSize(particleSystem) / sizeDivisibilityFactor;
        Vector3 centerPoint = collisionEvents[0].intersection;

        float angleDiv = 360 / totalGroundEffectsToSpawn;
        List<Vector3> spawnPoints = new List<Vector3>();

        for (float i = 0; i <= 360; i += angleDiv)
        {
            float x = centerPoint.x + currentRadius * Mathf.Cos(i);
            float z = centerPoint.z + currentRadius * Mathf.Sin(i);
            spawnPoints.Add(new Vector3(x, heightAboveGroundToSpawn, z));
        }

        SpawnEffectAtPoints(spawnPoints);
    }
}
