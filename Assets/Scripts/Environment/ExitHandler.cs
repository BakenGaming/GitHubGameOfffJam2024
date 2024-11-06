using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExitHandler : MonoBehaviour
{
    [SerializeField] private GameObject openDoor;
    [SerializeField] private GameObject closedDoor;

    private void OnEnable() 
    {
        GridMovement.OnKeyCollected += OpenDoor;    
    }

    private void OnDisable() 
    {
        GridMovement.OnKeyCollected -= OpenDoor;    
    }

    private void Awake() 
    {
        openDoor.SetActive(false);
        closedDoor.SetActive(true);    
    }

    private void OpenDoor(ObjectType type)
    {
        openDoor.SetActive(true);
        closedDoor.SetActive(false);    
    }

}
