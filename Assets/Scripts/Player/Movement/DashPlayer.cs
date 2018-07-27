using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animator))]
public class DashPlayer : MonoBehaviour
{

    [Header("Dash Requirements")]
    public float dashDistance = 10;
    public GameObject dashTrail;
    public GameObject dashBody;
    public GameObject dashSpawnPoint;

    private Animator playerAnimator;


    // Use this for initialization
    void Start()
    {
        playerAnimator = gameObject.GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(PlayerControlsManager.DashKeyboard) &&
            playerAnimator.GetBool(PlayerControlsManager.MoveParam))
            DashForward();
    }

    void DashForward()
    {
        RaycastHit hit;
        Vector3 destination = gameObject.transform.position +
            gameObject.transform.forward * dashDistance;

        // Obstacle is in Front
        if (Physics.Linecast(gameObject.transform.position, destination, out hit))
            destination = gameObject.transform.position +
                gameObject.transform.forward * (hit.distance - 1);

        gameObject.transform.position = destination;
        Instantiate(dashTrail, dashSpawnPoint.transform.position, gameObject.transform.rotation);

        Instantiate(dashBody,
            new Vector3(dashSpawnPoint.transform.position.x, dashSpawnPoint.transform.position.y - 0.8f,
                dashSpawnPoint.transform.position.z),
            dashBody.transform.rotation);
    }
}
