using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameArea : MonoBehaviour
{
    [Range(10, 1000)]
    public float range;
    public InitialPopulations[] initialPopulations;
    private PlayerController PlayerController;
    
    // draw gizmo for the game area 
    private void OnDrawGizmos() {

        // draw cube for play area
        Gizmos.color = new Color(0f, 0f, 0f, 0.2f);
        Gizmos.DrawCube(transform.position, new Vector3(range * 2, range, range * 2));

        // draw wire for cube
        Gizmos.color = new Color(0f, 0f, 0f, 0.4f);
        Gizmos.DrawWireCube(transform.position, new Vector3(range * 2, range, range * 2));

        // move the transform to plane level
        transform.position = new Vector3(transform.position.x, range / 2, transform.position.z);
        
    }
    private void Awake() {
        // set the player controller
        foreach (InitialPopulations pop in initialPopulations)
        {
            for (int i = 0; i < pop.count; i++)
            {
                SpawnEntity(pop.entity);
            }
        }
    }

    public Vector3 GetRandomPosition()
    {
        Vector3 position = new Vector3(Random.Range(-range, range), 0.5f, Random.Range(-range, range));
        return position;
    }

    public void SpawnEntity(GameObject entity)
    {
        Vector3 spawnPoint = GetRandomPosition();
        spawnPoint = new Vector3(spawnPoint.x, entity.transform.position.y, spawnPoint.z);
        Instantiate(entity, spawnPoint, Quaternion.identity);
    }
 
}

[System.Serializable]
public class InitialPopulations
{
    public string name;
    public GameObject entity;
    public int count;
}
