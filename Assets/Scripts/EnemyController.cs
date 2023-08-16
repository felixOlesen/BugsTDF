using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;
using UnityEngine.UI;
public class EnemyController : MonoBehaviour
{
public GameObject path;
private SpriteShapeController pathController;
private Spline pathSpline;

[SerializeField]
private float speed;

[SerializeField]
private int currentHealth;
public int maxHealth = 100;
public HealthBar healthBar;

private Vector3 currentCheckpointPos;
private int currentCheckpointIndex;

private void Start() {
    path = GameObject.Find("WoodenPath");
    pathController = path.GetComponent<SpriteShapeController>();
    pathSpline = pathController.spline;
    speed = 2;
    // Debug.Log(pathSpline.GetPointCount());
    // Debug.Log(pathSpline.GetPosition(2));
    transform.position = pathSpline.GetPosition(0);
    currentCheckpointIndex = 1;
    currentCheckpointPos = pathSpline.GetPosition(currentCheckpointIndex);

    currentHealth = maxHealth;
    healthBar.SetMaxHealth(maxHealth);
    
}

private void Update() {
    currentCheckpointPos = UpdateCheckpoint();
    transform.position = Vector3.MoveTowards(transform.position, currentCheckpointPos, speed * Time.deltaTime);
}

private Vector3 UpdateCheckpoint() {
    if(currentCheckpointPos == transform.position) {
        if(currentCheckpointIndex < pathSpline.GetPointCount() && currentCheckpointIndex < pathSpline.GetPointCount()) {
            currentCheckpointPos = pathSpline.GetPosition(currentCheckpointIndex);
            currentCheckpointIndex += 1;
            // Debug.Log(currentCheckpointIndex);
        }
        if(currentCheckpointIndex == pathSpline.GetPointCount()) {
            Destroy(gameObject);
        }
    }
    return currentCheckpointPos;
    
}

public void TakeDamage(int damage) {
    currentHealth -= damage;

    healthBar.SetHealth(currentHealth);

    if(currentHealth <= 0) {
        Destroy(gameObject);
    }

}


}
