using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public static PlayerCamera instance;
    [Header("Camera Settings")]
    public float mouseSensitivity = 150f;
    public Transform playerBody;

    [Header("Limited Look Mode")]
    public bool limitedLookMode = false;
    public float maxYawAngle = 30f; // left/right head turn limit
    public float maxVerticalAngle = 40f; // up/down limit for locked mode

    [Header("Movement Lock")]
    public bool enableMovement = true;

    private float xRotation = 0f;
    private float yawOffset = 0f;

    public Transform playerBodyRoot; // assign the player root transform
    private Vector3 lastPlayerPos;
    private Quaternion lastPlayerRot;

    private Vector3 lastPosition;
    private Quaternion lastRotation;
    public float cameraActivity;

    private void Awake()
    {
        instance = this;
    }
    void Update()
    {
        Vector3 camPosDelta = transform.position - lastPosition;
        float camRotDelta = Quaternion.Angle(transform.rotation, lastRotation);

        Vector3 playerPosDelta = playerBodyRoot.position - lastPlayerPos;
        float playerRotDelta = Quaternion.Angle(playerBodyRoot.rotation, lastPlayerRot);

        // Combine both
        cameraActivity = camPosDelta.magnitude + camRotDelta + playerPosDelta.magnitude + playerRotDelta;

        lastPosition = transform.position;
        lastRotation = transform.rotation;

        lastPlayerPos = playerBodyRoot.position;
        lastPlayerRot = playerBodyRoot.rotation;


        // If movement is disabled, camera cannot rotate at all
        if (!enableMovement)
            return;

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
