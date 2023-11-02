using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
public GameObject path;
public GameObject levelManager;
private SpriteShapeController pathController;
private Spline pathSpline;

[SerializeField]
private float speed;

[SerializeField]
private int currentHealth;
[SerializeField]
private int attackDamage = 10;
public int maxHealth = 100;
public HealthBar healthBar;
private Vector3 currentCheckpointPos;
private int currentCheckpointIndex;
public float armour;
public int moneyReward;
public bool stealthy;
private float timeCount = 0.0f;

private void Start() {
    path = GameObject.Find("WoodenPath");
    pathController = path.GetComponent<SpriteShapeController>();
    pathSpline = pathController.spline;
    levelManager = GameObject.Find("LevelManager");

    transform.position = pathSpline.GetPosition(0);
    currentCheckpointIndex = 1;
    currentCheckpointPos = pathSpline.GetPosition(currentCheckpointIndex);

    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    
}

private void Update() {
    currentCheckpointPos = UpdateCheckpoint();
    Aim(currentCheckpointPos);
    transform.position = Vector3.MoveTowards(transform.position, currentCheckpointPos, speed * Time.deltaTime);
}

private void Aim(Vector3 targetPos) {
    

    if(targetPos != null) {
        Vector3 dir = (targetPos - transform.position).normalized;


        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Euler( 0, 0, angle - 90.0f);
    }
}

private Vector3 UpdateCheckpoint() {
    if(currentCheckpointPos == transform.position) {
        if(currentCheckpointIndex < pathSpline.GetPointCount() && currentCheckpointIndex < pathSpline.GetPointCount()) {
            currentCheckpointPos = pathSpline.GetPosition(currentCheckpointIndex);
            currentCheckpointIndex += 1;
        }
        if(currentCheckpointIndex == pathSpline.GetPointCount()) {
            Destroy(gameObject);
            levelManager.GetComponent<LevelManager>().LevelDamage(attackDamage);
            levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(Mathf.RoundToInt(moneyReward * 0.75f));
        }
    }
    return currentCheckpointPos;
}

public void TakeDamage(int damage, bool pierce, bool armourDestroying) {
    if(!pierce){
        damage = Mathf.RoundToInt((float)damage * armour);
    }
    if(armourDestroying) {
        armour = 1;
    }
    currentHealth -= damage;
    healthBar.SetHealth(currentHealth);
    if(currentHealth <= 0) {
        Destroy(gameObject);
        levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(moneyReward);
    }
}


}
