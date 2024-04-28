using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretInputHandler : MonoBehaviour
{
    TurretController turretController; 
    
    private void Awake() {
        turretController = gameObject.GetComponent<TurretController>();
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if(other.CompareTag("Enemy")) {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if((enemy.stealthy && turretController.stealthVision) || !enemy.stealthy) {
                turretController.EnemyFound(other.gameObject);
            } 
        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        if(other.CompareTag("Enemy")) {
            EnemyController enemy = other.gameObject.GetComponent<EnemyController>();

            if((enemy.stealthy && turretController.stealthVision) || !enemy.stealthy) {
                turretController.EnemyLost(other.gameObject);
            } 
        }
    }
}
