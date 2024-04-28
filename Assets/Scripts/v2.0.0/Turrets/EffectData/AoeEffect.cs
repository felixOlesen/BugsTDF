using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AoeEffect")]
public class AoeEffect : ScriptableObject
{
public bool hasAoe;
public string aoeType;
public float aoeRadius;
public float aoeScalar;
public GameObject aoeParticles;
}
