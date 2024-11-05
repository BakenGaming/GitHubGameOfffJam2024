using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static event Action OnPlayerSpawned;
    private static GameManager _i;
    [SerializeField] private int dynamiteCount;
    private Transform spawnPoint;
    private GameObject playerGO;
    private Transform currentSpawnPoint;
    private Camera mainCam;
    private GridMovement gridMovement;
    private bool isPaused, flashLightInactive;

    public static GameManager i { get { return _i; } }
    [SerializeField] private Transform sysMessagePoint;

    private void OnEnable() 
    {
        DamageHandler.OnPlayerDie += ReSpawnPlayer; 
        BlockManager.OnAllBlocksSpawned += Initialize;   
    }

    private void OnDisable() 
    {
        DamageHandler.OnPlayerDie -= ReSpawnPlayer;    
        BlockManager.OnAllBlocksSpawned -= Initialize;
    }
    private void Awake() 
    {
        _i = this;  
    }
    private void Initialize() 
    {
        mainCam = Camera.main;
        dynamiteCount = StaticVariables.i.GetGameStats().dynamiteCount;
        SpawnPlayer();
        SetupObjectPools();  
    }
    public void SetSpawnPoint(Transform _point)
    {
        spawnPoint = _point;
    }

    private void SpawnPlayer() 
    {
        playerGO = Instantiate(GameAssets.i.pfPlayer, spawnPoint.position, Quaternion.identity);
        playerGO.transform.SetParent(null);
        gridMovement = playerGO.GetComponent<GridMovement>();
        gridMovement.Initialize();
        OnPlayerSpawned?.Invoke();
    }

    private void ReSpawnPlayer(HazardType type)
    {
        playerGO = Instantiate(GameAssets.i.pfPlayer, currentSpawnPoint.position, Quaternion.identity);
        playerGO.transform.SetParent(null);
        playerGO.GetComponent<GridMovement>().Initialize();
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

    public GridMovement GetMovementScript(){return gridMovement;}
    public int GetDynamiteCount(){return dynamiteCount;}
    public void UpdateDynamiteCount(bool _increase){if(_increase) dynamiteCount++; else dynamiteCount--;}
    
    public Transform GetSysMessagePoint(){ return sysMessagePoint;}
    public GameObject GetPlayerGO() { return playerGO; }
    public bool GetIsPaused() { return isPaused; }

}
