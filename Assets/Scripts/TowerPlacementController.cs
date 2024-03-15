using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{

private GameObject buttonEntity;
private List <GameObject> currentTowerCollisions = new List <GameObject> ();

private void Start() {
    buttonEntity = GameObject.Find("ButtonEntity");
}
private void OnTriggerEnter2D(Collider2D other) {
    if (other.CompareTag("Path")){
        buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(false, "path");
    } else if (other.CompareTag("Tower")){
        buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(false, "tower");
        currentTowerCollisions.Add(other.gameObject);
        
    }
}
private void OnTriggerExit2D(Collider2D other) {
    if (other && buttonEntity && other.CompareTag("Path")){
        buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(true, "path");
    } 
    if (other && buttonEntity && other.CompareTag("Tower") && other.transform.parent.GetComponent<TowerController>().placed){
        currentTowerCollisions.Remove(other.gameObject);
        if(currentTowerCollisions.Count == 0) {
            buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(true , "tower");
        }
    }
    
}
}

