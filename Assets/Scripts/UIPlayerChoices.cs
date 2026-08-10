using System;
using TMPro;
using UnityEngine;

public class UIPlayerChoices : MonoBehaviour
{
    [System.Serializable]
    public class OptionUI
    {
        public GameObject root;
        public GameObject selectedBox;  
    }

    public OptionUI[] options = new OptionUI[4]; // size 4

    public void UpdateOptionTexts(string[] newTexts)
    {
        for (int i = 0; i < options.Length; i++)
        {
            if (i < newTexts.Length)
            {
                options[i].root.SetActive(true);

                TMP_Text txt = options[i].root.GetComponent<TMP_Text>();
                txt.text = newTexts[i];
            }
            else
            {
                options[i].root.SetActive(false);
            }
        }

        if (newTexts.Length == 0)
        {
            for (int i = 0; i < options.Length; i++)
                options[i].selectedBox.SetActive(false);

            return;
        }
    }

    // Highlight one option
    public void SetSelectedOption(int index)
    {
        index = Mathf.Clamp(index, 0, options.Length - 1);

        for (int i = 0; i < options.Length; i++)
            options[i].selectedBox.SetActive(i == index);
    }


    public string GetSelectedOptionText(int index = 0)
    {
        if (index >= 0 && index < options.Length)
        {
            TMP_Text txt = options[index].root.GetComponent<TMP_Text>();
            return txt.text;
        }
        return null;
    }

    public int countActiveOptions() {
        int count = 0;
        foreach (OptionUI option in options) { 
            if(option.root.activeSelf) { count++; }
        }
        return count;
    }
}
