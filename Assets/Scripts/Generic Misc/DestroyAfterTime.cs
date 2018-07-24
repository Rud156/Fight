using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyAfterTime : MonoBehaviour
{

    public float destroyAfterTime = 3f;

    // Use this for initialization
    void Start()
    {
        Destroy(gameObject, destroyAfterTime);
    }
}
