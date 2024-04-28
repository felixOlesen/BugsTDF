using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretSpriteController : MonoBehaviour
{
    [SerializeField] GameObject activeWeapon;
    [SerializeField] GameObject activeBase;
    [SerializeField] List<GameObject> weapons;
    [SerializeField] List<GameObject> bases;
    

    public void LevelUpSprite(int level) {
        activeWeapon.SetActive(false);
        activeBase.SetActive(false);
        activeWeapon = weapons[level];
        activeBase = bases[level];
        activeWeapon.SetActive(true);
        activeBase.SetActive(true);
    }

    public GameObject GetActiveWeapon() {
        return activeWeapon;
    }

}
