using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    [Header("Camera Settings")]
    public float mouseSensitivity = 150f;
    public Transform playerBody;

    [Header("Limited Look Mode")]
    public bool limitedLookMode = false;
    public float maxYawAngle = 30f; // left/right head turn limit
    public float maxVerticalAngle = 40f; // up/down limit for locked mode

    private float xRotation = 0f;
    private float yawOffset = 0f;

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // Always clamp vertical rotation so camera NEVER flips
        xRotation -= mouseY;

        if (!limitedLookMode)
        {
            // Normal full vertical range (but still safe)
            xRotation = Mathf.Clamp(xRotation, -80f, 80f);

            // Camera pitch
            transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

            // Player rotates fully
            playerBody.Rotate(Vector3.up * mouseX);
        }
        else
        {
            // Limited vertical range (head tilt)
            xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

            // Limited horizontal head turn
            yawOffset += mouseX;
            yawOffset = Mathf.Clamp(yawOffset, -maxYawAngle, maxYawAngle);

            // Apply limited rotation
            transform.localRotation = Quaternion.Euler(xRotation, yawOffset, 0f);
        }
    }
}
