using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowButton : MonoBehaviour
{
    public static event Action<MoveDirections> OnArrowClicked;
    [SerializeField] private MoveDirections direction;

    public void OnArrowClick()
    {
        Debug.Log(direction);
        OnArrowClicked?.Invoke(direction);
    }
}
