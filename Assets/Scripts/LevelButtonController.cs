using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelButtonController : MonoBehaviour
{
    public int levelNumber;
    public GameObject star1;
    public GameObject star2;
    public GameObject star3;

    void Start()
    {
        LevelData data = SaveSystem.LoadLevelData("Level" + levelNumber);
        if(data != null) {
            star1.SetActive(true);
            star2.SetActive(true);
            star3.SetActive(true);
            gameObject.GetComponent<Image>().color = new Color(0, 1, 0, 1);
            Debug.Log(data.remainingHealth);
            if(data.remainingHealth >= 180) {
                star1.GetComponent<Image>().color = new Color(255,255,255,255);
                star2.GetComponent<Image>().color = new Color(255,255,255,255);
                star3.GetComponent<Image>().color = new Color(255,255,255,255);
            } else if(data.remainingHealth < 180 && data.remainingHealth >= 100) {
                star1.GetComponent<Image>().color = new Color(255,255,255,255);
                star2.GetComponent<Image>().color = new Color(255,255,255,255);
                star3.GetComponent<Image>().color = new Color(0,0,0,255);
            } else if(data.remainingHealth < 100 && data.remainingHealth > 0) {
                star1.GetComponent<Image>().color = new Color(255,255,255,255);
                star2.GetComponent<Image>().color = new Color(0,0,0,255);
                star3.GetComponent<Image>().color = new Color(0,0,0,255);
            }
        } else {
            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
            gameObject.GetComponent<Image>().color = new Color(1, 1, 1, 1);
        }
        
    }

}
