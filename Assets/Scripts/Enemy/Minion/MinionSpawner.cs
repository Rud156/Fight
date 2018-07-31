using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinionSpawner : MonoBehaviour
{

    public BoxCollider enclosingBoxCollider;
    public GameObject minion;
    public float spawnTime = 7f;

    private Coroutine coroutine;

    // Use this for initialization
    void Start()
    {
        StartSpawn();
    }

    public void StartSpawn()
    {
        coroutine = StartCoroutine(SpawnMinions());

    }

    public void StopSpawn()
    {

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

            Instantiate(minion, randomPoint, minion.transform.rotation);
            yield return new WaitForSeconds(spawnTime);
        }
    }
}
