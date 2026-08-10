using UnityEngine;

public class AutoPathFollower : MonoBehaviour
{
    public Transform[] exitPathPoints;
    public Transform[] returnPathPoints;
    public float turnSpeed = 5f;

    private int currentPoint = 0;
    private bool isFollowingExit = false;
    private bool isFollowingReturn = false;

    private PlayerMovement playerMovement;
    private CharacterController cc;

    private void Start()
    {
        playerMovement = GetComponent<PlayerMovement>();
        cc = GetComponent<CharacterController>();
    }

    private void Update()
    {
        if (isFollowingExit)
        {
            FollowPath(exitPathPoints, ref isFollowingExit);
        }
        else if (isFollowingReturn)
        {
            FollowPath(returnPathPoints, ref isFollowingReturn);
        }
    }

    public void BeginExitPath()
    {
        if (playerMovement != null)
        {
            playerMovement.toggleMoveAbility(false);
            playerMovement.toggleMoveLock(true);
        }

        currentPoint = 0;
        isFollowingExit = true;
        isFollowingReturn = false;
    }

    public void BeginReturnPath()
    {
        if (playerMovement != null)
        {
            playerMovement.toggleMoveAbility(false);
            playerMovement.toggleMoveLock(true);
        }

        currentPoint = 0;
        isFollowingReturn = true;
        isFollowingExit = false;
    }

    private void FollowPath(Transform[] path,ref bool followFlag)
    {
        if (path == null || path.Length == 0)
            return;

        Transform target = path[currentPoint];

        // Smooth rotation
        Vector3 direction = (target.position - transform.position).normalized;
        Quaternion targetRot = Quaternion.LookRotation(direction);
        transform.rotation = Quaternion.Lerp(transform.rotation, targetRot, turnSpeed * Time.deltaTime);

        // Move forward
        float speed = playerMovement.walkSpeed;
        cc.Move(transform.forward * speed * Time.deltaTime);

        // Check if reached point
        if (Vector3.Distance(transform.position, target.position) < 0.5f)
        {
            currentPoint++;

            if (currentPoint >= path.Length)
            {
                // Finished path
                followFlag = false;

                if (playerMovement != null)
                {
                    playerMovement.toggleMoveAbility(false);
                    playerMovement.toggleMoveLock(false);
                }
            }
        }
    }
}
