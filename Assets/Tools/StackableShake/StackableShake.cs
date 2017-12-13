using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StackableShake : MonoBehaviour
{
    private enum Space
    {
        Local = 0,
        World = 1
    }
    private class StackableShakeDecay
    {
        public float decay = 1.0f;
        public float timer = 0.0f;
    }

    [SerializeField] private Space space;
    [SerializeField] private float maxShake;
    private List<StackableShakeData> m_shakeParams = new List<StackableShakeData>();
    private List<StackableShakeDecay> m_stackableShakeDecay = new List<StackableShakeDecay>();

    private Transform trsf;
    private Vector3 startPosition;
    private float time;
    private int count;

    void Awake()
    {
        trsf = transform;
    }

    void OnEnable()
    {
        if (space == Space.Local)
        {
            startPosition = trsf.localPosition;
        }
        else
        {
            startPosition = trsf.position;
        }

        time = 0.0f;
    }

    public void ResetComponent()
    {
        StopAllCoroutines();
        count = 0;
        m_shakeParams.Clear();
        m_stackableShakeDecay.Clear();
        time = 0.0f;
    }

    void Update()
    {
        for (int i = count - 1; i >= 0; i--)
        {
            m_stackableShakeDecay[i].decay = m_shakeParams[i].curve.Evaluate(m_stackableShakeDecay[i].timer);
            m_stackableShakeDecay[i].timer += Time.deltaTime / m_shakeParams[i].duration;

            if (m_stackableShakeDecay[i].timer > 1.0f)
            {
                m_stackableShakeDecay.RemoveAt(i);
                m_shakeParams.RemoveAt(i);
                count = m_shakeParams.Count;
            }
        }

        if (count > 0)
        {
            Vector3 shake = new Vector3(0.0f, 0.0f, 0.0f);

            for (int i = 0; i < count; i++)
            {
                float decay = m_stackableShakeDecay[i].decay;

                if (m_shakeParams[i].noiseType == StackableShakeData.NoiseType.Perlin)
                {
                    if (m_shakeParams[i].enableX == true)
                    {
                        shake.x += (Mathf.PerlinNoise(time * m_shakeParams[i].frequency.x, m_shakeParams[i].seed.x) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.x;
                    }

                    if (m_shakeParams[i].enableY == true)
                    {
                        shake.y += (Mathf.PerlinNoise(time * m_shakeParams[i].frequency.y, m_shakeParams[i].seed.y) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.y;
                    }

                    if (m_shakeParams[i].enableZ == true)
                    {
                        shake.z += (Mathf.PerlinNoise(time * m_shakeParams[i].frequency.z, m_shakeParams[i].seed.z) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.z;
                    }
                }
                else if (m_shakeParams[i].noiseType == StackableShakeData.NoiseType.Random)
                {
                    if (m_shakeParams[i].enableX == true)
                    {
                        shake.x += Random.Range(-1.0f, 1.0f) * decay * m_shakeParams[i].amplitude.x;
                    }

                    if (m_shakeParams[i].enableY == true)
                    {
                        shake.y += Random.Range(-1.0f, 1.0f) * decay * m_shakeParams[i].amplitude.y;
                    }

                    if (m_shakeParams[i].enableZ == true)
                    {
                        shake.z += Random.Range(-1.0f, 1.0f) * decay * m_shakeParams[i].amplitude.z;
                    }
                }
                else if (m_shakeParams[i].noiseType == StackableShakeData.NoiseType.Triangle)
                {
                    if (m_shakeParams[i].enableX == true)
                    {
                        float v = time * m_shakeParams[i].frequency.x + m_shakeParams[i].seed.x;
                        shake.x += (Mathf.Abs((v - (int)v) * 2.0f - 1.0f) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.x;
                    }

                    if (m_shakeParams[i].enableY == true)
                    {
                        float v = time * m_shakeParams[i].frequency.y + m_shakeParams[i].seed.y;
                        shake.y += (Mathf.Abs((v - (int)v) * 2.0f - 1.0f) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.y;
                    }

                    if (m_shakeParams[i].enableZ == true)
                    {
                        float v = time * m_shakeParams[i].frequency.z + m_shakeParams[i].seed.z;
                        shake.z += (Mathf.Abs((v - (int)v) * 2.0f - 1.0f) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.z;
                    }
                }
                else if (m_shakeParams[i].noiseType == StackableShakeData.NoiseType.Wave)
                {
                    if (m_shakeParams[i].enableX == true)
                    {
                        float v = time * m_shakeParams[i].frequency.x + m_shakeParams[i].seed.x;
                        float t = Mathf.Abs((v - (int)v) * 2.0f - 1.0f);
                        shake.x += ((t * t * (3.0f - 2.0f) * t) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.x;
                    }

                    if (m_shakeParams[i].enableY == true)
                    {
                        float v = time * m_shakeParams[i].frequency.y + m_shakeParams[i].seed.y;
                        float t = Mathf.Abs((v - (int)v) * 2.0f - 1.0f);
                        shake.y += ((t * t * (3.0f - 2.0f) * t) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.y;
                    }

                    if (m_shakeParams[i].enableZ == true)
                    {
                        float v = time * m_shakeParams[i].frequency.z + m_shakeParams[i].seed.z;
                        float t = Mathf.Abs((v - (int)v) * 2.0f - 1.0f);
                        shake.z += ((t * t * (3.0f - 2.0f) * t) * 2.0f - 1.0f) * decay * m_shakeParams[i].amplitude.z;
                    }
                }
            } 

            shake.x = Mathf.Clamp(shake.x, -maxShake, maxShake);
            shake.y = Mathf.Clamp(shake.y, -maxShake, maxShake);
            shake.z = Mathf.Clamp(shake.z, -maxShake, maxShake);

            if (space == Space.Local)
            {
                trsf.localPosition = startPosition + shake;
            }
            else
            {
                trsf.position = startPosition + shake;
            }

            time += Time.deltaTime;
        }
        else
        {
            time = 0.0f;

            if (space == Space.Local)
            {
                trsf.localPosition = startPosition;
            }
            else
            {
                trsf.position = startPosition;
            }
        }
    }

    public void Shake(StackableShakeData stackableShakeData)
    {
        m_shakeParams.Add(stackableShakeData);
        m_stackableShakeDecay.Add(new StackableShakeDecay());
        count = m_shakeParams.Count;
    }
}
