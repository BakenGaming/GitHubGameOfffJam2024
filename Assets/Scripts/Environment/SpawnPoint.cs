using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PointType
{
    playerSpawn, exit, miningBlock
}
public class SpawnPoint : MonoBehaviour
{
    [SerializeField] private PointType type;

    public PointType GetPointType(){return type;}
    private void OnDrawGizmos() 
    {
        if(type == PointType.playerSpawn) Gizmos.color = Color.blue;    
        else if(type == PointType.exit) Gizmos.color = Color.red; 
        else Gizmos.color = Color.gray;

        Gizmos.DrawSphere(transform.position, .5f);       
    }
}
