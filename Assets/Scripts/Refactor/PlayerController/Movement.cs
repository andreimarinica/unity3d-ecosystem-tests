using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Movement : MonoBehaviour
{
    [System.NonSerialized] public float speed = 10f;
    [System.NonSerialized] public float visualRange = 4f;
    public float targetDistance;
    public Vector3 targetLocation;
    public GameObject target;
    public GameArea GameArea;
    // Start is called before the first frame update
    void Awake()
    {
        GameArea = FindObjectOfType<GameArea>();
    }

    void Start() {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveToTarget(Vector3 targetPosition)
    {
        // change the vector3 target to use our Y POS
        Vector3 target = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);

        // our position will move towards target
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        // look at our target
        transform.LookAt(target);
    }
}
