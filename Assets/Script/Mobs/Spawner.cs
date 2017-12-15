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

	void Awake () 
    {
        pool = new GameObject[poolSize];
        trsf = transform;

        for (int i = 0; i < poolSize; i++)
        {
            pool[i] = Instantiate(objectsToSpawn, trsf);
            pool[i].SetActive(false);
        }

        StartCoroutine(SpawnCoroutine());
	}

    void SpawnObjectAtRandomPosition()
    {
        for (int i = 0; i < poolSize; i++)
        {
            if (pool[i].activeSelf == false)
            {
                Vector2 r = Random.insideUnitCircle * spawnRange;
                pool[i].transform.position = trsf.position + new Vector3(r.x, r.y, 0.0f);
                pool[i].SetActive(true);
                break;
            }
        }
    }

    IEnumerator SpawnCoroutine()
    {
        while (true)
        {
            while (spawn == false)
            {
                yield return null;
            }

            yield return new WaitForSeconds(Random.Range(randomDelay.x, randomDelay.y));
            Debug.Log(randomDelay.x + randomDelay.y);
            SpawnObjectAtRandomPosition();
        }
    }
}
