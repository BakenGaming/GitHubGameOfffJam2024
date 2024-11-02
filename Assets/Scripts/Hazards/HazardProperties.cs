using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public enum HazardType
{
    spikes, blade, slammer, projectile
}
public class HazardProperties : MonoBehaviour
{
    [SerializeField] private HazardType type;

    public HazardType GetHazardType(){return type;}
}
