using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject levelCanvas;

    private void Start() {
        mainCanvas.SetActive(true);
        levelCanvas.SetActive(false);
    }

    public void StartGame() {
        Debug.Log("Game Started");
        mainCanvas.SetActive(false);
        levelCanvas.SetActive(true);
        
    }

    public void EnterLevel(string lvlNum) {
        Debug.Log(lvlNum);
        SceneManager.LoadScene("Level"+lvlNum);
    }

    public void OpenSettings() {
        Debug.Log("Settings Opened");
    }

    public void QuitGame() {
        Debug.Log("Quitting Game");
        Application.Quit();
    }

}
