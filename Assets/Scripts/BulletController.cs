using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour
{

    private Vector3 shootDir;
    public int attackPower;
    public float projectileSpeed;
    public bool armourPierce;

    public void shot(Vector3 shootDir, bool piercing) {
        this.shootDir = shootDir;
        projectileSpeed = 4f;
        this.armourPierce = piercing;
        transform.eulerAngles = new Vector3(0, 0, AngleFromVectorFloat(shootDir));
        
    }

    private void Update() {
        transform.position += shootDir * projectileSpeed * Time.deltaTime;
    }

    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            //Debug.Log("GOBLIN HIT!");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(attackPower, this.armourPierce);
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
