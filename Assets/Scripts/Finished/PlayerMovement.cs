using System.Collections;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float walkSpeed = 4f;
    public bool movementEnabled = true;
    public bool lockedForwardMode = false;

    private Vector3 lockedDirection;


    private CharacterController controller;

    public AutoPathFollower pathFollower;

    private bool isExiting = true;

    void Start()
    {
        controller = GetComponent<CharacterController>();

        if (controller == null)
        {
            Debug.LogError("PlayerMovement requires a CharacterController component.");
        }
    }

    public void toggleMoveLock(bool isLocked) { 
        lockedForwardMode = isLocked;
        if (isLocked)
        {
            lockedDirection = transform.forward.normalized;
            if (isExiting)
            {
                pathFollower.BeginExitPath();
                isExiting = false;
            }
            else { 
                pathFollower.BeginReturnPath();
                isExiting = true;
            }
        }
    }
    public void toggleMoveAbility(bool canMove) { 
        movementEnabled = canMove;
    }


    void Update()
    {

        if (!movementEnabled)
            return;

        float vertical = 0f;
        float horizontal = 0f;

        Vector3 move;

        if (lockedForwardMode)
        {
            vertical = Input.GetKey(KeyCode.W) ? 1f : 0f;
            move = lockedDirection * vertical;
        }
        else
        {
            vertical = Input.GetKey(KeyCode.S) ? -1f : Input.GetKey(KeyCode.W) ? 1f : 0f;
            horizontal = Input.GetKey(KeyCode.A) ? -1f : Input.GetKey(KeyCode.D) ? 1f : 0f;
            move = (transform.forward * vertical + transform.right * horizontal).normalized;
        }

        
        controller.Move(move * walkSpeed * Time.deltaTime);
        if (lockedForwardMode)
        {
            Debug.Log("LOCKED MODE ACTIVE — move = " + move);
        }

    }
}
