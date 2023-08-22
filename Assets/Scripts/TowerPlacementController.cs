using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerPlacementController : MonoBehaviour
{

private GameObject buttonEntity;

private void Start() {
    buttonEntity = GameObject.Find("ButtonEntity");
}
private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Goblin Detected");
    if (other.CompareTag("Path") || other.CompareTag("Tower")){
        buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(false);
    }
}
private void OnTriggerExit2D(Collider2D other) {
    if (other.CompareTag("Path") || other.CompareTag("Tower")){
        buttonEntity.GetComponent<ButtonManager>().DetermineTowerPlacement(true);
    }
    
}
}

