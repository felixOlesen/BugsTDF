using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretTargetingController : MonoBehaviour
{

// Bullet Prefab
[SerializeField] GameObject projectile;

// Turret Stats
[SerializeField] float criticalHitScalar;
int attackPower;
float attackSpeed;
float criticalHitChance;
bool armourPierce;
bool armourDestroying;

// Firing Logic
bool alternatingFire;
bool firing;
[SerializeField] float projectileDestroyTime;
Coroutine firingCoroutine;
List<GameObject> targetList;

// Bullet Instantiation Varaibles
List<GameObject> firingPoints;
GameObject activeWeapon;
AoeManager aoeManager;

private void Awake() {
    targetList = new List<GameObject>();
}

public void AddEnemyTarget(GameObject enemy) => targetList.Add(enemy);
public void RemoveEnemyTarget(GameObject enemy) => targetList.Remove(enemy);
public void SetAttackSpeed(float speed) => attackSpeed = speed;
public void SetCriticalHitChance(float chance) => criticalHitChance = chance;
public void SetAttackPower(int power) => attackPower = power;
public void SetArmourPierce(bool pierce) => armourPierce = pierce;
public void SetArmourDestroying(bool destroying) => armourDestroying = destroying;
public void SetAoeManager(AoeManager manager) => aoeManager = manager;
public void SetActiveWeapon(GameObject weapon) {
    activeWeapon = weapon;
    ExtractFiringPoints();
} 

private void ExtractFiringPoints() {
    firingPoints = activeWeapon.GetComponent<TurretWeaponController>().firingPoints;
}
private void Update() {
    if(targetList.Count > 0) {
        // Clear null target objects in the list
        if(!firing) {
            targetList.RemoveAll(target => target == null);
        }

        // If list is still not empty LockOn
        if(targetList.Count > 0) {
            LockOn(targetList[0]);
        }

        if(firingCoroutine == null) {
            firing = true;
            firingCoroutine = StartCoroutine(Fire());
        }
    }
    
}

IEnumerator Fire() {
    // TODO: Implement alternating fire method for multi-barrel weapons
    Vector3 shootDir = targetList[0].transform.position - transform.position;
    GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
    bool criticalHit = CriticalHitCheck();
    int calculatedAttackPower = criticalHit ? Mathf.RoundToInt(attackPower * criticalHitScalar) : attackPower;

    tempProjectile.GetComponent<ProjectileController>().shot(calculatedAttackPower, shootDir, armourPierce, armourDestroying, aoeManager, criticalHit);
    Destroy(tempProjectile, projectileDestroyTime);
    
    yield return new WaitForSeconds(attackSpeed);
    firingCoroutine = null;
    firing = false;
}

private void LockOn(GameObject target) {
    Vector3 offset = (target.transform.position - activeWeapon.transform.position).normalized;
    activeWeapon.transform.rotation = Quaternion.LookRotation(
    Vector3.forward, // Keep z+ pointing straight into the screen.
    offset           // Point y+ toward the target.
    ); 
}

private bool CriticalHitCheck() {
    float randomValue = Random.Range(0f, 1f);
    bool criticalHit = (randomValue < criticalHitChance) ? true : false;
    return criticalHit;
}

}
