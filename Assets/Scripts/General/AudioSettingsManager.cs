using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSettingsManager : MonoBehaviour
{
    public void SetMasterVolume(float volume)
    {
        StaticVariables.i.GetMusicMixer().audioMixer.SetFloat("MasterVolume", volume);
    }

    public void SetSFXVolume(float volume)
    {
        StaticVariables.i.GetSFXMixer().audioMixer.SetFloat("SFXVolume", volume);
    }

    public void SetMusicVolume(float volume)
    {
        StaticVariables.i.GetMusicMixer().audioMixer.SetFloat("MusicVolume", volume);
    }
}
