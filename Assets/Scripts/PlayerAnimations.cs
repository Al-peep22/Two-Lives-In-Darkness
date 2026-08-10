using UnityEngine;
using System.Collections;

//Moves the player in a way to replicate a movement of a character in a story.
//For example, if the player chooses to "Sit on Bed", the player will move to the bed and sit down.
//If the player chooses to "Lay in Bed", the player will move to the bed and lay down.
//If the player chooses to "Stand Up", the player will stand up from the bed.
public class PlayerAnimations : MonoBehaviour
{
    [Header("References")]
    public Animator animator;
    public GameObject player;
    private Transform PlayerTransform;
    private CharacterController PlayerController;
    public PlayerMovement movement;
    public PlayerCamera cam;

    [Header("Targets")]
    public SFXClip bedMovement;

    [Header("Targets")]
    public Transform bedActionPoint;
    public Transform radioActionPoint;

    public static PlayerAnimations Instance;

    private bool isDayOne = true;

    private bool cutSceneInProcess = false;

    private void Awake()
    {
        Instance = this;
        if (player != null) {
            PlayerTransform = player.transform;
            PlayerController = player.gameObject.GetComponentInChildren<CharacterController>();
        }
    }

    private void MovePlayer(Transform theSpot) {
        PlayerController.enabled = false;
        // Move
        PlayerTransform.position = theSpot.position;
        // Rotate
        PlayerTransform.rotation = theSpot.rotation;
        PlayerController.enabled = true;
    }
    public void WakeUp()
    {
        animator.enabled = true;
        MovePlayer(bedActionPoint);
        movement.movementEnabled = false;
        cam.enableMovement = false;
        cutSceneInProcess = true;
        switchOptionsActive(false);

        animator.SetBool("isWaken", true);
        StartCoroutine(WakeUpRoutine());
    }

    public void switchOptionsActive(bool isOn) {
        if (((StoryManager.instance.getProgress() || cutSceneInProcess) && !isOn) || (((!StoryManager.instance.getProgress() && !cutSceneInProcess) && isOn)))
        {
            UIManager.Instance.gamePanel.transform.Find("OptionUI").gameObject.SetActive(isOn);
        }
    }

    private IEnumerator WakeUpRoutine()
    {        
        // Wait until animator enters the wake-up state
        yield return new WaitUntil(() =>
            animator.GetCurrentAnimatorStateInfo(0).IsName("WakeUp")
        );

        AudioSource bedAudio = SFXManager.instance.PlaySFXClip(bedMovement, transform, 1f);
        bedAudio.loop = true;
        bedAudio.Play();
        Debug.Log("Audio Playing");

        float animationLength = animator.GetCurrentAnimatorStateInfo(0).length;
        float timer = 0f;

        float pauseDelay = 0.5f;      // wait 0.2s before pausing
        float movementTimer = 0f;     // counts time since last movement

        while (timer < animationLength)
        {
            timer += Time.deltaTime;

            if (cam.cameraActivity > 0.01f)
            {
                movementTimer = 0f; // reset timer because camera is moving

                if (!bedAudio.isPlaying)
                {
                    bedAudio.UnPause();
                    Debug.Log("Audio unpaused");
                }
            }
            else
            {
                movementTimer += Time.deltaTime;

                // Only pause if camera has been still long enough
                if (movementTimer > pauseDelay && bedAudio.isPlaying)
                {
                    bedAudio.Pause();
                    Debug.Log("Audio paused");
                }
            }

            yield return null;
        }

        // End of animation
        bedAudio.Stop();

        animator.SetBool("isWaken", false);
        movement.movementEnabled = true;
        cam.enableMovement = true;
        animator.enabled = false;

        if (isDayOne)
        {
            isDayOne = false;
            UIManager.Instance.AddTask("Feed Kesi");
        }
        cutSceneInProcess = false;
        switchOptionsActive(true);
    }

    public IEnumerator SittingDownCafe() {
        yield return new WaitForSeconds(2f);
    }
}