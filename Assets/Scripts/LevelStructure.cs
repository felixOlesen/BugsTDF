using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LevelStructure")]
public class LevelStructure : ScriptableObject
{
    [SerializeField] 
    private List<int> enemyWaves;
    [SerializeField] 
    private List<int> enemy1Waves;
    [SerializeField] 
    private List<int> enemy2Waves;
    [SerializeField] 
    private List<int> enemy3Waves;
    [SerializeField] 
    private List<int> enemy4Waves;
    [SerializeField] 
    private List<string> levelOrder;
    [SerializeField] 
    private int totalWaves;

    public string[] GetWaveOrder(int waveNum) {
        string[] order = levelOrder[waveNum].Split(",");
        return order;
    }
   
   public int GetEnemyNumberAtWave(int waveNum, string enemyName) {
    int enemyNumber = 0;

    if(enemyName == "Basic") {
       enemyNumber = enemyWaves[waveNum];
    } else if(enemyName == "Swarm") {
        enemyNumber = enemy1Waves[waveNum];
    } else if(enemyName == "Armour") {
        enemyNumber = enemy2Waves[waveNum];
    } else if(enemyName == "Stealth") {
        enemyNumber = enemy3Waves[waveNum];
    } else if(enemyName == "Boss") {
        enemyNumber = enemy4Waves[waveNum];
    }
    return enemyNumber;
   }

   public int GetTotalEnemyNumberAtWave(int waveNum) {
    int totalEnemies = 0;
    totalEnemies += enemyWaves[waveNum];
    totalEnemies += enemy1Waves[waveNum];
    totalEnemies += enemy2Waves[waveNum];
    totalEnemies += enemy3Waves[waveNum];
    totalEnemies += enemy4Waves[waveNum];
    return totalEnemies;
   }
   public int GetTotalWaves() {
    return totalWaves;
   }

   
}
