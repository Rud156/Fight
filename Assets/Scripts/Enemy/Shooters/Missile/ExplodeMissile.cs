using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMissile : MonoBehaviour
{

    [Header("Explosion Effect")]
    public GameObject explosion;

    [Header("Missile Damage")]
    public float missileDamage;

    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Instantiate(explosion, gameObject.transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
