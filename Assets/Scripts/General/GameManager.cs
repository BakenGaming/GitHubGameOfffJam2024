using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _i;

    private GameObject playerGO;
    private bool isPaused;

    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;

    private void Awake() 
    {
        _i = this;    
    }

    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public GameObject GetPlayerGO() { return playerGO; }
    public bool GetIsPaused() { return isPaused; }

}
