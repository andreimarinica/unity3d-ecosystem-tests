using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManageCollision : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.TryGetComponent<Stats>(out Stats entity))
        {
            if(entity.Species == Species.Food)
            {
                GetComponent<PlayerController>().Movement.targetLocation = GetComponent<PlayerController>().Movement.GameArea.GetRandomPosition();
            }
        }
    }
}
