using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Environment : MonoBehaviour
{
    [Range(10, 1000)]
    public float range;
    public InitialPopulations[] initialPopulations;
    public LayerMask animalLayer;
    // Start is called before the first frame update
    void Awake()
    {
        // foreach(InitialPopulations pop in initialPopulations)
        // {
        //     for (int i = 0; i < pop.count; i++)
        //     {
        //         SpawnAnimal(pop.prefab);
        //     }
        // }
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnDrawGizmos() {
        Gizmos.color = new Color(0f, 0f, 0f, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(range * 2, range, range * 2));
        transform.position = new Vector3(transform.position.x, range / 2, transform.position.z);
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-range, range), 0.5f, Random.Range(-range, range));

        if(!Physics.Raycast(position, Vector3.down, 10))
        {
            position = GetRandomPosition();
        }

        return position;
    }

    public Vector3 GetRunAwayPosition(Transform fromTarget)
    {
        float currentDistance = 1f;
        Vector3 position = Vector3.zero;
        for (int i = 0; i < 20; i++)
        {
            Vector3 tryPosition = GetRandomPosition();
            float dist = Vector3.Distance(tryPosition, fromTarget.position);
            if(dist > currentDistance)
            {
                position = tryPosition;
                currentDistance = dist;
            }
        }
        return position;
    }

    public void SpawnAnimal(GameObject prefab)
    {
        Vector3 spawnPoint = GetRandomPosition();
        spawnPoint = new Vector3(spawnPoint.x, prefab.transform.position.y, spawnPoint.z);
        Instantiate(prefab, spawnPoint, Quaternion.identity);
    }
}

// [System.Serializable]
// public class InitialPopulations
// {
//     public string name;
//     public GameObject prefab;
//     public int count;
// }
