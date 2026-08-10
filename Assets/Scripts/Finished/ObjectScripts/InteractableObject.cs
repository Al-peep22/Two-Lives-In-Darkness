using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    public string[] options;
    public string[] options2;

    public bool optionsSwitch = false;
    public bool onOptions1 = true;

    public bool isRec = false;          // true = rectangle, false = radius
    public float interactDistance = 3f; // used when isRec == false
    public float interactWidth = 2f;    // rectangle width
    public float interactLength = 3f;   // rectangle length


    // Priority based on facing direction
    public float priority;

    public void UpdatePriority(Vector3 playerForward, Vector3 playerPos)
    {
        Vector3 toObj = (transform.position - playerPos).normalized;
        priority = Vector3.Dot(playerForward, toObj);
    }

    public bool IsCloseEnough(Vector3 playerPos)
    {
        if (!isRec)
        {
            // Circle distance check
            float dist = Vector3.Distance(playerPos, transform.position);
            return dist <= interactDistance;
        }
        else
        {
            // Rectangle check
            Vector3 localPos = transform.InverseTransformPoint(playerPos);

            bool withinWidth = Mathf.Abs(localPos.x) <= interactWidth * 0.5f;
            bool withinLength = Mathf.Abs(localPos.z) <= interactLength * 0.5f;

            return withinWidth && withinLength;
        }
    }


    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;

        if (!isRec)
        {
            // Draw radius sphere
            Gizmos.DrawWireSphere(transform.position, interactDistance);
        }
        else
        {
            // Draw rectangle (box)
            Vector3 size = new Vector3(interactWidth, interactDistance, interactLength);
            Gizmos.DrawWireCube(transform.position, size);
        }
    }


}
