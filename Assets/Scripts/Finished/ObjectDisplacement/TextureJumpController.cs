using System.Collections.Generic;
using UnityEngine;

public class TextureJumpController : MonoBehaviour
{
    public Transform realCollider;
    public GameObject textureObject;

    public float snapSpeed = 20f;

    private bool shouldSnap = false;

    [System.Serializable]
    public class EffectObject
    {
        public Transform OriginalPlacement;
        public Transform ActualPlacement;
    }

    public List<EffectObject> effectObjects = new List<EffectObject>();

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

    public void SnapSelectedObject(EffectObject obj)
    {
        obj.OriginalPlacement.gameObject.SetActive(false);
        obj.ActualPlacement.gameObject.SetActive(true);

        if (textureObject.GetComponent<Collider>() != null)
            textureObject.GetComponent<Collider>().enabled = false;

        shouldSnap = true;
    }

    public void HideTexture()
    {
        textureObject.SetActive(false);
        textureObject.transform.position = realCollider.position;
    }

    public void HideAffectedObjects()
    {
        foreach (var obj in effectObjects)
        {
            obj.OriginalPlacement.gameObject.SetActive(false);
            obj.ActualPlacement.gameObject.SetActive(false);
            obj.OriginalPlacement.position = obj.ActualPlacement.position;
            obj.OriginalPlacement.rotation = obj.ActualPlacement.rotation;
        }
    }

}
