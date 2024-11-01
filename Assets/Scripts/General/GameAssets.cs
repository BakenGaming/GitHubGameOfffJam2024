using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;


public class GameAssets : MonoBehaviour
{
    private static GameAssets _i;

    public static GameAssets i
    { 
      get
        {
            if (_i == null) _i = (Instantiate(Resources.Load("GameAssets")) as GameObject).GetComponent<GameAssets>();
            return _i;
        }  
    }

    private void Awake() 
    {
        _i = this;    
    }

    public Transform pfSysMessage;
    public Transform pfDamagePopup;
  
    public SoundAudioClip[] audioClipArray;
    public MusicAudioClip[] musicTrackArray;

    [System.Serializable]
    public class SoundAudioClip
    {
        public AudioMixerGroup mixer;
        public SoundManager.Sound sound;
        public AudioClip audioClip;
        [Range(0f, 1f)]
        public float volume;
    }

    [System.Serializable]
    public class MusicAudioClip
    {
        public AudioMixerGroup mixer;
        public SoundManager.Music music;
        public AudioClip audioClip;
        [Range(0f, 1F)]
        public float volume;
    }
}
