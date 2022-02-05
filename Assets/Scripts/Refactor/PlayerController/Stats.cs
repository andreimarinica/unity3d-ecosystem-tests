using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health = 100f;
    public float hunger = 0f;
    public float reproductiveUrge = 0f;
    public int generation = 0;
    public int sex; // 0 = male; 1 = female;
    public enum Species {
        Food,
        Rabbit,
        Fox
    }

    public enum Diet {
        Food,
        Rabbit,
        Fox
    }
    void Start()
    {
        // Generate Sex On Init
        sex = Random.Range(0,2);
    }

    void Update()
    {
        
    }
}
