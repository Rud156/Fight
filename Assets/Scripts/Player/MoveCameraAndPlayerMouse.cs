using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraAndPlayerMouse : MonoBehaviour
{

    [Header("Camera Stats")]
    public GameObject mainCamera;
    public float horizontalRotationSpeed = 100;
    public float verticalRotationSpeed = 10;

    private float maxLookAngle = 60f;
    private float minLookAngle = 30f;

    /// <summary>
    /// Start is called on the frame when a script is enabled just before
    /// any of the Update methods is called the first time.
    /// </summary>
    void Start()
    {
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        RotatePlayerAndMoveCamera();
    }

    void RotatePlayerAndMoveCamera()
    {
        float horizontal = Input.GetAxis(PlayerControlsManager.MouseX) * horizontalRotationSpeed
            * Time.deltaTime;

        float vertical = Input.GetAxis(PlayerControlsManager.MouseY) * verticalRotationSpeed
            * Time.deltaTime;


    }
}
