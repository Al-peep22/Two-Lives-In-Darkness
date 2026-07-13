using UnityEngine;

public class DriftCollider : MonoBehaviour
{
    public TextureJumpController textureController;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            // Player touched the collider → snap texture to this position
            textureController.SnapToCollider();

            //// Optional: zero out collider offset (your “reset” mechanic)
            //transform.localPosition = Vector3.zero;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            textureController.SnapToCollider();
            transform.localPosition = Vector3.zero;
        }
    }
}
