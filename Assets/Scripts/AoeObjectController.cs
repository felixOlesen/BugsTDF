using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AoeObjectController : MonoBehaviour
{
    // Start is called before the first frame update

private CircleCollider2D aoeCollider;
public float aoeScalar;
public int aoeDamage;
void Start() {
    aoeCollider = gameObject.AddComponent(typeof(CircleCollider2D)) as CircleCollider2D;
}

public void InflictAoe(float radius, float duration, float scalar, int damage, string attackType) {
    aoeCollider.radius = radius;
    aoeScalar = scalar;
    aoeDamage = damage;
    gameObject.tag = attackType;
    Invoke("FinishAoe", duration);
}

private void FinishAoe() {
    aoeCollider.radius = 0.0f;
}
}
