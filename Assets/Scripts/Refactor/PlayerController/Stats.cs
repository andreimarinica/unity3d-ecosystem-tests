using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stats : MonoBehaviour
{
    public float health = 100f;
    public float healthDecreaseRatio = 10f;
    public float hunger = 0f;
    public float hungerDecreaseRatio = 10f;
    public float reproductiveUrge = 0f;
    public int generation = 0;
    public int sex; // 0 = male; 1 = female;
    public Species Species;
    public Species Diet;
    public List<GameObject> foodList = new List<GameObject>();
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
        //if(hunger >= 100f && health <= 100f) health = Mathf.Clamp(health -= Time.deltaTime * healthDecreaseRatio, 0f, 100f);
    }

    private void OnDrawGizmos() {
        if(this.Species == Species.Fox)
        {
            Gizmos.color = new Color(0f, 0f, 1f, 0.2f); 
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 4);
        }

        if(this.Species == Species.Rabbit)
        {
            Gizmos.color = new Color(0f, 1f, 0f, 0.2f);
            Gizmos.DrawSphere(transform.position, GetComponent<Movement>().visualRange * 2);
        }

        if(this.Species == Species.Food)
        {
            Gizmos.color = new Color(1f, 0f, 0f, 0.5f); 
            Gizmos.DrawSphere(transform.position, 0.5f);
        }                        
        
    }
}
