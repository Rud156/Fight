using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(Animator))]
public class PlayerController : MonoBehaviour
{
    private int currentAttackState;
    private Rigidbody playerRB;

    // Use this for initialization
    void Start()
    {
        playerRB = gameObject.GetComponent<Rigidbody>();
        currentAttackState = 0;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
