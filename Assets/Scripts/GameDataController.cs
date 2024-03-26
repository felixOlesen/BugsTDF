using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameDataController : MonoBehaviour
{
    public int overallBugsKilled;
    public int overallMoneySpent;
    public int overallTurretsPlaced;
    public TMP_Text bugStats;
    public TMP_Text moneyStats;
    public TMP_Text turretStats;
    public bool tutorialNeeded;

    private void Awake() {
        //Load game data
        GameData data = SaveSystem.LoadGameData();

        if(data != null) {
            overallBugsKilled = data.overallBugsKilled;
            overallMoneySpent = data.overallMoneySpent;
            overallTurretsPlaced = data.overallTurretsPlaced;
            tutorialNeeded = data.tutorialNeeded;
            Debug.Log("Save File Found");
        } else {
            overallBugsKilled = 0;
            overallMoneySpent = 0;
            overallTurretsPlaced = 0;
            tutorialNeeded = true;
            Debug.Log("Save File Not Found");
        }

        if(bugStats && moneyStats && turretStats) {
            bugStats.text = overallBugsKilled.ToString("N0") + " Bugs Killed";
            moneyStats.text = "$" + overallMoneySpent.ToString("N0") + " Spent";
            turretStats.text = overallTurretsPlaced.ToString("N0") + " Turrets Placed";
        }
        
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

    public void SetTutorialNeeded(bool needed) {
        tutorialNeeded = needed;
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
