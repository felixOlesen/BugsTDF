using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using UnityEngine.U2D;
using UnityEngine.UI;
using UnityEngine.Rendering.Universal;

public class LevelManager : MonoBehaviour
{

    public GameObject enemy;
    public GameObject enemy1;
    public GameObject enemy2;
    public GameObject enemy3;
    public GameObject enemy4;
    public GameObject levelPath;
    public int waveNumber;
    [SerializeField]
    private LevelStructure levelStructure;
    private List<int> enemyWaves = new List<int>() {15, 25, 20, 15, 20, 10, 0, 10, 30, 45, 30, 0, 30, 0, 120, 30, 50, 0, 50, 70, 55, 0, 0, 0, 0, 0, 40, 120, 200, 300};
    private List<int> enemy1Waves = new List<int>() {0, 0, 3, 0, 0, 10, 0, 20, 20, 15, 40, 30, 30, 0, 0, 20, 10, 40, 0, 90, 0, 45, 35, 0, 60, 0, 0, 30, 90, 100};
    private List<int> enemy2Waves = new List<int>() {0, 0, 0, 5, 0, 5, 0, 0, 17, 20, 10, 40, 0, 70, 0, 20, 10, 60, 30, 0, 90, 45, 70, 130, 0, 0, 60, 45, 75, 100};
    private List<int> enemy3Waves = new List<int>() {0, 0, 0, 0, 15, 10, 50, 0, 15, 20, 5, 0, 70, 0, 0, 20, 10, 0, 60, 0, 0, 45, 70, 0, 0, 200, 60, 150, 200, 250};
    private List<int> enemy4Waves = new List<int>() {0, 0, 0, 0, 0, 0, 0, 0, 0, 1, 0, 2, 0, 0, 3, 4, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 4, 5, 6, 10};
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
    public TMP_Text initializedWaveText;
    private float spawnDelay;

    private bool waveTimeUp;
    private bool rewardGiven;

    public AudioSource waveStartSound1;
    public AudioSource waveStartSound2;
    public AudioSource gameOverSound1;
    public AudioSource ost3;
    public AudioSource ost5;
    public AudioSource ost6;
    public AudioSource ost7;

    public Button startWaveButton;
    public Button speedUpButton;
    public Sprite buttonOnSprite;
    public Sprite buttonOffSprite;

    private bool spedUp;

    public GameObject globalLightObject;
    public Light2D globalLight;

    public int remainingHealth;
    public int bugsKilled;
    public int moneySpent;
    public int turretsPlaced;

    public bool levelComplete;


    private void Start() {
        gameOverMenu.SetActive(false);
        lvlUpMenu.SetActive(false);
        lvlCompleteMenu.SetActive(false);
        midWave = false;
        waveUI.text = "Wave 0";
        waveNumber = 0;
        spawnDelay = 0.75f;
        initializedWaveText.color = new Color(1, 1, 1, 0);

        //Performance Improvements
        QualitySettings.vSyncCount = 1;
        levelPath.GetComponent<SpriteShapeController>().BakeMesh();
        levelPath.GetComponent<SpriteShapeController>().BakeCollider();
        globalLight = globalLightObject.GetComponent<Light2D>();
        globalLight.intensity = 0.5f;
        PauseMenuController.isPaused = false;

        remainingHealth = 0;
        bugsKilled = 0;
        moneySpent = 0;
        turretsPlaced = 0;
        levelComplete = false;
    }
    
    private void Update() {
        if(currentEnemies.TrueForAll(EnemyCheck) && waveTimeUp) {
            currentEnemies.Clear();
            
            midWave = false;
            startWaveButton.gameObject.SetActive(true);
            speedUpButton.gameObject.SetActive(false);
            if(!rewardGiven) {
                if(ost5.isPlaying) {
                    StartCoroutine(FadeMusic(false, ost5));
                } else if(ost3.isPlaying) {
                    StartCoroutine(FadeMusic(false, ost3));
                } else {
                    StartCoroutine(FadeMusic(false, ost6));
                }
                StartCoroutine(FadeMusic(true, ost7));
                ChangeMoneyTotal(75 * waveNumber);
                rewardGiven = true;
                StartCoroutine(FadeDayNight("day"));
            }
            
        }
        if(!midWave && waveNumber >= levelStructure.GetTotalWaves() && !levelComplete) {
            Debug.Log("Level Complete!");

            remainingHealth = int.Parse(healthUI.text);

            levelComplete = true;
            gameObject.GetComponent<PauseMenuController>().levelComplete = true;
            lvlCompleteMenu.SetActive(true);
            gameOverMenu.SetActive(false);
            lvlUpMenu.SetActive(false);
            SaveLevelData();
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

    public void IncrementBugsKilled() {
        bugsKilled++;
    }

    public void IncrementTurretsPlaced() {
        turretsPlaced++;
    }

    public List<int> GetGameData() {
        List<int> data = new List<int>{bugsKilled, moneySpent, turretsPlaced};
        return data;
    }

    public void InitializeWave() {
        if(!midWave && !ost5.isPlaying && !ost3.isPlaying && !ost6.isPlaying) {
            startWaveButton.gameObject.SetActive(false);
            speedUpButton.gameObject.SetActive(true);
            StartCoroutine(FadeDayNight("night"));
            int waveSoundIndex = UnityEngine.Random.Range(0,2);
            if(waveSoundIndex == 1){
                waveStartSound2.Play();
            } else {
                waveStartSound1.Play();
            }
            waveNumber += 1;
            StartCoroutine(FadeMusic(false, ost7));
            if(levelStructure.GetEnemyNumberAtWave(waveNumber-1, "Boss") > 0) {
                StartCoroutine(FadeMusic(true, ost5));
            } else if(waveNumber < 10) {
                StartCoroutine(FadeMusic(true, ost3));
            } else {
                StartCoroutine(FadeMusic(true, ost6));
            }
            waveUI.text = "Wave " + waveNumber.ToString();
            StartCoroutine(SpawnEnemies());
            midWave = true;
            rewardGiven = false;
            initializedWaveText.text = waveUI.text;
            StartCoroutine(FadeImage());
            
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

    IEnumerator FadeImage() {
        initializedWaveText.gameObject.SetActive(true);
        for (float i = 0; i <= 1; i += Time.deltaTime/2) {
            // set color with i as alpha
            initializedWaveText.color = new Color(1, 1, 1, i);
            yield return null;
        }
        for (float i = 1; i >= 0; i -= Time.deltaTime/2) {
            // set color with i as alpha
            initializedWaveText.color = new Color(1, 1, 1, i);
            yield return null;
        }
        initializedWaveText.gameObject.SetActive(false);
    }

    IEnumerator FadeMusic(bool fadeIn, AudioSource ost) {
        if(fadeIn) {
            ost.Play();
            for (float i = 0; i <= 1; i += Time.deltaTime/2) {
            // set color with i as alpha
            ost.volume = i;
            yield return null;
            }
        } else {
            for (float i = 1; i >= 0; i -= Time.deltaTime/2) {
            // set color with i as alpha
            ost.volume = i;
            yield return null;
            }
            ost.Stop();
        }
    }

    IEnumerator FadeDayNight(string dayOrNight) {
        if(dayOrNight == "day"){
            for (float i = 0; i <= 0.5f; i += Time.deltaTime/2) {
                globalLight.intensity = i;
                yield return null;
            }
        }
        if(dayOrNight == "night") {
            for (float i = 0.5f; i >= 0.1f; i -= Time.deltaTime/2) {
            globalLight.intensity = i;
            yield return null;
        }
        }
        
    }

    public void ChangeGameSpeed() {
        if(spedUp) {
            Time.timeScale = 1;
            spedUp = false;
            speedUpButton.image.sprite = buttonOffSprite;
        } else {
            Time.timeScale = 1.5f;
            spedUp = true;
            speedUpButton.image.sprite = buttonOnSprite;
        }
    }

    IEnumerator SpawnEnemies() {
        int waveTime = levelStructure.GetTotalEnemyNumberAtWave(waveNumber-1);

        StartCoroutine(WaveTimer(waveTime));

        string[] waveOrder = levelStructure.GetWaveOrder(waveNumber-1);
        int basicOccurences = 0;
        int swarmOccurences = 0;
        int armourOccurences = 0;
        int stealthOccurences = 0;
        int bossOccurences = 0;

        foreach(string enemyType in waveOrder) {
            if(enemyType == "Basic") {
                basicOccurences++;
            } else if(enemyType == "Swarm") {
                swarmOccurences++;
            } else if(enemyType == "Armour") {
                armourOccurences++;
            } else if(enemyType == "Stealth") {
                stealthOccurences++;
            } else if(enemyType == "Boss") {
                bossOccurences++;
            }                   
        }

        Debug.Log("Basic: " + basicOccurences);
        Debug.Log("Swarm: " + swarmOccurences);
        Debug.Log("Armour: " + armourOccurences);
        Debug.Log("Stealth: " + stealthOccurences);
        Debug.Log("Boss: " + bossOccurences);

        foreach(string enemyType in waveOrder) {
            int nEnemy = levelStructure.GetEnemyNumberAtWave(waveNumber-1, enemyType);
            if(enemyType == "Basic") {
                StartCoroutine(EnemyCoroutine(nEnemy/basicOccurences, enemy));
                yield return new WaitForSeconds(spawnDelay * nEnemy/basicOccurences);
            } else if(enemyType == "Swarm") {
                StartCoroutine(EnemyCoroutine(nEnemy/swarmOccurences, enemy1));
                yield return new WaitForSeconds(spawnDelay * nEnemy/swarmOccurences);
            } else if(enemyType == "Armour") {
                StartCoroutine(EnemyCoroutine(nEnemy/armourOccurences, enemy2));
                yield return new WaitForSeconds(spawnDelay * nEnemy/armourOccurences);
            } else if(enemyType == "Stealth") {
                StartCoroutine(EnemyCoroutine(nEnemy/stealthOccurences, enemy3));
                yield return new WaitForSeconds(spawnDelay * nEnemy/stealthOccurences);
            } else if(enemyType == "Boss") {
                StartCoroutine(EnemyCoroutine(nEnemy/bossOccurences, enemy4));
                yield return new WaitForSeconds(spawnDelay * nEnemy/bossOccurences);
            }
            
        }

    }

    public void SwarmSpawning(int numEn, GameObject enPrefab, int checkPointInd, Vector3 checkPointPos, Vector3 pos) {
        int buffer = 12;
        StartCoroutine(WaveTimer(numEn + buffer));
        StartCoroutine(SwarmCoroutine(numEn, enPrefab, checkPointInd, checkPointPos, pos));
    }

    IEnumerator SwarmCoroutine(int numEn, GameObject enPrefab, int checkPointInd, Vector3 checkPointPos, Vector3 pos) {
        for(int i = 1; i <= numEn; i++) {
            GameObject prefab = Instantiate(enPrefab);
            prefab.GetComponent<EnemyController>().SetCurrentCheckpoint(checkPointPos, checkPointInd);
            prefab.transform.position = pos;
            currentEnemies.Add(prefab);
            yield return new WaitForSeconds(spawnDelay);
        }
    }

    IEnumerator EnemyCoroutine(int numEn, GameObject enPrefab) {
        for(int i = 1; i <= numEn; i++) {
            GameObject prefab = Instantiate(enPrefab);
            // if(waveNumber >= 10) {
            //     prefab.GetComponent<EnemyController>().speed *= (waveNumber * 0.08f);
            //     prefab.GetComponent<EnemyController>().maxHealth = Mathf.RoundToInt(prefab.GetComponent<EnemyController>().maxHealth * (waveNumber * 0.08f));
            //     prefab.GetComponent<EnemyController>().armour /= (waveNumber * 0.08f);
            // }

            currentEnemies.Add(prefab);
            yield return new WaitForSeconds(spawnDelay);
        }
    }


    public void LevelDamage(int dmg) {
        int currentHealth = Int32.Parse(healthUI.text);
        currentHealth -= dmg;
        if(currentHealth <= 0) {
            Debug.Log("Game over Trigger");
            gameOverSound1.Play();
            gameOverMenu.SetActive(true);
            gameObject.GetComponent<PauseMenuController>().SetGameOverFlag(true);
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
        if(amount < 0) {
            moneySpent -= amount;
        }
    }

    public void RetryLevel() {
        Scene scene = SceneManager.GetActiveScene();
        PauseMenuController.isPaused = false;
        Debug.Log("Retry Level Saving Game Data");
        gameObject.GetComponent<PauseMenuController>().SaveGameData();
        SceneManager.LoadScene(scene.name);
        Time.timeScale = 1f;
    }

    public void SaveLevelData() {
        Debug.Log("Saving Level Data");
        SaveSystem.SaveLevelData(this, SceneManager.GetActiveScene().name);
    }

    public void LoadLevelData() {
        Debug.Log("Loading Level Data");
        SaveSystem.LoadLevelData(SceneManager.GetActiveScene().name);
    }

}
