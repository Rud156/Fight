using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{

    [Header("Spawner Data")]
    public BoxCollider enclosingBoxCollider;
    public GameObject minion;
    public float initialSpawnTime = 5f;

    [Header("Player")]
    public GameObject player;
    public float distanceFromPlayer = 30f;

    [Header("Debug")]
    public bool runAtStart = false;

    private float currentSpawnTime;
    private Coroutine coroutine;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        currentSpawnTime = initialSpawnTime;
        if (runAtStart)
            StartSpawn();
    }

    public void StartSpawn()
    {
        coroutine = StartCoroutine(SpawnMinions());

    }

    public void StopSpawn()
    {
        currentSpawnTime -= 2;
        StopCoroutine(coroutine);
    }

    IEnumerator SpawnMinions()
    {
        while (true)
        {
            Vector3 randomPoint = new Vector3(
                Random.Range(-enclosingBoxCollider.bounds.extents.x, enclosingBoxCollider.bounds.extents.x),
                0,
                Random.Range(-enclosingBoxCollider.bounds.extents.z, enclosingBoxCollider.bounds.extents.z)
            ) + enclosingBoxCollider.bounds.center;

            float generatedDistanceFromPlayer = Vector3.Distance(randomPoint,
                new Vector3(player.transform.position.x, 0, player.transform.position.z));

            if (distanceFromPlayer > generatedDistanceFromPlayer)
            {
                yield return null;
                continue;
            }

            Instantiate(minion, randomPoint, minion.transform.rotation);
            yield return new WaitForSeconds(initialSpawnTime);
        }
    }
}
