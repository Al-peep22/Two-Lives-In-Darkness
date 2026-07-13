using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public UIFader fader;
    public UIPlayerChoices playerChoices;

    private int selectedIndex = 0;

    public List<InteractableObject> nearbyObjects = new List<InteractableObject>();

    public Transform player;

    //SETTINGS: Mouse Sensitivity = 300, Emotion Opacity = Default,

    void Update()
    {
        HandleSelectionInput();
        HandleConfirmInput();

        RefreshOptions(player.forward, player.position);
    }


    public void RefreshOptions(Vector3 playerForward, Vector3 playerPos)
    {
        List<InteractableObject> validObjects = new List<InteractableObject>();

        // Only include objects close enough
        foreach (var obj in nearbyObjects)
        {
            if (obj.IsCloseEnough(playerPos))
                validObjects.Add(obj);
        }

        // If none are close enough, hide all options
        if (validObjects.Count == 0)
        {
            playerChoices.UpdateOptionTexts(new string[0]);
            return;
        }

        // Update priorities
        foreach (var obj in validObjects)
            obj.UpdatePriority(playerForward, playerPos);

        // Sort by priority (highest first)
        validObjects.Sort((a, b) => b.priority.CompareTo(a.priority));

        // Merge options (max 4)
        List<string> merged = new List<string>();

        foreach (var obj in validObjects)
        {
            foreach (var opt in obj.options)
            {
                merged.Add(opt);
                if (merged.Count >= 4)
                    break;
            }
            if (merged.Count >= 4)
                break;
        }

        // Update UI
        playerChoices.UpdateOptionTexts(merged.ToArray());
        selectedIndex = 0;
    }



    private void HandleSelectionInput()
    {
        // Arrow keys
        if (Input.GetKeyDown(KeyCode.UpArrow))
            MoveSelection(false);   // up = move up

        if (Input.GetKeyDown(KeyCode.DownArrow))
            MoveSelection(true);    // down = move down

        // Left/Right arrows (you said right = up, left = down)
        if (Input.GetKeyDown(KeyCode.RightArrow))
            MoveSelection(false);

        if (Input.GetKeyDown(KeyCode.LeftArrow))
            MoveSelection(true);

        // Mouse scroll wheel
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0f)
            MoveSelection(false);   // scroll up = move up
        else if (scroll < 0f)
            MoveSelection(true);    // scroll down = move down
    }

    private void HandleConfirmInput()
    {
        // Enter or E
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.E))
        {
            OnOptionConfirmed(selectedIndex);
        }
    }

    public void MoveSelection(bool moveDown)
    {
        int maxOptions = CountActiveOptions();

        if (moveDown)
            selectedIndex = Mathf.Min(selectedIndex + 1, maxOptions - 1);
        else
            selectedIndex = Mathf.Max(selectedIndex - 1, 0);

        playerChoices.SetSelectedOption(selectedIndex);
    }

    private int CountActiveOptions()
    {
        int count = 0;
        foreach (var opt in playerChoices.options)
            if (opt.root.activeSelf)
                count++;
        return count;
    }

    private void OnOptionConfirmed(int index)
    {
        string selectedText = playerChoices.GetSelectedOptionText(index);
        Debug.Log("Player selected option: " + index + " (" + selectedText + ")");
        // You can call your gameplay logic here
        // Example: StoryManager.Instance.HandlePlayerChoice(index);
    }
}
