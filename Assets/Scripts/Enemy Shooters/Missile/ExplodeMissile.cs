using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeMissile : MonoBehaviour
{

    [Header("Explosion Effect")]
    public GameObject explosion;

    [Header("Damage Stats")]
    public float damageRadius;
    public float damagePower;

    /// <summary>
    /// OnCollisionEnter is called when this collider/rigidbody has begun
    /// touching another rigidbody/collider.
    /// </summary>
    /// <param name="other">The Collision data associated with this collision.</param>
    void OnCollisionEnter(Collision other)
    {
        Instantiate(explosion, gameObject.transform.position, explosion.transform.rotation);

        Collider[] colliders = Physics.OverlapSphere(gameObject.transform.position, damageRadius);
        foreach (Collider collider in colliders)
        {
            Rigidbody rb = collider.GetComponent<Rigidbody>();
            if (rb != null)
                rb.AddExplosionForce(damagePower, gameObject.transform.position, damageRadius, 3,
                    ForceMode.Impulse);
        }
    }
}
