using UnityEngine;

public class TextureJumpController : MonoBehaviour
{
    public Transform realCollider;
    public GameObject textureObject;

    public float snapSpeed = 20f;

    private bool shouldSnap = false;

    void Update()
    {
        if (shouldSnap)
        {
            // Move the TEXTURE, not the parent
            textureObject.transform.position = Vector3.MoveTowards(
                textureObject.transform.position,
                realCollider.position,
                snapSpeed * Time.deltaTime
            );


            if (Vector3.Distance(textureObject.transform.position, realCollider.position) < 0.01f)
            {
                textureObject.transform.position = realCollider.position;
                shouldSnap = false;
            }
        }
    }

    public void SnapToCollider()
    {
        textureObject.SetActive(true);

        if (textureObject.GetComponent<Collider>() != null)
            textureObject.GetComponent<Collider>().enabled = false;

        shouldSnap = true;
    }

    public void HideTexture()
    {
        textureObject.SetActive(false);
        textureObject.transform.position = realCollider.position;
    }
}
