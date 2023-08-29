using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class LevelUpManager : MonoBehaviour
{
public List<Queue<TowerData>> lvlTree;
private GameObject currentTower;
public TMP_Text nameOne;
public TMP_Text nameTwo;
public TMP_Text nameThree;
public TMP_Text descOne;
public TMP_Text descTwo;
public TMP_Text descThree;
public GameObject lvlUpMenu;
public GameObject levelManager;
private Vector3 mousePos;


private void Update() {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

    if(!PauseMenuController.isPaused && Input.GetMouseButtonDown(0)) {
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 4);
        if(currentTower != null && (hit.collider == null || !hit.collider.CompareTag("Tower"))) {
            currentTower.GetComponent<TowerController>().SetSelection(false);
        }
        
    }
}

public void DisplayOptions(GameObject tower) {

    lvlUpMenu.SetActive(true);

    lvlTree = tower.GetComponent<TowerController>().GetLevelTree();
    if(currentTower != null) {
        currentTower.GetComponent<TowerController>().SetSelection(false); 
    }
    currentTower = tower;
    currentTower.GetComponent<TowerController>().SetSelection(true);

    if(lvlTree[0].Count != 0) {
        nameOne.text = lvlTree[0].Peek().upgradeName;
        descOne.text = lvlTree[0].Peek().description;
    } else {
        nameOne.text = "Empty";
        descOne.text = "Empty";
    }
    if(lvlTree[1].Count != 0) {
        nameTwo.text = lvlTree[1].Peek().upgradeName;
        descTwo.text = lvlTree[1].Peek().description;
    } else {
        nameTwo.text = "Empty";
        descTwo.text = "Empty";
    }
    if(lvlTree[2].Count != 0) {
        nameThree.text = lvlTree[2].Peek().upgradeName;
        descThree.text = lvlTree[2].Peek().description;
    } else {
        nameThree.text = "Empty";
        descThree.text = "Empty";
    }
    

}

public void LevelUp(int branch) {
    /*
        1. Method called by onclick button
        2. Requires int input from button referring to specific queue branch
        3. Peeks at the queue and fetches the lvl up details
        4. Calls ImproveStats() method
        5. Dequeue the specified branch
        6. Refreshes the Displayed options
    */
    branch -= 1;
    if(lvlTree[branch].Count != 0) {
        int cost = lvlTree[branch].Peek().upgradeCost;
        if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(cost)) {
            string index = lvlTree[branch].Peek().buffIdx;
            float magnitude = lvlTree[branch].Peek().magnitude;
            ImproveStats(index, magnitude);
            levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(cost);
            lvlTree[branch].Dequeue();
        }
    }
    DisplayOptions(currentTower);
}

public void ImproveStats(string idx, float mag) {
    /*
        1. Check the buff IDX (could be related to dictionary)
        2. Improve tower stat by a magnitude param 
    */
    if(idx == "attackSpeed") {
        currentTower.GetComponent<TowerController>().attackSpeed /= mag;
    }
    if(idx == "rangeRadius") {
        float changed = (float)currentTower.GetComponent<TowerController>().rangeRadius * mag;
        Mathf.RoundToInt(changed);
        currentTower.GetComponent<TowerController>().rangeRadius = (int)changed;
    }
    if(idx == "attackPower") {
        float changed = (float)currentTower.GetComponent<TowerController>().attackPower * mag;
        Mathf.RoundToInt(changed);
        currentTower.GetComponent<TowerController>().attackPower = (int)changed;
    }

}
}
