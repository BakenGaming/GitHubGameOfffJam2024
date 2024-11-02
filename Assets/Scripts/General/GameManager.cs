using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _i;


    [SerializeField] private Transform spawnPoint;
   
    private GameObject playerGO;
    private Transform currentSpawnPoint;
    private Camera mainCam;
    private bool isPaused, flashLightInactive;

    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;

    private void OnEnable() 
    {
        DamageHandler.OnPlayerDie += ReSpawnPlayer;    
    }

    private void OnDisable() 
    {
        DamageHandler.OnPlayerDie -= ReSpawnPlayer;    
    }
    private void Awake() 
    {
        _i = this;  
        mainCam = Camera.main;
        currentSpawnPoint = spawnPoint;
        SpawnPlayer();
        SetupObjectPools();  
    }

    private void SpawnPlayer() 
    {
        playerGO = Instantiate(GameAssets.i.pfPlayer, spawnPoint.position, Quaternion.identity);
        playerGO.transform.SetParent(null);
        playerGO.GetComponent<PlayerInputController>().Initialize();
    }

    private void ReSpawnPlayer(HazardType type)
    {
        playerGO = Instantiate(GameAssets.i.pfPlayer, currentSpawnPoint.position, Quaternion.identity);
        playerGO.transform.SetParent(null);
        playerGO.GetComponent<PlayerInputController>().Initialize();
    }


    public void SetupObjectPools()
    {
        //Do the below for all objects that will need pooled for use
        //ObjectPooler.SetupPool(OBJECT, SIZE, "NAME") == Object is pulled from GameAssets, Setup object with a SO that contains size and name
        
        //The below is placed in location where object is needed from pool
        //==============================
        //PREFAB_SCRIPT instance = ObjectPooler.DequeueObject<PREFAB_SCRIPT>("NAME");
        //instance.gameobject.SetActive(true);
        //instance.Initialize();
        //==============================
    }

    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public GameObject GetPlayerGO() { return playerGO; }
    public bool GetIsPaused() { return isPaused; }

}
