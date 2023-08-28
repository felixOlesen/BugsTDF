using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
public List<Queue<TowerData>> lvlTree;
private GameObject currentTower;

public void DisplayOptions(GameObject tower) {
    lvlTree = tower.GetComponent<TowerController>().GetLevelTree();
    currentTower = tower;
    Debug.Log(lvlTree[0].Peek().name);
    /*
        1. Get Level informations from Queue.Peek
        2. Change button text areas to show the level up options for the specific tower
        3. SetActive to the canvas
    */
}

public void LevelUp(int branch) {
    /*
        1. Method called by onclick button
        2. Requires int input from button referring to specific queue branch
        3. Peeks at the queue and fetches the lvl up details
        4. Calls ImproveStats() method
        5. Dequeue the specified branch
        6. Refreshes the Displayed options
    */
}

public void ImproveStats() {
    /*
        1. Check the buff IDX (could be related to dictionary)
        2. Improve tower stat by a magnitude param 
    */
}


}
