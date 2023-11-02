using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

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
    private bool midWave = false;
    private List<GameObject> currentEnemies = new List<GameObject>();
    public GameObject gameOverMenu;
    public GameObject lvlUpMenu;
    public GameObject lvlCompleteMenu;
    public TMP_Text waveUI;
    private float spawnDelay;

    private bool waveTimeUp;
    private void Start() {
        gameOverMenu.SetActive(false);
        lvlUpMenu.SetActive(false);
        lvlCompleteMenu.SetActive(false);
        midWave = false;
        waveUI.text = "Wave: 1";
        waveNumber = 0;
        spawnDelay = 0.2f;
    }
    
    private void Update() {
        if(currentEnemies.TrueForAll(EnemyCheck) && waveTimeUp) {
            currentEnemies.Clear();
            midWave = false;
        }
        if(!midWave && waveNumber >= 10) {
            Debug.Log("Level Complete!");
            lvlCompleteMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            lvlUpMenu.SetActive(false);
        }
    }

    private bool EnemyCheck(GameObject item) {
        bool check;
        if(item == null) {
            check = true;
        } else {
            check = false;
        }
        return check;
    }
    public void InitializeWave() {
        if(!midWave) {
            waveNumber += 1;
            waveUI.text = "Wave: " + waveNumber.ToString();
            // Debug.Log("Starting Wave: " + waveNumber);
            SpawnEnemies();
            midWave = true;
        } else {
            // Debug.Log("Wave already started");
        }
    }

    IEnumerator WaveTimer(int numEn) {
        waveTimeUp = false;
        float totalWaveTime = spawnDelay * numEn;
        // Debug.Log("Timer Started: " + totalWaveTime );
        yield return new WaitForSeconds(totalWaveTime);
        // Debug.Log("Wave Finished");
        waveTimeUp = true;
    }

    private void SpawnEnemies() {
        //implement wave waiting feature for the entire wave based off the spawning time as a constant
        int nEnemy = enemyWaves[waveNumber-1];

        StartCoroutine(WaveTimer(nEnemy));
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
            GameObject prefab = Instantiate(enPrefab);
            currentEnemies.Add(prefab);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    public void LevelDamage(int dmg) {
        int currentHealth = Int32.Parse(healthUI.text);
        currentHealth -= dmg;
        if(currentHealth <= 0) {
            Debug.Log("Game over Trigger");
            gameOverMenu.SetActive(true);
            Time.timeScale = 0f;
            PauseMenuController.isPaused = true;
        }
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

    public void RetryLevel() {
        Scene scene = SceneManager.GetActiveScene();
        PauseMenuController.isPaused = false;
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }

}
