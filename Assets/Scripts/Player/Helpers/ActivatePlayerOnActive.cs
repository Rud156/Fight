using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivatePlayerOnActive : MonoBehaviour
{
    public PlayerController controller;

    // Use this for initialization
    void Start()
    {
        controller.ActivatePlayerMovement();
    }
}
