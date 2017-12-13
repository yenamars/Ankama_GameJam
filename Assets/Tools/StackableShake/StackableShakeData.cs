using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "StackableShakeData", menuName = "ScriptableObjects/StackableShake", order = 0)]
public class StackableShakeData : ScriptableObject 
{
    public enum NoiseType
    {
        Perlin = 0,
        Wave = 1,
        Triangle = 2,
        Random = 3
    }

    public NoiseType noiseType;
    public bool enableX;
    public bool enableY;
    public bool enableZ;
    public Vector3 amplitude;
    public Vector3 frequency;
    public bool randomSeed;
    public Vector3 seed;
    public float duration;
    public float delay;
    public AnimationCurve curve;
}
