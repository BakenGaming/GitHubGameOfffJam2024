using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[Serializable]
public class GamePoints
{
    public int goldPoints; 
    public int totalGoldPoints;
    public int keyPoints;
    public int deathDeduction;
    public int noDeathBonus;
    public int noCheatBonus;
    public int[] cheatDeductions;
}
public class GameManager : MonoBehaviour
{
    public static GameManager i { get { return _i; } }
    public static event Action OnPlayerSpawned;
    private static GameManager _i;
    [Header("Point Values")]
    [SerializeField] private GamePoints gamePoints;
    private int dynamiteCount, goldCount, levelGoldTotal;
    private Transform spawnPoint;
    private GameObject playerGO;
    private Transform currentSpawnPoint;
    private GridMovement gridMovement;
    private PointSystem pointSystem;
    private bool isPaused;


    private void OnEnable() 
    {
        BlockManager.OnAllBlocksSpawned += Initialize;  
    }

    private void OnDisable() 
    {
        BlockManager.OnAllBlocksSpawned -= Initialize;
    }
    private void Awake() 
    {
        _i = this;  
    }
    private void Initialize() 
    {
        dynamiteCount = StaticVariables.i.GetGameStats().dynamiteCount;
        goldCount = 0;
        pointSystem = new PointSystem();
        SpawnPlayer();
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

    private void ReSpawnPlayer()
    {
        playerGO = Instantiate(GameAssets.i.pfPlayer, currentSpawnPoint.position, Quaternion.identity);
        playerGO.transform.SetParent(null);
        playerGO.GetComponent<GridMovement>().Initialize();
    }

    //Set Functions
    public void PauseGame(){if(isPaused) return; else isPaused = true;}
    public void UnPauseGame(){if(isPaused) isPaused = false; else return;}
    public void SetTotalGoldForLevel(int _amount) {levelGoldTotal = _amount;}
    public void UpdatedGoldCount(){goldCount++;}
    public void UpdateDynamiteCount(bool _increase){if(_increase) dynamiteCount++; else dynamiteCount--;}

    //Get functions
    public GridMovement GetMovementScript(){return gridMovement;}
    public PointSystem GetPointSystem(){return pointSystem;}
    public GamePoints GetGamePoints(){return gamePoints;}
    public GameObject GetPlayerGO() { return playerGO; }
    public int GetTotalGoldForLevel(){return levelGoldTotal;}
    public int GetDynamiteCount(){return dynamiteCount;}
    public int GetGoldCount(){return goldCount;}
    public bool GetIsPaused() { return isPaused; }

}
