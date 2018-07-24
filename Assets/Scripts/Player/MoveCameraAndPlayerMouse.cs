using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveCameraAndPlayerMouse : MonoBehaviour
{
    public float horizontalRotationSpeed = 1000;

    // Update is called once per frame
    void Update()
    {
        RotatePlayerAndMoveCamera();
    }

    void RotatePlayerAndMoveCamera()
    {
        float mouseX = Input.GetAxis(PlayerControlsManager.MouseX);

        gameObject.transform.Rotate(Vector3.up * mouseX * horizontalRotationSpeed * Time.deltaTime);
    }
}
