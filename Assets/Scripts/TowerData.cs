using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "TowerData")]
public class TowerData : ScriptableObject {

    public string buffIdx;
    public float magnitude;
    public string description;
    public string upgradeName;
    public int upgradeCost;
    public Sprite upgradeIcon;

}
