using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TowerController : MonoBehaviour
{
    // Start is called before the first frame update
    public bool armourPierce;
    public int price;
    public int sellPrice;
    public int attackPower;
    public int rangeRadius;
    public float attackSpeed;
    public bool placed;
    private CircleCollider2D towerRange;
    private GameObject currentTarget;
    private List<GameObject> enemyList;

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
    public GameObject weapon1;
    public GameObject weapon2;
    public GameObject weapon3;
    public GameObject activeWeapon;
    public GameObject towerBase;
    public GameObject towerBase1;
    public GameObject towerBase2;
    public GameObject towerBase3;
    public GameObject activeTowerBase;
    public bool stealthVision;
    public bool armourDestroying;


    private void Start() {
        towerRange = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
        towerRange.isTrigger = true;
        placed = false;
        enemyList = new List<GameObject>();
        rangeShape = transform.GetChild(0).gameObject;
        activeWeapon = weapon;
        // weapon = transform.GetChild(2).gameObject;
        rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);
        sellPrice = Mathf.RoundToInt(Mathf.Abs(price) * 0.75f);
        isSelected = true;
        CreateLvlTree();
    }

    private void Update() {
        towerRange.radius = rangeRadius;
        rangeShape.transform.localScale = new Vector3(rangeRadius*2, rangeRadius*2, 1);
        if(enemyList.Count > 0 && placed) {
            LockOn(enemyList[0]);
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

    public void UpdateSellPrice(int addedCost) {
        sellPrice += Mathf.RoundToInt(addedCost * 0.75f);
    }

    public void UpdateSkinLevel(int ind) {
        if(ind == 1) {
            weapon.SetActive(false);
            activeWeapon = weapon1;
            weapon1.SetActive(true);
            towerBase.SetActive(false);
            activeTowerBase = towerBase1;
            towerBase1.SetActive(true);
        } else if(ind == 2) {
            weapon1.SetActive(false);
            activeWeapon = weapon2;
            weapon2.SetActive(true);
            towerBase1.SetActive(false);
            activeTowerBase = towerBase2;
            towerBase2.SetActive(true);
        } else if(ind == 3) {
            weapon2.SetActive(false);
            activeWeapon = weapon3;
            weapon3.SetActive(true);
            towerBase2.SetActive(false);
            activeTowerBase = towerBase3;
            towerBase3.SetActive(true);
        }
    }

    IEnumerator Fire() {
        if(enemyList.Count > 0 && enemyList[0] != null) {
            Vector3 shootDir = enemyList[0].transform.position - transform.position;
            GameObject tempProjectile = Instantiate(projectile, transform.position, Quaternion.identity);
            tempProjectile.GetComponent<BulletController>().attackPower = attackPower;
            Destroy(tempProjectile, 3f);
            tempProjectile.GetComponent<BulletController>().shot(shootDir, armourPierce, armourDestroying);
        } 
        yield return new WaitForSeconds(attackSpeed);
        currentCoroutine = null;
    }
    

    private void LockOn(GameObject target) {
        if(target != null) {
            Vector3 offset = (target.transform.position - activeWeapon.transform.position).normalized;
            activeWeapon.transform.rotation = Quaternion.LookRotation(
            Vector3.forward, // Keep z+ pointing straight into the screen.
            offset           // Point y+ toward the target.
            ); 
        }
    }

    private void OnTriggerEnter2D(Collider2D other) {

        if (other.CompareTag("Enemy"))
        {
            if (other.gameObject.GetComponent<EnemyController>().stealthy && stealthVision) {
                enemyList.Add(other.gameObject);
            } else if (!other.gameObject.GetComponent<EnemyController>().stealthy) {
                enemyList.Add(other.gameObject);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D other) {
        if (other.CompareTag("Enemy") && enemyList.Count > 0)
        {
            enemyList.Remove(other.gameObject);
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
