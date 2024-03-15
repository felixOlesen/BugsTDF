using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
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
public TMP_Text priceOne;
public TMP_Text priceTwo;
public TMP_Text priceThree;
public TMP_Text sellPrice;
public GameObject lvlUpMenu;
public GameObject levelManager;
private Vector3 mousePos;

public GameObject goldOne;
public GameObject goldTwo;
public GameObject goldThree;
public GameObject silverOne;
public GameObject silverTwo;
public GameObject silverThree;
public GameObject bronzeOne;
public GameObject bronzeTwo;
public GameObject bronzeThree;
public GameObject buttonOne;
public GameObject buttonTwo;
public GameObject buttonThree;

public GameObject completePanel1;
public GameObject completePanel2;
public GameObject completePanel3;

public GameObject upgradeIcon1;
public GameObject upgradeIcon2;
public GameObject upgradeIcon3;


private void Update() {
    mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;

    if(!PauseMenuController.isPaused && Input.GetMouseButtonDown(0)) {
        Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 4);

        if(currentTower != null && (hit.collider == null || !hit.collider.CompareTag("Tower"))) {
            if(hit.collider == null){
                currentTower.GetComponent<TowerController>().SetSelection(false);
                lvlUpMenu.SetActive(false);
            } 
        }
    }
    if(lvlTree != null && !PauseMenuController.isPaused) {
        if(lvlTree[0].Count > 0 && levelManager.GetComponent<LevelManager>().CheckMoneyTotal(lvlTree[0].Peek().upgradeCost)) {
            buttonOne.GetComponent<Button>().interactable = true;
        } else {
            buttonOne.GetComponent<Button>().interactable = false;
        }

        if(lvlTree[1].Count > 0 && levelManager.GetComponent<LevelManager>().CheckMoneyTotal(lvlTree[1].Peek().upgradeCost)) {
            buttonTwo.GetComponent<Button>().interactable = true;
        } else {
            buttonTwo.GetComponent<Button>().interactable = false;
        }

        if(lvlTree[2].Count > 0 && levelManager.GetComponent<LevelManager>().CheckMoneyTotal(lvlTree[2].Peek().upgradeCost)) {
            buttonThree.GetComponent<Button>().interactable = true;
        } else {
            buttonThree.GetComponent<Button>().interactable = false;
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
    sellPrice.text = "$" + currentTower.GetComponent<TowerController>().sellPrice.ToString();

    if(lvlTree[0].Count != 0) {
        completePanel1.SetActive(false);
        upgradeIcon1.SetActive(true);
        buttonOne.SetActive(true);
        goldOne.SetActive(true);
        nameOne.text = lvlTree[0].Peek().upgradeName;
        descOne.text = lvlTree[0].Peek().description;
        priceOne.text = "$" + Mathf.Abs(lvlTree[0].Peek().upgradeCost).ToString();
        upgradeIcon1.GetComponent<Image>().sprite = lvlTree[0].Peek().upgradeIcon;
    } else {
        completePanel1.SetActive(true);
        nameOne.text = "";
        descOne.text = "";
        priceOne.text = "";
        upgradeIcon1.SetActive(false);
        buttonOne.SetActive(false);
        // goldOne.SetActive(false);
        // silverOne.SetActive(false);
        // bronzeOne.SetActive(false);
    }
    if(lvlTree[1].Count != 0) {
        completePanel2.SetActive(false);
        upgradeIcon2.SetActive(true);
        buttonTwo.SetActive(true);
        goldTwo.SetActive(true);
        nameTwo.text = lvlTree[1].Peek().upgradeName;
        descTwo.text = lvlTree[1].Peek().description;
        priceTwo.text = "$" + Mathf.Abs(lvlTree[1].Peek().upgradeCost).ToString();
        upgradeIcon2.GetComponent<Image>().sprite = lvlTree[1].Peek().upgradeIcon;
    } else {
        completePanel2.SetActive(true);
        nameTwo.text = "";
        descTwo.text = "";
        priceTwo.text = "";
        upgradeIcon2.SetActive(false);
        buttonTwo.SetActive(false);
        // goldTwo.SetActive(false);
        // silverTwo.SetActive(false);
        // bronzeTwo.SetActive(false);
    }
    if(lvlTree[2].Count != 0) {
        completePanel3.SetActive(false);
        upgradeIcon3.SetActive(true);
        buttonThree.SetActive(true);
        goldThree.SetActive(true);
        nameThree.text = lvlTree[2].Peek().upgradeName;
        descThree.text = lvlTree[2].Peek().description;
        priceThree.text = "$" + Mathf.Abs(lvlTree[2].Peek().upgradeCost).ToString();
        upgradeIcon3.GetComponent<Image>().sprite = lvlTree[2].Peek().upgradeIcon;
    } else {
        completePanel3.SetActive(true);
        nameThree.text = "";
        descThree.text = "";
        priceThree.text = "";
        upgradeIcon3.SetActive(false);
        buttonThree.SetActive(false);
        // goldThree.SetActive(false);
        // silverThree.SetActive(false);
        // bronzeThree.SetActive(false);
    }
    UpdateTierImages();
    UpdateTierSkin(tower, lvlTree);

}

public void UpdateTierSkin(GameObject tower, List<Queue<TowerData>> tree) {
    if(tree[0].Count <= 2 && tree[1].Count <= 2 && tree[2].Count <= 2) {
        tower.GetComponent<TowerController>().UpdateSkinLevel(1);
    } 
    if(tree[0].Count <= 1 && tree[1].Count <= 1 && tree[2].Count <= 1) {
        tower.GetComponent<TowerController>().UpdateSkinLevel(2);
    } 
    if(tree[0].Count <= 0 && tree[1].Count <= 0 && tree[2].Count <= 0) {
        tower.GetComponent<TowerController>().UpdateSkinLevel(3);
    }
}

public void SellTower() {
    /*
    1. Get sellprice
    2. Destroy object
    3. Set LvlUp Menu inactive
    4. Add Money
    */
    int money = currentTower.GetComponent<TowerController>().sellPrice;
    Destroy(currentTower);
    lvlUpMenu.SetActive(false);
    levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(money);
}

private void UpdateTierImages() {
    GameObject bronze = null;
    GameObject silver = null;
    GameObject gold = null;

    for(int i = 0; i < lvlTree.Count; i++){
        if(i == 0) {
            bronze = bronzeOne;
            silver = silverOne;
            gold = goldOne;
        } else if(i == 1) {
            bronze = bronzeTwo;
            silver = silverTwo;
            gold = goldTwo;
        } else if(i == 2) {
            bronze = bronzeThree;
            silver = silverThree;
            gold = goldThree;
        }
        if(lvlTree[i].Count == 3) {
            bronze.SetActive(true);
            silver.SetActive(false);
            gold.SetActive(false);
        } else if(lvlTree[i].Count == 2) {
            bronze.SetActive(false);
            silver.SetActive(true);
            gold.SetActive(false);
        } else if(lvlTree[i].Count == 1) {
            bronze.SetActive(false);
            silver.SetActive(false);
            gold.SetActive(true);
        } else {
            bronze.SetActive(false);
            silver.SetActive(false);
            gold.SetActive(false);
        }
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
    if(!PauseMenuController.isPaused) {
        if(lvlTree[branch].Count != 0) {
            int cost = lvlTree[branch].Peek().upgradeCost;
            if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(cost)) {
                string index = lvlTree[branch].Peek().buffIdx;
                float magnitude = lvlTree[branch].Peek().magnitude;
                ImproveStats(index, magnitude);
                levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(cost);
                currentTower.GetComponent<TowerController>().UpdateSellPrice(Mathf.Abs(cost));
                lvlTree[branch].Dequeue();
            }
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
    if(idx == "addStealthVision") {
        currentTower.GetComponent<TowerController>().stealthVision = true;
    }
    if(idx == "addArmourPierce") {
        currentTower.GetComponent<TowerController>().armourPierce = true;
    }
    if(idx == "addArmourDestroying") {
        currentTower.GetComponent<TowerController>().armourDestroying = true;
    }
    if(idx == "addExplosive") {
        currentTower.GetComponent<TowerController>().aoeType = "explosive";
    }
    if(idx == "addStun") {
        currentTower.GetComponent<TowerController>().aoeType = "stun";
    }
    if(idx == "aoeRadius") {
        currentTower.GetComponent<TowerController>().aoeRadius *= mag;
    }
    if(idx == "stunDuration") {
        currentTower.GetComponent<TowerController>().stunDuration *= mag;
    }

}
}
