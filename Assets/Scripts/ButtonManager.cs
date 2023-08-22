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
    private bool goodPlacement;

    private void Start() {
        towers = new Dictionary<string, GameObject>(){
            {"purpleWizard", purpleWizardPreFab},
            {"bloodMoon", bloodMoonPreFab},
            {"tower3", tower3PreFab},
            {"tower4", tower4PreFab}
        };
        goodPlacement = true;
    }

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        
        if(towerHeld){
            currentTower.transform.position = mousePos;
        }

        if(Input.GetMouseButtonDown(0) && (towerHeld && goodPlacement)){
            towerHeld = false;
            currentTower.GetComponent<TowerController>().SetSelection(false);
            currentTower.GetComponent<TowerController>().SetPlacement(true);
        }
    }

    public void DetermineTowerPlacement(bool placement) {
        goodPlacement = placement;
        Debug.Log(placement);
        if(!placement) {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0f , 0f, 0.3f);
        } else {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 0f , 0f, 0.3f);
        }
    }

    public void TowerCreate(string towerName){
        GameObject preFab = towers[towerName];
        currentTower = Instantiate(preFab, mousePos, Quaternion.identity);
        towerHeld = true;
        int cost = currentTower.GetComponent<TowerController>().price;
        levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(cost);
    }

    public void StartWave(){
        levelManager.GetComponent<LevelManager>().IncrementWave();
        levelManager.GetComponent<LevelManager>().InitializeWave();


    }

}
