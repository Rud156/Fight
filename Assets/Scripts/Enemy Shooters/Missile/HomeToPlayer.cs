using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HomeToPlayer : MonoBehaviour
{
    [Header("Missile Stats")]
    public float homingSensitivity;
    public float speed;

    private Transform player;

    // Use this for initialization
    void Start()
    {
        player = GameObject.FindGameObjectWithTag(TagsManager.Player).transform;
    }

    /// <summary>
    /// This function is called every fixed framerate frame, if the MonoBehaviour is enabled.
    /// </summary>
    void FixedUpdate()
    {
        if (player == null)
            return;

        Vector3 relativePos = player.position - gameObject.transform.position;
        Quaternion targetRotation = Quaternion.LookRotation(relativePos);
        gameObject.transform.rotation = Quaternion.Slerp(gameObject.transform.rotation,
            targetRotation, homingSensitivity);
        gameObject.transform.Translate(Vector3.forward * speed * Time.deltaTime);
    }
}
