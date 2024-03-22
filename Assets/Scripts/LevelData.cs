using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LevelData
{
    public int remainingHealth;
    public int bugsKilled;
    public int moneySpent;
    public int turretsPlaced;


public LevelData(LevelManager levelManager) {
    remainingHealth = levelManager.remainingHealth;
    bugsKilled = levelManager.bugsKilled;
    moneySpent = levelManager.moneySpent;
    turretsPlaced = levelManager.turretsPlaced;
}

}
