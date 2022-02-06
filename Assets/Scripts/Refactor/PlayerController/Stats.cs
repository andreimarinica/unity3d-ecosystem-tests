using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class Stats : MonoBehaviour
{
    public float health = 100f;
    public float healthDecreaseRatio = 10f;
    public float hunger = 0f;
    public float hungerDecreaseRatio = 10f;
    public float reproductiveUrge = 0f;
    public float reproductiveUrgeIncreaseRatio = 5f;
    public int generation = 0;
    public int sex; // 0 = male; 1 = female;
    public Species Species;
    public Species Diet;
    public List<GameObject> foodList = new List<GameObject>();
    public Material deathMat;
    void Start()
    {
        // Generate Sex On Init
        sex = Random.Range(0,2);
    }

    void Update()
    {
        // process hunger variable
        if(hunger < 100f) hunger = Mathf.Clamp(hunger += Time.deltaTime * hungerDecreaseRatio, 0f, 100f);

        // process health variable
        if(hunger >= 100f && health <= 100f) health = Mathf.Clamp(health -= Time.deltaTime * healthDecreaseRatio, 0f, 100f);

        // process reproductiveUrge
        reproductiveUrge = Mathf.Clamp(reproductiveUrge += Time.deltaTime * reproductiveUrgeIncreaseRatio, 0f, 100f);
    }

    

}
