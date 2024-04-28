using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "UpgradeBranch")]
public class TurretUpgradeBranch : ScriptableObject
{
public string branchName;
public List<string> upgradeNames;
public List<string> upgradeDescriptions;
public List<string> upgradeIndexes;
public List<float> upgradeMagnitude;
public List<int> upgradeCosts;
public List<Sprite> upgradeIcons;

}
