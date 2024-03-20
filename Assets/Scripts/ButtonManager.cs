using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonManager : MonoBehaviour
{
    public GameObject tower1Button;
    public GameObject tower2Button;
    public GameObject tower3Button;
    public GameObject tower4Button;
    public GameObject tower2PreFab;
    public GameObject tower1PreFab;
    public GameObject tower3PreFab;
    public GameObject tower4PreFab;
    public GameObject levelManager;
    private Vector3 mousePos;
    private GameObject currentTower;
    private bool towerHeld;
    private IDictionary<string, GameObject> towers;
    private bool goodPlacement;
    private bool goodPlacementTowers;
    private bool goodPlacementEnvObjects;
    private bool goodPlacementPath;
    public GameObject cancelText;
    public AudioSource buttonHoverSound;
    public AudioSource buttonClickSound;
    public GameObject muteSymbol;
    public bool muted;
    public TMP_Text t1Price;
    public TMP_Text t2Price;
    public TMP_Text t3Price;
    public TMP_Text t4Price;

    private void Start() {
        towers = new Dictionary<string, GameObject>(){
            {"tower2", tower2PreFab},
            {"tower1", tower1PreFab},
            {"tower3", tower3PreFab},
            {"tower4", tower4PreFab}
        };
        goodPlacement = true;
        goodPlacementTowers = true;
        goodPlacementPath = true;
        goodPlacementEnvObjects = true;
        muted = false;
        muteSymbol.SetActive(false);
        t1Price.overrideColorTags = true;
        t2Price.overrideColorTags = true;
        t3Price.overrideColorTags = true;
        t4Price.overrideColorTags = true;
    }

    void Update() {
        mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = Camera.main.transform.position.z + Camera.main.nearClipPlane;
        
        if(towerHeld){
            currentTower.transform.position = mousePos;
            tower1Button.GetComponent<Button>().interactable = false;
            tower2Button.GetComponent<Button>().interactable = false;
            tower3Button.GetComponent<Button>().interactable = false;
            tower4Button.GetComponent<Button>().interactable = false;
        } else {
            tower1Button.GetComponent<Button>().interactable = true;
            tower2Button.GetComponent<Button>().interactable = true;
            tower3Button.GetComponent<Button>().interactable = true;
            tower4Button.GetComponent<Button>().interactable = true;
        }
        if(!PauseMenuController.isPaused) {
            if(Input.GetMouseButtonDown(0) && (towerHeld && goodPlacement && !EventSystem.current.IsPointerOverGameObject())){
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
                if (hit.collider != null && hit.collider.CompareTag("Tower") && goodPlacement && !EventSystem.current.IsPointerOverGameObject()) {
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

            CheckTextColorChange(t1Price);
            CheckTextColorChange(t2Price);
            CheckTextColorChange(t3Price);
            CheckTextColorChange(t4Price);
        }
    }

    public void CheckTextColorChange(TMP_Text priceText) {
        if(levelManager.GetComponent<LevelManager>().CheckMoneyTotal(-int.Parse(priceText.text.Replace("$", "")))) {
            priceText.color = new Color32(205, 250, 252, 255);
        } else {
            priceText.color = new Color32(104, 104, 104, 255);
        }
    }

    public void DetermineTowerPlacement(bool clear, string clearanceType) {
        if(clearanceType == "path") {
            goodPlacementPath = clear;
        } else if(clearanceType == "tower") {
            goodPlacementTowers = clear;
        } else if(clearanceType == "envObject") {
            goodPlacementEnvObjects = clear;
        }
        Debug.Log("Path: " + goodPlacementPath);
        Debug.Log("Towers: " + goodPlacementTowers);
        Debug.Log("EnvObjects: " + goodPlacementEnvObjects);
        if(goodPlacementPath && goodPlacementTowers && goodPlacementEnvObjects) {
            goodPlacement = true;
        } else {
            goodPlacement = false;
        }
        Debug.Log("Overall Placement: " + goodPlacement);
        
        if(currentTower != null && !goodPlacement) {
            currentTower.transform.GetChild(0).GetComponent<SpriteRenderer>().color = new Color(1f, 0f , 0f, 0.3f);
        } else if(currentTower != null) {
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
                ClickButtonSound();
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

    public void MuteButton() {
        if(muted) {
            AudioListener.volume = 1;
            muted = false;
            muteSymbol.SetActive(false);
        } else {
            AudioListener.volume = 0;
            muted = true;
            muteSymbol.SetActive(true);
        }
    }

}
