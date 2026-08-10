using UnityEngine;

public class SFXManager : MonoBehaviour
{
    public static SFXManager instance;

    [Header("Audio Source Prefabs")]
    [SerializeField] private AudioSource SFXObject;
    [SerializeField] private AudioSource SFXObject3D;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public AudioSource PlaySFXClip(SFXClip sfx, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = sfx.clip;
        audioSource.volume = sfx.baseVolume * volume;

        // Start at the correct time
        audioSource.time = sfx.secSkipped;

        audioSource.Play();

        float duration = (sfx.secEndEarly <= 0)
    ? audioSource.clip.length - sfx.secSkipped
    : sfx.secEndEarly - sfx.secSkipped;


        Destroy(audioSource.gameObject, duration);

        return audioSource;
    }

    public void PlaySFX3DClip(SFXClip sfx, Transform spawnTransform, float volume)
    {
        AudioSource audioSource = Instantiate(SFXObject3D, spawnTransform.position, Quaternion.identity);

        audioSource.clip = sfx.clip;
        audioSource.volume = sfx.baseVolume * volume;

        audioSource.time = sfx.secSkipped;
        audioSource.Play();

        float duration = (sfx.secEndEarly <= 0)
     ? audioSource.clip.length - sfx.secSkipped
     : sfx.secEndEarly - sfx.secSkipped;


        Destroy(audioSource.gameObject, duration);
    }

    public void PlayRandomSFXClip(SFXClip[] clips, Transform spawnTransform, float volume)
    {
        int randIndex = Random.Range(0, clips.Length);
        SFXClip sfx = clips[randIndex];

        AudioSource audioSource = Instantiate(SFXObject, spawnTransform.position, Quaternion.identity);

        audioSource.clip = sfx.clip;
        audioSource.volume = sfx.baseVolume * volume;

        audioSource.time = sfx.secSkipped;
        audioSource.Play();

        float duration = (sfx.secEndEarly <= 0)
    ? audioSource.clip.length - sfx.secSkipped
    : sfx.secEndEarly - sfx.secSkipped;


        Destroy(audioSource.gameObject, duration);
    }
}
