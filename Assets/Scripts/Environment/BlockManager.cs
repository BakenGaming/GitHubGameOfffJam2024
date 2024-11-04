using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq.Expressions;
using UnityEngine;

public class BlockManager : MonoBehaviour
{
    public static event Action OnAllBlocksSpawned;
    public static event Action OnCheatComplete;
    public static event Action OnBlocksExploded;
    [SerializeField] private List<SpawnPoint> spawnPoints;
    [SerializeField] private Color normalColor, cheatColor, finalCheatColor;
    private float cheatTimer;

    private List<Block> tntBlocks;
    private List<Block> miningBlocksSpawned;

    private void OnEnable() 
    {
        Block.OnTNTActivated += AddToTNTBlock; 
        GridMovement.OnSequenceExecuted += ExplodeBlocks; 
        UIController.OnSequenceReset += ResetSequence;  
        UIController.OnHideBlocks += HideBlocks;  
    }

    private void OnDisable() 
    {
        Block.OnTNTActivated -= AddToTNTBlock;    
        GridMovement.OnSequenceExecuted -= ExplodeBlocks;   
        UIController.OnSequenceReset -= ResetSequence;
        UIController.OnHideBlocks -= HideBlocks;  
    }

    private void Start() 
    {
        tntBlocks = new List<Block>();
        miningBlocksSpawned = new List<Block>();
        foreach(SpawnPoint point in spawnPoints)
        {
            if(point.GetPointType() == PointType.miningBlock)
            {
                GameObject newBlock = Instantiate(GameAssets.i.pfMiningBlock, point.gameObject.transform.position, Quaternion.identity);
                miningBlocksSpawned.Add(newBlock.GetComponent<Block>());
            }
            
            if (point.GetPointType() == PointType.exit)
                Instantiate(GameAssets.i.pfExit, point.gameObject.transform.position, Quaternion.identity);
            
            if (point.GetPointType() == PointType.playerSpawn)
                GameManager.i.SetSpawnPoint(point.gameObject.transform);
        }  

        OnAllBlocksSpawned?.Invoke();
    }

    private void AddToTNTBlock(Block _block)
    {
        tntBlocks.Add(_block);
    }

    private void ExplodeBlocks()
    {
        StartCoroutine("Explode");
    }

    IEnumerator Explode()
    {
        foreach (Block block in tntBlocks)
        {
            block.Explode();
            yield return new WaitForSeconds(1f);
        }
        OnBlocksExploded?.Invoke();
    }

    private void ResetSequence()
    {
        tntBlocks.Clear();
    }

    //======= Cheats ========

    private void HideBlocks(float _time, bool _isFinalCheat)
    {
        cheatTimer = _time;
        if(_isFinalCheat)
        {
            foreach (Block block in miningBlocksSpawned)
            {
                block.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = finalCheatColor;    
            }
            OnCheatComplete?.Invoke();
        }
        else StartCoroutine("TemporaryBlockHideCheat");
    }

    IEnumerator TemporaryBlockHideCheat()
    {
        foreach (Block block in miningBlocksSpawned)
        {
            block.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = cheatColor;    
        }
        yield return new WaitForSecondsRealtime(cheatTimer);
        foreach (Block block in miningBlocksSpawned)
        {
            block.transform.Find("Sprite").GetComponent<SpriteRenderer>().color = normalColor;    
        }
        OnCheatComplete?.Invoke();
    }
}
