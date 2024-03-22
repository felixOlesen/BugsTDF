using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public int overallBugsKilled;
    public int overallMoneySpent;
    public int overallTurretsPlaced;


public GameData(GameDataController gameDataController) {
    overallBugsKilled = gameDataController.overallBugsKilled;
    overallMoneySpent = gameDataController.overallMoneySpent;
    overallTurretsPlaced = gameDataController.overallTurretsPlaced;
}

}
