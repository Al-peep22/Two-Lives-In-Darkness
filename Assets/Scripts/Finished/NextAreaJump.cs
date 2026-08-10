using UnityEngine;

//check if player is in collider when triggered then check if player has pressed F and then move player into the next room or area
public class NextAreaJump : MonoBehaviour
{
    public bool oneDirection = false;
    public string cutSceneAfter = "";

    public RoomSwap targetRoom;
    public RoomSwap previousRoom;

    public Transform moveToPoint;

    private KeyCode interactKey = KeyCode.F;

    private bool playerInside = false;
    private Transform player;

    private bool keyReady = true;
    private PlayerMovement playerMovement;


    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = true;
            player = other.transform;

            playerMovement = other.GetComponent<PlayerMovement>();
        }
    }


    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInside = false;
            player = null;
        }
    }


    private void Update()
    {
        if (playerInside && keyReady && Input.GetKeyDown(interactKey))
        {
            Debug.Log("[NextAreaJump] Interact key pressed, moving player. keyReady = false");
            keyReady = false;
            MovePlayer();
        }

        if (Input.GetKeyUp(interactKey))
        {
            Debug.Log("[NextAreaJump] Interact key released, keyReady = true.");
            keyReady = true;
        }

        if (cutSceneAfter != "") { 
            StoryManager.instance.HandlePlayerChoice(cutSceneAfter);
        }
    }

    private void MovePlayer()
    {
        if (player != null && moveToPoint != null)
        {

            CharacterController cc = player.GetComponent<CharacterController>();

            if (cc != null)
                cc.enabled = false;

            player.position = moveToPoint.position;
            player.rotation = moveToPoint.rotation;

            if (oneDirection && playerMovement != null)
            {
                Debug.Log("Locked Movement");
                playerMovement.toggleMoveLock(true);
            }
            else if (playerMovement != null){ 
                playerMovement.toggleMoveLock(false);
            }

            if (cc != null)
                cc.enabled = true;

            if (previousRoom != null)
                previousRoom.DeactivateRoom();

            if (targetRoom != null)
                targetRoom.ActivateRoom();
        }
    }



}
