using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement; 
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class MainMenuButtons : MonoBehaviour
{
    public GameObject mainCanvas;
    public GameObject levelCanvas;
    public GameObject settingsMenu;
    Resolution[] resolutions;
    public AudioMixer audioMixer;

    public TMP_Dropdown resolutionDropdown;

    public AudioSource buttonClickSound;

    public GameObject levelLoader;

    private void Start() {
        mainCanvas.SetActive(true);
        levelCanvas.SetActive(false);
        settingsMenu.SetActive(false);
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
        Time.timeScale = 0.2f;
        

    }

    public void StartGame() {
        Debug.Log("Game Started");
        mainCanvas.SetActive(false);
        levelCanvas.SetActive(true);
        
    }

    public void EnterLevel(string lvlNum) {
        Debug.Log(lvlNum);
        Time.timeScale = 1f;
        levelLoader.GetComponent<SceneTransitionManager>().EnterLevel(lvlNum); 
    }

    public void Back() {
        levelCanvas.SetActive(false);
        settingsMenu.SetActive(false);
        mainCanvas.SetActive(true);
    }

    public void OpenSettings() {
        Debug.Log("Settings Opened");
        mainCanvas.SetActive(false);
        settingsMenu.SetActive(true);
    }

    public void QuitGame() {
        Debug.Log("Quitting Game");
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

    public void ButtonClick() {
        buttonClickSound.Play();
    }


}
