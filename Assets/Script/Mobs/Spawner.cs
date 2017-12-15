using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour 
{
    public bool spawn;
    [SerializeField] private GameObject objectsToSpawn;
    [SerializeField] private int poolSize;
    [SerializeField] private float spawnRange;
    [SerializeField] public Vector2 randomDelay;

    [HideInInspector] public Transform MobRoot;

    private GameObject[] pool;
    private Transform trsf;

	void Awake () 
    {
        pool = new GameObject[poolSize];
        trsf = transform;

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(objectsToSpawn, trsf);
            pool[i].SetActive(false);
        }
	}

    void SpawnObjectAtRandomPosition()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i].activeSelf == false)
            {
                Vector2 r = Random.insideUnitCircle * spawnRange;
                pool[i].GetComponent<BaseAI>().IsDead = false;
                pool[i].transform.parent = MobRoot;
                pool[i].transform.position = trsf.position + new Vector3(r.x, r.y, 0.0f);
                pool[i].SetActive(true);
               // Debug.Log("Span");
                break;
            }
        }
    }

    public void StartSpawn()
    {
        StartCoroutine(SpawnCoroutine());
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(Random.Range(randomDelay.x, randomDelay.y));
            while (spawn == false)
            {
                yield return null;
            }

           // Debug.Log(Time.realtimeSinceStartup);
            SpawnObjectAtRandomPosition();
        }
    }

    public void Stop()
    {
        StopCoroutine("SpawnCoroutine");
        spawn = false;
    }
}
