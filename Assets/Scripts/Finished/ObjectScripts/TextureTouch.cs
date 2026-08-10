using UnityEngine;

public class TextureTouch : MonoBehaviour
{
    public TextureJumpController controller;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            controller.HideTexture();
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.CompareTag("Player"))
        {
            controller.HideTexture();
        }
    }
}
