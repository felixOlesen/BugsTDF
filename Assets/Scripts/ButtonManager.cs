using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{
    public GameObject purpleWizardPreFab;
    public GameObject bloodMoonPreFab;
    public GameObject tower3PreFab;
    public GameObject tower4PreFab;
    public GameObject levelManager;
    private Vector3 mousePos;
    private GameObject currentTower;
    private bool towerHeld;
    private IDictionary<string, GameObject> towers;

    private void Start() {
        towers = new Dictionary<string, GameObject>(){
            {"purpleWizard", purpleWizardPreFab},
            {"bloodMoon", bloodMoonPreFab},
            {"tower3", tower3PreFab},
            {"tower4", tower4PreFab}
        };
    }

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        
        if(towerHeld){
            currentTower.transform.position = mousePos;
        }

        if(Input.GetMouseButtonDown(0) && towerHeld){
            towerHeld = false;
            currentTower.GetComponent<TowerController>().SetSelection(false);
        }
    }

    public void TowerCreate(string towerName){
        GameObject preFab = towers[towerName];
        currentTower = Instantiate(preFab, mousePos, Quaternion.identity);
        towerHeld = true;
        int cost = currentTower.GetComponent<TowerController>().price;
        Debug.Log(cost);
        levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(cost);
    }

    public void StartWave(){
        levelManager.GetComponent<LevelManager>().IncrementWave();
        levelManager.GetComponent<LevelManager>().InitializeWave();


    }

}
