using UnityEngine;

//Will hover and show up when the player is near an interactable object. It will show the button to interact and the action its preforming
//For example when the player is near the radio it will show a hovering string "E - Turn On" and then automaticly update to "E - Turn Off" when the player turns on the radio.
public class HoveringIcon : MonoBehaviour
{
    [SerializeField] public string activeStr;
    [SerializeField] public string inactiveStr;
    [SerializeField] public bool isActive;
    [SerializeField] public bool optionChanges;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
