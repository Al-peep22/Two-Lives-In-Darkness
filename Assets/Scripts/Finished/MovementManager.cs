using UnityEngine;

public class MovementManager : MonoBehaviour
{
    public PlayerMovement movement;
    public PlayerCamera cameraLook;

    public void StartCinematic()
    {
        movement.movementEnabled = false;
        cameraLook.limitedLookMode = false;
    }

    public void EndCinematic()
    {
        movement.movementEnabled = true;
    }

    public void EnableLockedForwardMode()
    {
        movement.lockedForwardMode = true;
        cameraLook.limitedLookMode = true;
    }

    public void DisableLockedForwardMode()
    {
        movement.lockedForwardMode = false;
        cameraLook.limitedLookMode = false;
    }
}
