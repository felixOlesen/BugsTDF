using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Start is called before the first frame update

    public int attackPower = 10;
    public int rangeRadius;
    public float attackSpeed;
    private CircleCollider2D towerRange;
    private GameObject currentTarget;
    private Queue<GameObject> enemyQueue;

    [SerializeField]
    private GameObject projectile;
    private Coroutine currentCoroutine;

    public bool isSelected;
    private GameObject rangeShape;

    private void Start() {
        towerRange = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        towerRange.isTrigger = true;
        enemyQueue = new Queue<GameObject>();
        rangeShape = transform.GetChild(0).gameObject;
        rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);
        isSelected = true;
        
    }

    private void Update() {
        towerRange.radius = rangeRadius;
        if(enemyQueue.Count > 0) {
            // LockOn(enemyQueue.Peek());
            if(currentCoroutine == null) {
                currentCoroutine = StartCoroutine(Fire());
            }
        }
        if(isSelected) {
            rangeShape.SetActive(true);
        } else {
            rangeShape.SetActive(false);
        }
        
    }

    public void SetSelection(bool selection) {
        isSelected = selection;
    }

    IEnumerator Fire() {
        GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
        tempProjectile.GetComponent<BulletController>().attackPower = attackPower;
        Destroy(tempProjectile, 5f);
        if(enemyQueue.Peek() != null) {
            Vector3 shootDir = enemyQueue.Peek().transform.position - transform.position;
            tempProjectile.GetComponent<BulletController>().shot(shootDir);
        }
        

        yield return new WaitForSeconds(attackSpeed);
        currentCoroutine = null;
    }

    // private void LockOn(GameObject target) {
    //     Vector3 offset = (target.transform.position - transform.position).normalized;

    //     transform.rotation = Quaternion.LookRotation(
    //         Vector3.forward, // Keep z+ pointing straight into the screen.
    //         offset           // Point y+ toward the target.
    //     ); 
    // }

    private void OnTriggerEnter2D(Collider2D other) {
        //Debug.Log("Goblin Detected");
        if (other.CompareTag("Enemy"))
        {
            // simply set the object you collide with inactive
            enemyQueue.Enqueue(other.gameObject);
            // currentTarget = other.gameObject;

        }
        
    }

    private void OnTriggerExit2D(Collider2D other) {
        //Debug.Log("Goblin Out of Sight");
        enemyQueue.Dequeue();
        // currentTarget = 
    }

}
