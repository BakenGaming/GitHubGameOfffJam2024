using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class ExperienceSystem
{
    public static event Action OnLevelUp;

    private int currentExp;
    private int currentLevel;
    private int expNextLevel;

    public ExperienceSystem()
    {
        currentExp = 0;
        currentLevel = 1;
        if (currentExp == 0) expNextLevel = 10;
    }

    public void IncreaseExperience(int amount)
    {
        currentExp += amount;
        if (currentExp >= expNextLevel) LevelUp();
    }

    private void LevelUp()
    {
        currentLevel++;
        currentExp = (expNextLevel - currentExp) * 1;
        expNextLevel = (int)Mathf.Round((expNextLevel + 10) * 1.1f);
        OnLevelUp?.Invoke();
    }

    public int GetLevel()
    {
        return currentLevel;
    }
    public int GetExp()
    {
        return currentExp;
    }

    public int GetNextLevel()
    {
        return expNextLevel;
    }

    public float GetExpPercent()
    {
        return (float)currentExp / (float)expNextLevel;
    }

    public void TriggerLevelUpInEditor()
    {
        OnLevelUp?.Invoke();
    }
}
