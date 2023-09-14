using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Start is called before the first frame update
    public int price;
    public int attackPower;
    public int rangeRadius;
    public float attackSpeed;
    public bool placed;
    private CircleCollider2D towerRange;
    private GameObject currentTarget;
    private Queue<GameObject> enemyQueue;

    [SerializeField]
    private GameObject projectile;
    private Coroutine currentCoroutine;

    public bool isSelected;
    private GameObject rangeShape;
    public TowerData lvla1;
    public TowerData lvla2;
    public TowerData lvla3;
    public TowerData lvlb1;
    public TowerData lvlb2;
    public TowerData lvlb3;
    public TowerData lvlc1;
    public TowerData lvlc2;
    public TowerData lvlc3;

    public Queue<TowerData> branch1;
    public Queue<TowerData> branch2;
    public Queue<TowerData> branch3;

    public List<Queue<TowerData>> lvlTree;

    public GameObject weapon;


    private void Start() {
        towerRange = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        towerRange.isTrigger = true;
        placed = false;
        enemyQueue = new Queue<GameObject>();
        rangeShape = transform.GetChild(0).gameObject;
        weapon = transform.GetChild(2).gameObject;
        rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);
        isSelected = true;
        CreateLvlTree();


        
    }

    private void Update() {
        towerRange.radius = rangeRadius;
        rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);
        if(enemyQueue.Count > 0 && placed) {
            
            LockOn(enemyQueue.Peek());
            if(currentCoroutine == null) {
                currentCoroutine = StartCoroutine(Fire());
            }
        }
        if(isSelected) {
            rangeShape.GetComponent<SpriteRenderer>().enabled = true;
        } else {
            rangeShape.GetComponent<SpriteRenderer>().enabled = false;
        }

        if(!placed) {
            towerRange.enabled = false;
        } else {
            towerRange.enabled = true;
        }
        
    }

    public void SetSelection(bool selection) {
        isSelected = selection;
    }

    public void SetPlacement(bool placement) {
        placed = placement;
    }

    IEnumerator Fire() {
        if(enemyQueue.Count > 0 && enemyQueue.Peek() != null) {
            Vector3 shootDir = enemyQueue.Peek().transform.position - transform.position;
            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            tempProjectile.GetComponent<BulletController>().attackPower = attackPower;
            Destroy(tempProjectile, 3f);
            tempProjectile.GetComponent<BulletController>().shot(shootDir);
        } else if(enemyQueue.Peek() == null) {
            enemyQueue.Dequeue();
        }
        

        yield return new WaitForSeconds(attackSpeed);
        currentCoroutine = null;
    }
    

    private void LockOn(GameObject target) {
        if(target != null) {
            Vector3 offset = (target.transform.position - weapon.transform.position).normalized;

            weapon.transform.rotation = Quaternion.LookRotation(
            Vector3.forward, // Keep z+ pointing straight into the screen.
            offset           // Point y+ toward the target.
            ); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Enemy"))
        {
            enemyQueue.Enqueue(other.gameObject);
        }
    }
    private void OnTriggerExit2D(Collider2D other) {

        if (other.CompareTag("Enemy") && enemyQueue.Count > 0)
        {
            enemyQueue.Dequeue();
        }
    }

    private void CreateLvlTree() {
        branch1 = new Queue<TowerData>(new[] {lvla1, lvlb1, lvlc1});
        branch2 = new Queue<TowerData>(new[] {lvla2, lvlb2, lvlc2});
        branch3 = new Queue<TowerData>(new[] {lvla3, lvlb3, lvlc3});
        lvlTree = new List<Queue<TowerData>>{branch1, branch2, branch3};
    }

    public List<Queue<TowerData>> GetLevelTree() {
        return lvlTree;
    }

}
