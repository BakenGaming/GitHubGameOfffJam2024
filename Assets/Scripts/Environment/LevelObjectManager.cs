using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
    [SerializeField] private List<LevelObject> levelObjects;

    private int goldCount=0;

    private void OnEnable() 
    {
        BlockManager.OnBlocksExploded += EnableColliders; 
        GameManager.OnPlayerSpawned += Initialize;       
    }

    private void OnDisable() 
    {
        BlockManager.OnBlocksExploded -= EnableColliders;    
        GameManager.OnPlayerSpawned -= Initialize;
    }

    private void Initialize()
    {
        foreach (LevelObject _object in levelObjects)
        {
            _object.gameObject.GetComponent<CircleCollider2D>().enabled = false;
            if(_object.GetObjectType() == ObjectType.gold) goldCount++;
        }
        GameManager.i.SetTotalGoldForLevel(goldCount);

    }
    private void EnableColliders()
    {
        foreach (LevelObject _object in levelObjects)
        {
            _object.gameObject.GetComponent<CircleCollider2D>().enabled = true;
        }
    }
}
    

