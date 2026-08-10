using UnityEngine;
using System.Collections.Generic;

//This is a script where it senses when the player walking into a room and turns of all the furniture in other rooms so you can't see furniture through walls.
//Each room will have a trigger collider that then turns off the prior rooms and on the current room
public class RoomSwap : MonoBehaviour
{
    [Header("Objects belonging to this room")]
    public List<GameObject> roomObjects = new List<GameObject>();

    private void Start()
    {
        SetObjectsActive(false);
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetObjectsActive(true);
        }
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetObjectsActive(true);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            SetObjectsActive(false);
        }
    }

    private void SetObjectsActive(bool state)
    {
        foreach (GameObject obj in roomObjects)
        {
            if (obj == null)
            {
                Debug.LogWarning("Found null object");
                continue;
            }

            // Only change state if different
            if (obj.activeSelf != state)
                obj.SetActive(state);
        }
    }

    public void ActivateRoom()
    {
        SetObjectsActive(true);
    }

    public void DeactivateRoom()
    {
        SetObjectsActive(false);
    }


}