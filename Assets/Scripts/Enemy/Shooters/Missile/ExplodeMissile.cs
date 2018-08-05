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
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        Instantiate(explosion, gameObject.transform.position, explosion.transform.rotation);
        Destroy(gameObject);
    }
}
