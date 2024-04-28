using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TurretManager : MonoBehaviour
{
[SerializeField] GameObject levelManagerObject;
LevelManager levelManager;
List<GameObject> deployedTurrets;
[SerializeField] List<GameObject> turretPrefabs;
// TurretShopUIController shopController;
TurretLevelUpController levelUpController;
GameObject currentTurret;
TurretController currentTurretController;
bool turretHeld;
Vector3 mousePosition;

private void Awake() {
    levelManager = levelManagerObject.GetComponent<LevelManager>();
    levelUpController = gameObject.GetComponent<TurretLevelUpController>();
    
    // Find UI Object in separate rendered scene
    // shopController = GameObject.Find("ShopUI").GetComponent<TurretShopUIController>();
    // if(!shopController){
    //     Debug.LogWarning("ShopController not found!");
    // }
}

private void Update() {
    mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
    mousePosition.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
    if(turretHeld) {
        currentTurret.transform.position = mousePosition;
        //TODO: Set Shop buttons to be '.interactable = false;'
    }

    //TODO: Implement Pause Checks
    if(Input.GetMouseButtonDown(0)){
        //TODO: Improve this code further
        if(turretHeld && currentTurretController.GetTurretPlacement() && !EventSystem.current.IsPointerOverGameObject()) {
            currentTurretController.TurretSelected(false);
            currentTurretController.SetDeployment(true);
            turretHeld = false;
            int price = currentTurretController.GetPrice();
            if(levelManager.CheckMoneyTotal(price)) {
                levelManager.ChangeMoneyTotal(price);
                levelManager.IncrementTurretsPlaced();
            } else {
                turretHeld = false;
                Destroy(currentTurret);
                Debug.Log("Not enough money!");
            }
        }

        Vector2 mousePosition2D = new Vector2(mousePosition.x, mousePosition.y);
        RaycastHit2D hit = Physics2D.Raycast(mousePosition2D, Vector2.zero, 4);

        if (hit.collider != null && hit.collider.CompareTag("Tower") && !EventSystem.current.IsPointerOverGameObject()) {
            GameObject selectedTurret = hit.collider.transform.parent.gameObject;
            selectedTurret.GetComponent<TurretController>().TurretSelected(true);
            //levelUpController.DisplayOptions(selectedTurret);
        }

    }

    //TODO: Display LevelUp Options
}

public void SpawnTower(int prefabNumber){
    int price = turretPrefabs[prefabNumber].GetComponent<TurretController>().GetPrice();

    if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(price)) {
        turretHeld = true;
        currentTurret = Instantiate(turretPrefabs[prefabNumber], mousePosition, Quaternion.identity);
        currentTurretController = currentTurret.GetComponent<TurretController>();
        currentTurret.GetComponent<TurretController>().TurretSelected(true);
        // cancelText.SetActive(true);
        // ClickButtonSound();
    }
}



}
