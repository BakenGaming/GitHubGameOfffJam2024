using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PointSystem
{
    public static event Action OnPointsChange;
    public int currentPoints;

    public PointSystem()
    {
        currentPoints = 0;
        OnPointsChange?.Invoke();
    }

    public void AdjustPointTotal(int _value, bool _isIncrease)
    {
        Debug.Log("Adjusting Points");
        if(_isIncrease) currentPoints = currentPoints + _value;
        else currentPoints = currentPoints + _value;

        OnPointsChange?.Invoke();
    }
    public int GetCurrentPoints(){return currentPoints;}
}
