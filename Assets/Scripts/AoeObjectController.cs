using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeObjectController : MonoBehaviour
{
    // Start is called before the first frame update

private CircleCollider2D aoeCollider;
public float aoeScalar;
public int aoeDamage;
public GameObject aoeParticles;
public ParticleSystem system;
void Start() {
    aoeCollider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
    if(aoeParticles) {
        aoeParticles = Instantiate(aoeParticles, transform.position, Quaternion.identity);
        aoeParticles.SetActive(false);
        system = aoeParticles.GetComponent<ParticleSystem>();
    }
    
}

public void InflictAoe(float radius, float duration, float scalar, int damage, string attackType) {
    aoeCollider.radius = radius;
    aoeScalar = scalar;
    aoeDamage = damage;
    gameObject.tag = attackType;
    if(aoeParticles) {
        aoeParticles.SetActive(true);
        aoeParticles.transform.position = transform.position;
    }
    system.Play();
    StartCoroutine(FinishAoe(duration));
    //Invoke("FinishAoe", duration);
}

IEnumerator FinishAoe(float duration) {
    yield return new WaitForSeconds(duration);
    aoeCollider.radius = 0.0f;
    //Debug.Log("Aoe Off");
}

}
