using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public CollisionP CollisionP;
    public Movement Movement;
    public Stats Stats;

    // Start is called before the first frame update
    //NOTE: if we use start method will throw error as the object is not assigned yet so we use to set these in awake
    void Awake()
    {
        CollisionP = GetComponent<CollisionP>();
        Movement = GetComponent<Movement>();
        Stats = GetComponent<Stats>();
    }

    // Update is called once per frame
    public void Update()
    {
        
    }
}
