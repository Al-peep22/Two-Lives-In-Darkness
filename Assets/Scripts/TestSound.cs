using UnityEngine;

public class TestSound : MonoBehaviour
{
    [SerializeField] private SFXClip clipToPlay;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            SFXManager.instance.PlaySFXClip(clipToPlay, transform, 1f);
            Debug.Log("Pressed T Audio Played");
        }
    }

}
