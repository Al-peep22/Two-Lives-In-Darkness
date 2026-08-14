using System;
using System.Collections;
using UnityEngine;

//Keeps track of time of day so certain options change based on time of day (example when near bed: Night-"Lay in Bed", Day-"Sit on Bed")
//Keeps track of player choices and what they have done so far in the story
public class StoryManager : MonoBehaviour
{
    public static StoryManager instance;

    private string currentScene = "opening";

    [Header("Cat Prefabs")]
    public GameObject catStanding;
    public GameObject catSitting;
    public GameObject catLaying;

    [Header("Cat Audios")]
    public SFXClip[] catMeowing;
    public SFXClip[] catHiss;
    public SFXClip catAngry;
    public SFXClip catAlert;
    public SFXClip catPurr;
    public SFXClip catJump;
    public SFXClip catEating;

    [Header("Person Prefabs")]
    public GameObject hand;
    public GameObject cafeCoffee;
    public GameObject vase;
    public GameObject person;

    [Header("Person Audios")]
    public SFXClip[] drinking;
    public SFXClip huming;
    public SFXClip gasp;

    [Header("Scene Points")]
    public Transform scene1Cat;
    public Transform scene1Thud;
    public Transform scene1distantMeow;
    public Transform scene2Bowls;
    public Transform scene2CatWaiting;

    [Header("Action Audios")]
    public SFXClip fillBowl;

    private bool cutSceneInProcess = false;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public bool getProgress() {
        return cutSceneInProcess;
    }

    public void openingScene()
    {
        cutSceneInProcess = true;
        PlayerAnimations.Instance.switchOptionsActive();
        StartCoroutine(OpeningSceneRoutine());
    }

    private IEnumerator OpeningSceneRoutine()
    {
        yield return new WaitForSeconds(2f);

        // 1. Play a random cat meow
        SFXManager.instance.PlaySFX3DClip(
            catMeowing[0],
            scene1Cat,
            1f
        );

        // 2. Spawn standing cat at scene1Cat
        GameObject cat = Instantiate(catStanding, scene1Cat.position, scene1Cat.rotation);

        // Small delay for pacing
        yield return new WaitForSeconds(8f);

        Destroy(cat);

        // 3. Play thud (cat jumps off bed)
        SFXManager.instance.PlaySFX3DClip(catJump, scene1Thud, 2f);

        // 4. Play another meow at thud location
        yield return new WaitForSeconds(catJump.clip.length);
        SFXManager.instance.PlaySFX3DClip(
            catMeowing[1],
            scene1Thud,
            1f
        );

        // 5. Play distant meow
        yield return new WaitForSeconds(5f);
        SFXManager.instance.PlaySFX3DClip(
            catMeowing[2],
            scene1distantMeow,
            0.5f
        );
        currentScene = "catHunger";
        yield return StartCoroutine(CatHungerSceneRoutine());

        cutSceneInProcess = false;
        PlayerAnimations.Instance.switchOptionsActive();
    }

    public void HandlePlayerChoice(string text)
    {
        if (text == "Refill Bowls") {
            cutSceneInProcess = false;
            SFXManager.instance.PlaySFXClip(fillBowl, scene2Bowls, 1f);
            UIManager.Instance.RemoveTask("Feed Kesi");
            StartCoroutine(FeedingSceneRoutine());
            currentScene = "";
            Debug.Log("Removed Task (Feed Kesi)");
        } else if (text == "catHunger") {
            cutSceneInProcess = false;
            StartCoroutine(CatHungerSceneRoutine());
        } else if (text == "cafeEnter1") {
            cutSceneInProcess = true;
            StartCoroutine(cafeFirstConvo());
        }
        PlayerAnimations.Instance.switchOptionsActive();
    }
    private IEnumerator FeedingSceneRoutine() {
        yield return new WaitForSeconds(fillBowl.clip.length);
        SFXManager.instance.PlaySFX3DClip(catEating, scene2Bowls, 1f);
        GameObject cat = Instantiate(catStanding, scene2Bowls.position, scene2Bowls.rotation);
        yield return new WaitForSeconds(catEating.clip.length);
        Destroy(cat);
    }
    private IEnumerator CatHungerSceneRoutine() {
        yield return new WaitForSeconds(2f);
        if (currentScene != "catHunger") yield break;
        SFXManager.instance.PlaySFX3DClip(catMeowing[0], scene2CatWaiting, 0.5f);
        GameObject cat = Instantiate(catSitting, scene2CatWaiting.position, scene2CatWaiting.rotation);
        yield return new WaitForSeconds(2f);
        Destroy(cat);
        yield return new WaitForSeconds(UnityEngine.Random.Range(4f, 10f));
        if (currentScene == "catHunger") {
            HandlePlayerChoice(currentScene);
        }
    }

    private IEnumerator cafeFirstConvo() {
        yield return PlayerAnimations.Instance.SittingDownCafe();
        //make the player sitdown with PlayerAnimations.cs
        //then person come up to player and say hello and take their regular order and small talk triggers
        yield return new WaitForSeconds(2f);
        cutSceneInProcess = false;
        PlayerAnimations.Instance.switchOptionsActive();
    }
}
