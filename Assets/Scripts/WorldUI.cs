using UnityEngine;

public class WorldUI : MonoBehaviour
{
    public Transform faceDirection;
    public Transform target;
    public Transform worldCanvas;
    public Vector3 offset;

    private void Start() {
        transform.SetParent(worldCanvas);

        //look at spot
        transform.rotation = Quaternion.LookRotation(transform.position - faceDirection.transform.position);

        //UI offset from target
        transform.position = target.position + offset;
    }
}
