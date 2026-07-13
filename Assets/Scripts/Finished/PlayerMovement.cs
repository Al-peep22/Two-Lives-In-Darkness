using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public bool movementEnabled = true;
    public bool lockedForwardMode = false;

    private CharacterController controller;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController component.");
        }
    }


    void Update()
    {
        if (!movementEnabled)
            return;

        float vertical = 0f;
        float horizontal = 0f;

        if (lockedForwardMode)
        {
            vertical = Input.GetKey(KeyCode.W) ? 1f : 0f;
            horizontal = 0f;
        }
        else
        {
            vertical = Input.GetAxisRaw("Vertical");
            horizontal = Input.GetAxisRaw("Horizontal");
        }

        Vector3 move = (transform.forward * vertical + transform.right * horizontal).normalized;
        controller.Move(move * walkSpeed * Time.deltaTime);
    }
}
