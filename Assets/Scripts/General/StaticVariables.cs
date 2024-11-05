using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

[Serializable]
public class GameStats
{
    public float speed;
    public int dynamiteCount;
    public float movementDelay;
}
public class StaticVariables : MonoBehaviour
{
    public static StaticVariables i;
    [Header("Player Stats")]
    [SerializeField] private GameStats gameStats;

    [Header("Layer Masks")]
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private LayerMask whatIsWall;
    [SerializeField] private LayerMask whatIsHazard;
    [SerializeField] private LayerMask whatIsExitPoint;
    [SerializeField] private LayerMask whatIsPlayer;
    [SerializeField] private LayerMask whatIsEnemy;
    [SerializeField] private LayerMask collectable;
    [SerializeField] private LayerMask whatIsUI;

    [Header("Audio")]
    [SerializeField] private AudioMixerGroup masterMixer;
    [SerializeField] private AudioMixerGroup sfxMixer;
    [SerializeField] private AudioMixerGroup musicMixer;

    private void Awake() 
    {
        i = this;
    }

    public GameStats GetGameStats(){return gameStats;}
    public LayerMask GetGroundLayer(){return whatIsGround;}
    public LayerMask GetHazardLayer(){return whatIsHazard;}
    public LayerMask GetExitPointLayer(){return whatIsExitPoint;}
    public LayerMask GetWallLayer() { return whatIsWall; }
    public LayerMask GetPlayerLayer() { return whatIsPlayer; }
    public LayerMask GetEnemyLayer() { return whatIsEnemy; }
    public LayerMask GetCollectableLayer() { return collectable; }
    public LayerMask GetUILayer(){ return whatIsUI; }
    public AudioMixerGroup GetMasterMixer(){ return masterMixer; }
    public AudioMixerGroup GetSFXMixer(){ return sfxMixer; }
    public AudioMixerGroup GetMusicMixer(){ return musicMixer; }

}
