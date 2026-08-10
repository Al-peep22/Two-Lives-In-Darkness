using UnityEngine;
using UnityEngine.Audio;

public class SoundMixerManager : MonoBehaviour
{
    [SerializeField] private AudioMixer audioMixer;

    public void SetMasterVolume(float level) {
        //audioMixer.SetFloat("MasterVolume", level);
        audioMixer.SetFloat("MasterVolume", Mathf.Log10(level)*20f);
    }
    public void SetSFXVolume(float level) { 
        //audioMixer.SetFloat("SFXVolume", level);
        audioMixer.SetFloat("SFXVolume", Mathf.Log10(level) * 20f);
    }
    public void SetMusicVolume(float level) { 
        //audioMixer.SetFloat("MusicVolume", level);
        audioMixer.SetFloat("MusicVolume", Mathf.Log10(level) * 20f);
    }
    public void SetEnvirormentVolume(float level) { 
        //audioMixer.SetFloat("EnvirormentVolume", level);
        audioMixer.SetFloat("EnvirormentVolume", Mathf.Log10(level) * 20f);
    }
}
