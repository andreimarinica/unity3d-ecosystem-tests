using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public ManageCollision ManageCollision;
    public Movement Movement;
    public Stats Stats;

    // Start is called before the first frame update
    //NOTE: if we use start method will throw error as the object is not assigned yet so we use to set these in awake
    void Awake()
    {
        ManageCollision = GetComponent<ManageCollision>();
        Movement = GetComponent<Movement>();
        Stats = GetComponent<Stats>();
    }

    void Start() {
        // on init add the sphere collider as a trigger
        // NOTE: when we will instantiate a child it will take this component as well
        SphereCollider sphereCollider = gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = Movement.visualRange;
        sphereCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void KillEntity(GameObject entity)
    {
        Destroy(entity);
    }

    public void InstantiateEntity(GameObject entity)
    {
        Instantiate(entity, Movement.GameArea.GetRandomPosition(), Quaternion.identity);
    }
}
