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
public bool swarmHost;
public GameObject swarmChild;
public bool isSwarmChild;
private CircleCollider2D aoeRange;
public GameObject aoeObject;
public float aoeScalar = 1.0f;
public bool stunned;

private void Start() {
    path = GameObject.Find("WoodenPath");
    pathController = path.GetComponent<SpriteShapeController>();
    pathSpline = pathController.spline;
    levelManager = GameObject.Find("LevelManager");
    aoeRange = aoeObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
    aoeRange.isTrigger = true;
    aoeRange.radius = 0.0f;
    if(!isSwarmChild) {
        transform.position = pathSpline.GetPosition(0);
        currentCheckpointIndex = 1;
        currentCheckpointPos = pathSpline.GetPosition(currentCheckpointIndex);
    }
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

IEnumerator UpdateAoeRange(float range, float duration, float scalar) {
    aoeRange.radius = range;
    aoeScalar = scalar;
    yield return new WaitForSeconds(duration);
    aoeRange.radius = 0.0f;
    aoeScalar = 1;
}

public void SpawnSwarm() {
    for(int i = 0; i < 10; i++) {
        GameObject tempEnemy = Instantiate(swarmChild);
        tempEnemy.GetComponent<EnemyController>().SetCurrentCheckpoint(currentCheckpointPos, currentCheckpointIndex);
        tempEnemy.transform.position = transform.position;
    }
}

public void SetCurrentCheckpoint(Vector3 pos, int ind) {
    this.currentCheckpointPos = pos;
    this.currentCheckpointIndex = ind;
}


public void TakeDamage(int damage, bool pierce, bool armourDestroying, string aoeType, float aoeRadius, float stunDuration, float aoeScalar) {
    if(!pierce){
        damage = Mathf.RoundToInt((float)damage * armour);
    }
    if(armourDestroying) {
        armour = 1;
    }
    if(aoeType == "stun" || aoeType == "explosive" ) {
        aoeObject.tag = aoeType;
        StartCoroutine(UpdateAoeRange(aoeRadius, stunDuration, aoeScalar));
    }
    currentHealth -= damage;
    healthBar.SetHealth(currentHealth);
    if(currentHealth <= 0) {
        if(swarmHost) {
            int nEnemy = 10;
            levelManager.GetComponent<LevelManager>().SwarmSpawning(nEnemy, swarmChild, currentCheckpointIndex, currentCheckpointPos, transform.position);
        }
        Destroy(gameObject);
        levelManager.GetComponent<LevelManager>().ChangeMoneyTotal(moneyReward);
    }
}

private void OnTriggerEnter2D(Collider2D other) {
    if(other.CompareTag("stun")) {
        if(!stunned) {
            Debug.Log(other);
            speed *= other.transform.parent.gameObject.GetComponent<EnemyController>().aoeScalar;
            stunned = true;
        }
    } else if(other.CompareTag("explosive")) {
        float aoeDamage = 100 * aoeScalar;
        TakeDamage( Mathf.RoundToInt(aoeDamage), false, false, "Untagged", 0.0f, 0.0f, 0.0f);
    }
}
private void OnTriggerExit2D(Collider2D other) {
    if(other.CompareTag("stun")) {
        speed /= other.transform.parent.gameObject.GetComponent<EnemyController>().aoeScalar;
        stunned = false;
    }
}
}
