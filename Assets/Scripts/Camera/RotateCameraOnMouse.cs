using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCameraOnMouse : MonoBehaviour
{

    [Header("Camera Stats")]
    public float upDownSpeed;
    public int minAngle = 30;
    public int maxAngle = -40;

    // Update is called once per frame
    void Update()
    {
        float mouseY = Input.GetAxis(PlayerControlsManager.MouseY);
        float yRotation = mouseY * upDownSpeed * Time.deltaTime;

        gameObject.transform.Rotate(Vector3.left * yRotation);
    }

    /// <summary>
    /// LateUpdate is called every frame, if the Behaviour is enabled.
    /// It is called after all Update functions have been called.
    /// </summary>
    void LateUpdate()
    {
        float xRotation = gameObject.transform.localRotation.eulerAngles.x;
        float normalizedXRotation = xRotation;
        if (xRotation < 0)
            normalizedXRotation += 360;
        else if (xRotation > 360)
            normalizedXRotation -= 360;

        if (normalizedXRotation > minAngle && normalizedXRotation < maxAngle)
        {
            float minDiff = Mathf.Abs(normalizedXRotation - minAngle);
            float maxDiff = Mathf.Abs(normalizedXRotation - maxAngle);
            if (minDiff < maxDiff)
                normalizedXRotation = minAngle;
            else
                normalizedXRotation = maxAngle;
        }

        gameObject.transform.localRotation = Quaternion.Euler(normalizedXRotation, 0, 0);
    }
}
