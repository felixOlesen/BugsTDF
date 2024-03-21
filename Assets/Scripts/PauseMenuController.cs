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
    
    // Start is called before the first frame update
    void Start()
    {
        pauseMenu.SetActive(false);
        resolutions = Screen.resolutions;
        resolutionDropdown.ClearOptions();

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
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Escape)) {
            PauseGame();
        }
        
    }

    public void PauseGame() {
        pauseMenu.SetActive(true);
        Time.timeScale = 0f;
        isPaused = true;
    }

    public void ResumeGame() {
        pauseMenu.SetActive(false);
        Time.timeScale = 1f;
        isPaused = false;
    }

    public void MainMenu() {
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
        Debug.Log("Quitting Game");
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
}
