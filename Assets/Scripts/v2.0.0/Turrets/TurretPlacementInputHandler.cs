using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretPlacementInputHandler : MonoBehaviour
{
TurretController turretController;
List <GameObject> currentTurretCollisions = new List <GameObject> ();
List <GameObject> currentEnvCollisions = new List <GameObject> ();
bool pathPlacement;
bool turretPlacement;
bool envObjectPlacement;
bool placement;

private void Awake() {
    turretController = transform.parent.GetComponent<TurretController>();

    // Enable Placement Until Proven Otherwise
    pathPlacement = true;
    turretPlacement = true;
    envObjectPlacement = true;
}

private void UpdateTurretPlacement() {
        placement = false;
        if(pathPlacement && turretPlacement && envObjectPlacement) {
            placement = true;
        }
        // Debug.Log("Path Placement: " + pathPlacement);
        // Debug.Log("Turret Placement: " + turretPlacement);
        // Debug.Log("Env Placement: " + envObjectPlacement);
        turretController.SetPlacement(placement);
}

private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Path")){
        pathPlacement = false;
        UpdateTurretPlacement();
    } else if (other.CompareTag("Tower") && other.transform.parent.GetComponent<TurretController>().GetTurretDeployment()){
        turretPlacement = false;
        UpdateTurretPlacement();
        currentTurretCollisions.Add(other.gameObject);
        
    } else if (other.CompareTag("UnplaceableGrid")){
        envObjectPlacement = false;
        UpdateTurretPlacement();
        currentEnvCollisions.Add(other.gameObject);
    }
}
private void OnTriggerExit2D(Collider2D other) {
    if (other && other.CompareTag("Path")){
        pathPlacement = true;
        UpdateTurretPlacement();
    } 
    if (other && other.CompareTag("Tower") && other.transform.parent.GetComponent<TurretController>().GetTurretDeployment()){
        currentTurretCollisions.Remove(other.gameObject);
        if(currentTurretCollisions.Count == 0) {
            turretPlacement = true;
            UpdateTurretPlacement();
        }
    }
    if (other && other.CompareTag("UnplaceableGrid")){
        currentEnvCollisions.Remove(other.gameObject);
        if(currentEnvCollisions.Count == 0) {
            envObjectPlacement = true;
            UpdateTurretPlacement();
        }
    }
    
}
}
