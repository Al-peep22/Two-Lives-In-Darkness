using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static UnityEngine.Rendering.DebugUI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    [Header("Main UI Functions")]
    public UIFader fader;

    [Header("Panel Functions")]
    [SerializeField] public GameObject homePanel;
    [SerializeField] public GameObject settingsPanel;
    [SerializeField] public GameObject creditsPanel;
    [SerializeField] public GameObject gamePanel;
    [SerializeField] private CanvasGroup panelGroup;
    [SerializeField] private CanvasGroup btnsGroup;

    [Header("Settings Functions")]
    [SerializeField] private float defaultMasterVolume = 100;
    [SerializeField] private float defaultSFXVolume = 100;
    [SerializeField] private float defaultMusicVolume = 100;
    [SerializeField] private float defaultEnvirormentVolume = 100;
    [SerializeField] private float defaultCamSensitivity = 75;
    [SerializeField] private float defaultOpacity = 60;
    [SerializeField] private GameObject audioPage;
    [SerializeField] private GameObject controlsPage;
    [SerializeField] private GameObject graphicPage;
    [SerializeField] private GameObject masterSlider;
    [SerializeField] private GameObject sfxSlider;
    [SerializeField] private GameObject musicSlider;
    [SerializeField] private GameObject envirormentSlider;
    [SerializeField] private GameObject sensitivitySlider;
    [SerializeField] private GameObject opacitySlider;
    [SerializeField] private AudioSource musicTestClip;
    [SerializeField] private AudioSource natureTestClip;
    [SerializeField] private AudioSource sfxTestClip;

    [Header("Game Functions")]
    public UIPlayerChoices playerChoices;
    public RoomSwap startRoom;
    private int selectedIndex = 0;
    public List<InteractableObject> nearbyObjects = new List<InteractableObject>();
    public Transform player;
    public TextMeshProUGUI taskBar;
    public ArrayList tasks = new ArrayList();
    private string[] previousOptions = new string[0];

    [Header("Home Functions")]
    [SerializeField] private AudioSource currentMusicSource;
    [SerializeField] private SFXClip mainBgMusic;
    [SerializeField] private SFXClip startNote;
    [SerializeField] private SFXClip settingsNote;
    [SerializeField] private SFXClip creditsNote;
    [SerializeField] private SFXClip quitNote;

    private void Awake()
    {
        Instance = this;
    }

    //SETTINGS: Mouse Sensitivity = 300, Emotion Opacity = Default,

    void Update()
    {
        gamePanel.transform.Find("TasksUI").gameObject.SetActive(taskBar.text != "");
        gamePanel.transform.Find("OptionUI").gameObject.SetActive(playerChoices.countActiveOptions() != 0);

        RefreshOptions(player.forward, player.position);

        HandleSelectionInput();
        HandleConfirmInput();

    }

    // GAME UI -----------------------------------------------------------------

    public void AddTask(string newTask) { 
        tasks.Add(newTask);
        UpdateTasks();
    }

    public void RemoveTask(string finishedTask = "All") {
        if (finishedTask == "All") { 
            tasks.RemoveRange(0, tasks.Count - 1);
            return;
        }
        tasks.Remove(finishedTask);
        UpdateTasks();
    }

    public void UpdateTasks() { 
        StringBuilder sb = new StringBuilder();
        foreach (string task in tasks) {
            sb.Append(task);
            sb.Append('\n');
        }
        taskBar.text = sb.ToString();
        gamePanel.transform.Find("TasksUI").gameObject.SetActive(taskBar.text != "");
    }
    public void RefreshOptions(Vector3 playerForward, Vector3 playerPos)
    {
        List<InteractableObject> validObjects = new List<InteractableObject>();

        // Only include objects close enough
        foreach (var obj in nearbyObjects)
        {
            if (obj.IsCloseEnough(playerPos)) { 

                validObjects.Add(obj);
            }
        }

        // If none are close enough, hide all options
        if (validObjects.Count == 0)
        {
            playerChoices.UpdateOptionTexts(new string[0]);
            previousOptions = new string[0];
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
            if (obj.optionsSwitch)
            {
                if (obj.onOptions1) {
                    foreach (var opt in obj.options)
                    {
                        merged.Add(opt);
                        if (merged.Count >= 4)
                            break;
                    }
                }
                else {
                    foreach (var opt in obj.options2)
                    {
                        merged.Add(opt);
                        if (merged.Count >= 4)
                            break;
                    }
                }
            }
            else { 
                foreach (var opt in obj.options)
                {
                    merged.Add(opt);
                    if (merged.Count >= 4)
                        break;
                }
            }
            if (merged.Count >= 4)
                break;
        }

        // Convert to array
        string[] newOptions = merged.ToArray();

        // ⭐ ONLY RESET IF OPTIONS ACTUALLY CHANGED
        bool optionsChanged = !AreArraysEqual(previousOptions, newOptions);

        if (optionsChanged)
            selectedIndex = 0;

        previousOptions = newOptions;

        playerChoices.UpdateOptionTexts(newOptions);
        playerChoices.SetSelectedOption(selectedIndex);
    }

    private bool AreArraysEqual(string[] a, string[] b)
    {
        if (a.Length != b.Length)
            return false;

        for (int i = 0; i < a.Length; i++)
            if (a[i] != b[i])
                return false;

        return true;
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
            Debug.Log("Pressed 'E'");
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

        Debug.Log("Selected index = " + selectedIndex);
    }

    private int CountActiveOptions()
    {
        int count = 0;
        foreach (var opt in playerChoices.options)
            if (opt.root.activeSelf)
                count++;
        return count;
    }

    private void switchOptionsUsed(int index) {
        List<InteractableObject> validObjects = new List<InteractableObject>();

        // Only include objects close enough
        foreach (var obj in nearbyObjects)
        {
            if (obj.IsCloseEnough(player.position))
            {

                validObjects.Add(obj);
            }
        }

        // Update priorities
        foreach (var obj in validObjects)
            obj.UpdatePriority(player.forward, player.position);

        // Sort by priority (highest first)
        validObjects.Sort((a, b) => b.priority.CompareTo(a.priority));


        int searchedIndex = 0;

        foreach (var obj in validObjects)
        {
            if (obj.optionsSwitch)
            {
                if (searchedIndex == index) {
                    obj.onOptions1 = !obj.onOptions1;
                    RefreshOptions(player.forward,player.position);
                    break;
                }
            }
            if (searchedIndex == index) { break; }
            searchedIndex++;
        }
    }

    private void OnOptionConfirmed(int index)
    {
        string selectedText = playerChoices.GetSelectedOptionText(index);
        Debug.Log("Player selected option: " + index + " (" + selectedText + ")");
        switchOptionsUsed(index);

        StoryManager.instance.HandlePlayerChoice(selectedText);
    }

    // SETTINGS UI ----------------------------------------------------------
    public void restoreSettings() {
        masterSlider.GetComponent<Slider>().value = defaultMasterVolume / 100; // 100f -> 0.100f
        sfxSlider.GetComponent<Slider>().value = defaultSFXVolume / 100;
        musicSlider.GetComponent<Slider>().value = defaultMusicVolume / 100;
        envirormentSlider.GetComponent<Slider>().value = defaultEnvirormentVolume / 100;
        sensitivitySlider.GetComponent<Slider>().value = defaultCamSensitivity / 100;
        opacitySlider.GetComponent<Slider>().value = defaultOpacity / 100; // 75f -> 0.75f
        updateSliderTexts();
    }

    public void updateSliderTexts() {
        masterSlider.transform.Find("Vol#").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(masterSlider.GetComponent<Slider>().value * 100f).ToString();
        sfxSlider.transform.Find("Vol#").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(sfxSlider.GetComponent<Slider>().value * 100f).ToString();
        musicSlider.transform.Find("Vol#").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(musicSlider.GetComponent<Slider>().value * 100f).ToString();
        envirormentSlider.transform.Find("Vol#").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(envirormentSlider.GetComponent<Slider>().value * 100f).ToString();
        sensitivitySlider.transform.Find("Sens#").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(sensitivitySlider.GetComponent<Slider>().value * 100f).ToString();
        PlayerCamera.instance.mouseSensitivity = Mathf.RoundToInt(466 * sensitivitySlider.GetComponent<Slider>().value);
        opacitySlider.transform.Find("EmotionTxt").GetComponent<TextMeshProUGUI>().text = Mathf.RoundToInt(opacitySlider.GetComponent<Slider>().value * 100f).ToString();
        opacitySlider.transform.Find("EmotionTxt").GetComponentInChildren<Image>().color = new Color32(119, 182, 245, (byte)(opacitySlider.GetComponent<Slider>().value * 50f));
    }

    public void startAudioTest() {
        if (!musicTestClip.gameObject.activeInHierarchy)
            musicTestClip.gameObject.SetActive(true);
        if (!natureTestClip.gameObject.activeInHierarchy)
            natureTestClip.gameObject.SetActive(true);
        if (!sfxTestClip.gameObject.activeInHierarchy)
            sfxTestClip.gameObject.SetActive(true);

        musicTestClip.Play();
        natureTestClip.Play();
        sfxTestClip.Play();
    }
    public void stopAudioTest() { 
        musicTestClip.Stop(); 
        natureTestClip.Stop();
        sfxTestClip.Stop();
    }

    public void ShowAudioSettings() {
        hideAllSettings();
        audioPage.SetActive(true);
    }
    public void ShowControls() {
        hideAllSettings();
        controlsPage.SetActive(true);
    }
    public void ShowGraphicSettings() {
        hideAllSettings();
        graphicPage.SetActive(true);
    }

    private void hideAllSettings() {
        graphicPage.SetActive(false);
        audioPage.SetActive(false);
        controlsPage.SetActive(false);
    }

    // MAIN UI --------------------------------------------------------------

    public void hideAllPanels() { 
        homePanel.gameObject.SetActive(false);
        settingsPanel.gameObject.SetActive(false);
        creditsPanel.gameObject.SetActive(false);
        gamePanel.gameObject.SetActive(false);
    }

    // HOME UI --------------------------------------------------------------

    //Pressed Start
    public void OnStart() {
        StopMusic();
        SFXManager.instance.PlaySFXClip(startNote, transform, 1f);
        StartCoroutine(SwitchPanel(gamePanel));
        Debug.Log("Pressed Start");
        startRoom.ActivateRoom();
    }

    //Pressed Settings
    public void OnSettings()
    {
        StopMusic();
        SFXManager.instance.PlaySFXClip(settingsNote, transform, 1f);
        StartCoroutine(SwitchPanel(settingsPanel));
        Debug.Log("Pressed Settings");
    }



    //Pressed Credits
    public void OnCredits() {
        StopMusic();
        SFXManager.instance.PlaySFXClip(creditsNote, transform, 1f);
        StartCoroutine(SwitchPanel(creditsPanel));
        Debug.Log("Pressed Credits");
    }

    //Exit Game
    public void OnExit()
    {
        StartCoroutine(SwitchPanel(homePanel));
        PlayMusic();
        Debug.Log("Pressed Main Menu");
    }


    //Quit
    public void OnQuit()
    {
        StopMusic();
        StartCoroutine(QuitSequence());
    }

    private IEnumerator SwitchPanel(GameObject newPanel)
    {
        // Fade in
        yield return FadeInSequence();

        // Switch panels AFTER fade is fully visible
        hideAllPanels();
        newPanel.SetActive(true);

        if (newPanel == gamePanel) {
            yield return new WaitForSeconds(1f);
            PlayerAnimations.Instance.WakeUp();
            StoryManager.instance.openingScene();
            yield return new WaitForSeconds(1f);
        }

        // Fade OUT (from black)
        yield return FadeOutSequence();

    }


    private IEnumerator QuitSequence()
    {
        // Play quit sound
        SFXManager.instance.PlaySFXClip(quitNote, transform, 1f);

        float duration = quitNote.secEndEarly > 0
            ? quitNote.secEndEarly - quitNote.secSkipped
            : quitNote.clip.length;

        // Fade in
        yield return FadeInSequence();

        Debug.Log("Quit Application");
        Application.Quit();
    }


    private IEnumerator FadeInSequence()
    {
        panelGroup.alpha = 0f;
        panelGroup.gameObject.SetActive(true);
        // Fade screen
        if (fader != null && panelGroup != null)
        {
            btnsGroup.alpha = 0f;
            yield return fader.FadeIn(panelGroup, 1f);
        }
    }
    private IEnumerator FadeOutSequence()
    {
        if (fader != null && panelGroup != null)
        {
            btnsGroup.alpha = 1f;
            yield return fader.FadeOut(panelGroup, 1f);
        }

        panelGroup.gameObject.SetActive(false);
    }





    //Play Music
    public void PlayMusic()
    {
        currentMusicSource.loop = true;
        currentMusicSource.Play();
    }


    //Stop Music
    public void StopMusic()
    {
        if (currentMusicSource != null)
        {
            currentMusicSource.loop = false;
            currentMusicSource.Stop();
        }
    }

}