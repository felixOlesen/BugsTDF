using System.Collections;
using System.Collections.Generic;
using TMPro;
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
    public GameObject cancelText;
    public AudioSource buttonHoverSound;
    public AudioSource buttonClickSound;

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
        if(!PauseMenuController.isPaused) {
            if(Input.GetMouseButtonDown(0) && (towerHeld && goodPlacement)){
                currentTower.GetComponent<TowerController>().SetSelection(false);
                currentTower.GetComponent<TowerController>().SetPlacement(true);
                towerHeld = false;
                int cost = currentTower.GetComponent<TowerController>().price;
                if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(cost)) {
                    levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(cost);
                } else {
                    towerHeld = false;
                    Destroy(currentTower);
                    Debug.Log("Not enough money!");
                }
            }

            if (Input.GetMouseButtonDown(0)) {
                Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);
                
                RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero, 4);
                if (hit.collider != null && hit.collider.CompareTag("Tower") && goodPlacement) {
                    GameObject selectedTower = hit.collider.gameObject.transform.parent.gameObject;
                    //selected.transform.parent.GetComponent<TowerController>().SetSelection(true);
                    gameObject.GetComponent<LevelUpManager>().DisplayOptions(selectedTower);


                }
            }

            if(towerHeld && Input.GetKeyDown("x")) {
                Debug.Log("Tower Cancelled");
                towerHeld = false;
                Destroy(currentTower);
                cancelText.SetActive(false);
            } 
            if(!towerHeld) {
                cancelText.SetActive(false);
            }
            
        }
        
    }

    public void DetermineTowerPlacement(bool placement) {
        goodPlacement = placement;
        if(!placement) {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0f , 0f, 0.3f);
        } else {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(0f, 0f , 0f, 0.3f);
        }
    }

    public void TowerCreate(string towerName){
        if(!PauseMenuController.isPaused) {
            GameObject preFab = towers[towerName];
            currentTower = Instantiate(preFab, mousePos, Quaternion.identity);
            towerHeld = true;
            int cost = currentTower.GetComponent<TowerController>().price;
            currentTower.SetActive(false);
            if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(cost)) {
                currentTower.SetActive(true);
                cancelText.SetActive(true);
            } else {
                towerHeld = false;
                Destroy(currentTower);
                Debug.Log("Not enough money!");
            }
        }
        
        
    }
    public void StartWave(){
        if(!PauseMenuController.isPaused) {
            levelManager.GetComponent<LevelManager>().InitializeWave();
        }
    }

    public void CallPause() {
        levelManager.GetComponent<PauseMenuController>().PauseGame();
    }

    public void HoverButtonSound() {
        buttonHoverSound.Play();
    }

    public void ClickButtonSound() {
        buttonClickSound.Play();
    }

}
