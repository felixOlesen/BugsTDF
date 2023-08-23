using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;

public class LevelManager : MonoBehaviour
{

    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject levelPath;
    public int waveNumber;
    private List<int> enemyWaves = new List<int>() {3, 5, 7, 8, 10, 13, 16, 20, 30, 45};
    private List<int> enemy1Waves = new List<int>() {0, 0, 2, 4, 6, 10, 13, 17, 20, 30};
    private List<int> enemy2Waves = new List<int>() {0, 0, 0, 3, 5, 7, 9, 13, 17, 20};
    private List<int> enemy3Waves = new List<int>() {0, 0, 0, 0, 2, 5, 7, 10, 15, 19};
    private List<int> enemy4Waves = new List<int>() {0, 0, 0, 0, 3, 7, 9, 11, 14, 16};
    private int totalHealth;
    public TMP_Text healthUI;
    private int totalMoney;
    public TMP_Text moneyUI;
    
    public void InitializeWave() {
        Debug.Log("Starting Wave: " + waveNumber);
        SpawnEnemies();
    }

    private void SpawnEnemies() {
        int nEnemy = enemyWaves[waveNumber-1];
        Debug.Log("Number of Goblins Spawned: " + nEnemy);
        StartCoroutine(EnemyCoroutine(nEnemy, enemy));

        int nEnemy1 = enemy1Waves[waveNumber-1];
        Debug.Log("Number of Mushrooms Spawned: " + nEnemy1);
        StartCoroutine(EnemyCoroutine(nEnemy1, enemy1));

        int nEnemy2 = enemy2Waves[waveNumber-1];
        Debug.Log("Number of Mushrooms Spawned: " + nEnemy2);
        StartCoroutine(EnemyCoroutine(nEnemy2, enemy2));

        int nEnemy3 = enemy3Waves[waveNumber-1];
        Debug.Log("Number of Mushrooms Spawned: " + nEnemy3);
        StartCoroutine(EnemyCoroutine(nEnemy3, enemy3));

        int nEnemy4 = enemy4Waves[waveNumber-1];
        Debug.Log("Number of Mushrooms Spawned: " + nEnemy4);
        StartCoroutine(EnemyCoroutine(nEnemy4, enemy4));

    }

    IEnumerator EnemyCoroutine(int numEn, GameObject enPrefab) {
        for(int i = 1; i <= numEn; i++) {
            Instantiate(enPrefab);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void IncrementWave() {
        waveNumber += 1;
    }

    public void LevelDamage(int dmg) {
        int currentHealth = Int32.Parse(healthUI.text);
        currentHealth -= dmg;
        healthUI.SetText(currentHealth.ToString());

    }

    public bool CheckMoneyTotal(int amount) {
        int currentMoney = Int32.Parse(moneyUI.text);
        currentMoney += amount;
        bool check = false;
        if(currentMoney < 0) {
            check = false;
        } else {
            check = true;
        }
        return check;
    }

    public void ChangeMoneyTotal(int amount) {
        int currentMoney = Int32.Parse(moneyUI.text);
        currentMoney += amount;
        moneyUI.SetText(currentMoney.ToString());
    }

}
