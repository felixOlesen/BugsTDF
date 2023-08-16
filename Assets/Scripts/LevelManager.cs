using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{

    public GameObject goblin;
    public GameObject knight;
    public GameObject levelPath;
    public int waveNumber;
    private List<int> goblinWaves = new List<int>() {3, 5, 7, 8, 10};
    private List<int> knightWaves = new List<int>() {0, 0, 2, 4, 6};

    public void InitializeWave() {
        Debug.Log("Starting Wave: " + waveNumber);
        SpawnEnemies();
    }

    private void SpawnEnemies() {
        int nGoblins = goblinWaves[waveNumber-1];
        Debug.Log("Number of Goblins Spawned: " + nGoblins);
        StartCoroutine(GoblinCoroutine(nGoblins));

        int nKnights = knightWaves[waveNumber-1];
        Debug.Log("Number of Goblins Spawned: " + nKnights);
        StartCoroutine(GoblinCoroutine(nKnights));

    }

    IEnumerator GoblinCoroutine(int numGob) {
        for(int i = 1; i <= numGob; i++) {
            Instantiate(goblin);
            yield return new WaitForSeconds(0.5f);
        }
    }

    IEnumerator KnightCoroutine(int numKni) {
        for(int i = 1; i <= numKni; i++) {
            Instantiate(knight);
            yield return new WaitForSeconds(0.5f);
        }
    }

    public void IncrementWave() {
        waveNumber += 1;
    }

}
