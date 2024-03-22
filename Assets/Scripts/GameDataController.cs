using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameDataController : MonoBehaviour
{
    public int overallBugsKilled;
    public int overallMoneySpent;
    public int overallTurretsPlaced;

    private void Start() {
        //Load game data
        GameData data = SaveSystem.LoadGameData();

        if(data != null) {
            overallBugsKilled = data.overallBugsKilled;
            overallMoneySpent = data.overallMoneySpent;
            overallTurretsPlaced = data.overallTurretsPlaced;
            Debug.Log("Save File Found");
        } else {
            overallBugsKilled = 0;
            overallMoneySpent = 0;
            overallTurretsPlaced = 0;
            Debug.Log("Save File Not Found");
        }
        Debug.Log("Bugs: " + overallBugsKilled);
        Debug.Log("Money: " + overallMoneySpent);
        Debug.Log("Turrets: " + overallTurretsPlaced);
        
    }

    public void AddBugsKilled(int bugs) {
        Debug.Log("Before AddedBugs: " + overallBugsKilled);
        overallBugsKilled += bugs;
        Debug.Log("After AddedBugs: " + overallBugsKilled);
    }

    public void AddMoneySpent(int money) {
        Debug.Log("Before MoneySpent: " + overallMoneySpent);
        overallMoneySpent+= money;
        Debug.Log("After MoneySpent: " + overallMoneySpent);
    }

    public void AddTurretsPlaced(int turrets) {
        Debug.Log("Before TurretsPlaces: " + overallTurretsPlaced);
        overallTurretsPlaced += turrets;
        Debug.Log("After TurretsPlaces: " + overallTurretsPlaced);
    }

    public int GetBugsKilled() {
        return overallBugsKilled;
    }

    public int GetMoneySpent() {
        return overallMoneySpent;
    }

    public int GetTurretsPlaced() {
        return overallTurretsPlaced;
    }

    public void SaveGameData() {
        Debug.Log("Saving Game Data");
        SaveSystem.SaveGameData(this);
    }

    public void LoadGameData() {
        Debug.Log("Loading Game Data");
        SaveSystem.LoadGameData();
    }

}
