using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretController : MonoBehaviour
{

// Turret Stats
public bool stealthVision;
[SerializeField] bool armourPiercing;
[SerializeField] bool armourDestroying;
[SerializeField] int attackPower;
[SerializeField] float attackSpeed;
[SerializeField] float criticalHitChance; 
[SerializeField] float rangeRadius;

// Pricing
[SerializeField] int price;
int sellPrice;

// Level Up Variables
int overallTurretLevel;
[SerializeField] List<int> branchLevels;
[SerializeField] List<TurretUpgradeBranch> upgradeBranches;

// Misc Objects/Components
CircleCollider2D rangeCollider;
[SerializeField] GameObject rangeShape;
bool turretPlacement;
bool turretDeployment;

// Turret Controllers/Managers
TurretSpriteController spriteController;
TurretTargetingController targetingController;
AoeManager aoeManager;

private void Awake() {
    // Cache Relevant Controllers
    spriteController = gameObject.GetComponent<TurretSpriteController>();
    targetingController = gameObject.GetComponent<TurretTargetingController>();
    aoeManager = gameObject.GetComponent<AoeManager>();

    // Set up Turret Range
    rangeCollider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
    rangeCollider.isTrigger = true;
    //TODO: Set up updates for range-radius collider on upgrade
    rangeCollider.radius = rangeRadius;
    rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);

    //Set Up Targeting Variables
    targetingController.SetActiveWeapon(spriteController.GetActiveWeapon());
    targetingController.SetAoeManager(aoeManager);
    UpdateTargetingStats();
}


// Turret Targeting Methods
public void EnemyFound(GameObject enemy) => targetingController.AddEnemyTarget(enemy);
public void EnemyLost(GameObject enemy) => targetingController.RemoveEnemyTarget(enemy);
public void UpdateTargetingStats() {
    // Update Targeting Stat Variables on Upgrade
    targetingController.SetArmourDestroying(armourDestroying);
    targetingController.SetArmourPierce(armourPiercing);
    targetingController.SetAttackPower(attackPower);
    targetingController.SetAttackSpeed(attackSpeed);
    targetingController.SetCriticalHitChance(criticalHitChance);
}

// Sprite Methods
public void UpdateSprite(int branchNumber) {
    spriteController.LevelUpSprite(branchLevels[branchNumber]);
    targetingController.SetActiveWeapon(spriteController.GetActiveWeapon());
}
public void TurretSelected(bool selection) {
    rangeShape.GetComponent<SpriteRenderer>().enabled = selection;
}

// AOE Object Methods 
// TODO: Maybe Make an AOE Object Controller


//Getters and Setters
public void SetDeployment(bool deployment) => turretDeployment = deployment;
public void SetPlacement(bool placement) {
    SpriteRenderer rangeRenderer = rangeShape.GetComponent<SpriteRenderer>();
    if(!placement) {
        rangeRenderer.color = new Color(1f, 0f , 0f, 0.3f);
    } else {
        rangeRenderer.color = new Color(0f, 0f , 0f, 0.3f);
    }
    turretPlacement = placement;
}

public int GetPrice() {
    return price;
}
public bool GetTurretPlacement() {
    return turretPlacement;
}
public bool GetTurretDeployment() {
    return turretDeployment;
}
public GameObject GetRangeShape() {
    return rangeShape;
}

}
