using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
            if(data.remainingHealth >= 180) {
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(true);
            } else if(data.remainingHealth < 180 && data.remainingHealth >= 100) {
                star1.SetActive(true);
                star2.SetActive(true);
                star3.SetActive(false);
            } else if(data.remainingHealth < 100 && data.remainingHealth > 0) {
                star1.SetActive(true);
                star2.SetActive(false);
                star3.SetActive(false);
            }
        } else {
            star1.SetActive(false);
            star2.SetActive(false);
            star3.SetActive(false);
        }
        
    }

}
