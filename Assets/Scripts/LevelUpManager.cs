using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelUpManager : MonoBehaviour
{
public List<Queue<TowerData>> lvlTree;

public void DisplayOptions(GameObject tower) {
    lvlTree = tower.GetComponent<TowerController>().GetLevelTree();
    Debug.Log(lvlTree[0].Peek().name);
}

}
