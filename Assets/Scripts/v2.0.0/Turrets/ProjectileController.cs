using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileController : MonoBehaviour
{
    Vector3 shootDirection;
    int attackPower;
    [SerializeField] float projectileSpeed; 
    bool armourPierce;
    bool armourDestroying;
    bool criticalHit;
    AoeManager aoeManager;

    public void shot(int power, Vector3 direction, bool piercing, bool destroying, AoeManager aoeEntity, bool critical) {
        attackPower = power;
        shootDirection = direction;
        armourPierce = piercing;
        armourDestroying = destroying;
        aoeManager = aoeEntity;
        criticalHit = critical;
        // -90 is offset angle for bullet direction
        transform.eulerAngles = new Vector3(0, 0, AngleFromVectorFloat(shootDirection)-90);
        
    }

    private void Update() {
        transform.position += shootDirection * projectileSpeed * Time.deltaTime;
    }


    private void OnTriggerEnter2D(Collider2D other) {
        if (other.CompareTag("Enemy")) {
            //Debug.Log("GOBLIN HIT!");
            other.gameObject.GetComponent<EnemyController>().TakeDamage(attackPower, armourPierce, armourDestroying, criticalHit);
            if(aoeManager) {
                aoeManager.AoeAttack(other.gameObject.transform.position);
            }
            Destroy(gameObject);
        }
    }

    private float AngleFromVectorFloat(Vector3 direction) {
        direction = direction.normalized;
        float d = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        if(d < 0) d += 360;

        return d;
    }
}
