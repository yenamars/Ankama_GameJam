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

    private GameObject[] pool;
    private Transform trsf;

    [HideInInspector] public int aliveMobs;

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
                BaseAI b = pool[i].GetComponent<BaseAI>();
                b.IsDead = false;
                b.SetSpawner(this);
                pool[i].transform.position = trsf.position + new Vector3(r.x, r.y, 0.0f);
                pool[i].SetActive(true);
                aliveMobs += 1;

                break;
            }
        }
    }

    public void mobIsDead()
    {
        aliveMobs -= 1;

        if (aliveMobs < 0)
        {
            Debug.LogWarning("There is a probleme with the spawner!!");
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
            SpawnObjectAtRandomPosition();

            yield return new WaitForSeconds(Random.Range(randomDelay.x, randomDelay.y));
            while (spawn == false)
            {
                yield return null;
            }
        }
    }

    public void Stop()
    {
        StopCoroutine("SpawnCoroutine");
        spawn = false;
    }
}
