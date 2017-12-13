using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableShakeController : MonoBehaviour 
{
    [SerializeField] private bool shakeOnEnable;
    [SerializeField] private StackableShakeData[] datas;
    [SerializeField] private StackableShake stackableShake;

    void OnEnable()
    {
        if (shakeOnEnable)
        {
            StartCoroutine(ShakeCoroutine());
        }
    }
      
    public void Shake()
    {
        StartCoroutine(ShakeCoroutine());
    }

    IEnumerator ShakeCoroutine()
    {
        for (int i = 0; i < datas.Length; i++)
        {
            if (datas[i].delay > 0)
            yield return new WaitForSeconds(datas[i].delay);


            if (datas[i].randomSeed == true)
            {
                StackableShakeData d = ScriptableObject.CreateInstance<StackableShakeData>();
                d.amplitude = datas[i].amplitude;
                d.curve = datas[i].curve;
                d.delay = datas[i].delay;
                d.duration = datas[i].duration;
                d.enableX = datas[i].enableX;
                d.enableY = datas[i].enableY; 
                d.enableZ = datas[i].enableZ;
                d.frequency = datas[i].frequency;
                d.noiseType = datas[i].noiseType;

                if (datas[i].enableX == true)
                    d.seed.x = Random.value;

                if (datas[i].enableY == true)
                    d.seed.y = Random.value;

                if (datas[i].enableZ == true)
                    d.seed.z = Random.value;

                stackableShake.Shake(d);
            }
            else
            {
                stackableShake.Shake(datas[i]);
            }

        }
    }

}
