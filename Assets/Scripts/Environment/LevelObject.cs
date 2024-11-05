using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ObjectType
{
    key, gold, hazard, door, TNT
}
public class LevelObject : MonoBehaviour, IObject
{
    [SerializeField] private ObjectType type;

    public ObjectType GetObjectType(){return type;}
    
}
