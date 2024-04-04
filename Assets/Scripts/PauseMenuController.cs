using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;
using TMPro;

public class PauseMenuController : MonoBehaviour
{
    public GameObject pauseMenu;
    public GameObject settingsMenu;
    public static bool isPaused;
    public AudioMixer audioMixer;
    Resolution[] resolutions;
    public TMP_Dropdown resolutionDropdown;
    public bool gameOver;
    public bool levelComplete;
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();
        gameOver = false;
        levelComplete = false;

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;
        for (int i =0; i < resolutions.Length; i++) {
            string option = resolutions[i].width + " X " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height) {
                currentResolutionIndex = i;
            }
        }
        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();
    }

    // Update is called once per frame
    void Update() {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
        
    }

    public void SetGameOverFlag(bool over) {
        gameOver = over;
    }

    public void PauseGame() {
        levelComplete = gameObject.GetComponent<LevelManager>().levelComplete;
        gameOver = gameObject.GetComponent<LevelManager>().gameOverFlag;
        if(!levelComplete && !gameOver) {
            gameObject.GetComponent<LevelManager>().turretInfoMenu.SetActive(false);
            gameObject.GetComponent<LevelManager>().bugInfoMenu.SetActive(false);
            pauseMenu.SetActive(true);
            Time.timeScale = 0f;
            isPaused = true;
        }
        
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu() {
        if(!gameOver && levelComplete) {
            LevelData data = SaveSystem.LoadLevelData(SceneManager.GetActiveScene().name);
            int currentRemainingHealth = gameObject.GetComponent<LevelManager>().remainingHealth;
            if(data == null) {
                gameObject.GetComponent<LevelManager>().SaveLevelData();
            } else if(data.remainingHealth < currentRemainingHealth) {
                gameObject.GetComponent<LevelManager>().SaveLevelData();
            } else {
                Debug.Log("Score lower than best saved score");
            }
        }
        SaveGameData();
        SceneManager.LoadScene("MainMenu");
        isPaused = false;

    }

    public void SettingsMenu() {
        settingsMenu.SetActive(true);
        pauseMenu.SetActive(false);
    }

    public void SettingsMenuBack() {
        settingsMenu.SetActive(false);
        pauseMenu.SetActive(true);
    }

    public void QuitGame() {
        if(!gameOver && levelComplete) {
            LevelData data = SaveSystem.LoadLevelData(SceneManager.GetActiveScene().name);
            int currentRemainingHealth = gameObject.GetComponent<LevelManager>().remainingHealth;
            Debug.Log("Current Remaining Health: " + currentRemainingHealth);
            Debug.Log("Previously Saved Remaining Health: " + currentRemainingHealth);
            if(data == null) {
                gameObject.GetComponent<LevelManager>().SaveLevelData();
            } else if(data.remainingHealth < currentRemainingHealth) {
                gameObject.GetComponent<LevelManager>().SaveLevelData();
            } else {
                Debug.Log("Score lower than best saved score");
            }

            
        }
        SaveGameData();
        isPaused = false;
        Application.Quit();
    }

    public void SetVolume(float vol) {
        audioMixer.SetFloat("Volume", vol);
    }

    public void SetQuality(int qualityIndex) {
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetFullscreen(bool isFullscreen) {
        Screen.fullScreen = isFullscreen;
    }

    public void SetResolution(int resolutionIndex) {
        Resolution resolution = resolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
    }

    public void SaveGameData() {
        List<int> data = gameObject.GetComponent<LevelManager>().GetGameData();
        gameObject.GetComponent<GameDataController>().AddBugsKilled(data[0]);
        gameObject.GetComponent<GameDataController>().AddMoneySpent(data[1]);
        gameObject.GetComponent<GameDataController>().AddTurretsPlaced(data[2]);
        gameObject.GetComponent<GameDataController>().SaveGameData();
    }

}
