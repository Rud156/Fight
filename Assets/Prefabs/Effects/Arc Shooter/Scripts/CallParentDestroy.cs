using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CallParentDestroy : MonoBehaviour
{
    /// <summary>
    /// OnTriggerEnter is called when the Collider other enters the trigger.
    /// </summary>
    /// <param name="other">The other Collider involved in this collision.</param>
    void OnTriggerEnter(Collider other)
    {
        Rigidbody target = other.GetComponent<Rigidbody>();
        if (!target || !other.CompareTag(TagsManager.Enemy))
            return;

        gameObject.transform.GetComponentInParent<SpawnSphereEffect>()
            .SpawnSphereEffectAtPoint(gameObject.transform.position);
    }
}
