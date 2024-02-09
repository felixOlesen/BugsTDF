using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Vector3 shootDir;
    public int attackPower;
    public float projectileSpeed;
    public bool armourPierce;
    public bool armourDestroying;
    private GameObject tower;

    public void shot(Vector3 shootDir, bool piercing, bool destroying, GameObject parentTower) {
        this.shootDir = shootDir;
        this.projectileSpeed = 12f;
        this.armourPierce = piercing;
        this.armourDestroying = destroying;
        this.tower = parentTower;
        // -90 is offset angle for bullet direction
        transform.eulerAngles = new Vector3(0, 0, AngleFromVectorFloat(shootDir)-90);
        
    }

    private void Update() {
        transform.position += shootDir * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            //Debug.Log("GOBLIN HIT!");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(this.attackPower, this.armourPierce, this.armourDestroying);
            if(tower) {
                tower.GetComponent<TowerController>().SetAoePosition(other.gameObject.transform.position);
            }
            Destroy(gameObject);
        }
    }

    private float AngleFromVectorFloat(Vector3 dir) {
        dir = dir.normalized;
        float d = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        if(d<0) d += 360;

        return d;
    }

}
