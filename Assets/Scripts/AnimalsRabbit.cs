using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AnimalsRabbit : MonoBehaviour
{
    [SerializeField] GameObject targetObj;
    [SerializeField] GameObject debugUI;
    GameObject foodTarget;

    TextMeshProUGUI debugTextUI;

    bool currentlyMoving = false;
    float movingSpeed = 5f;
    float randomX, randomZ;
    float rabbitHealth = 100f;
    //float rabbitMaxHealth = 100f;
    //float rabbitMaxHunger = 100f;
    float rabbitHunger = 0f;
    //float rabbitAge = 1f;
    //float rabbitMateNeed = 0f;
    string action;

    float multiplier = 10f;
    // Start is called before the first frame update
    void Start()
    {
        debugTextUI = debugUI.GetComponent<TextMeshProUGUI>();
    }
    void Update() {
        {
            if(Input.GetKeyDown(KeyCode.Space)) action="roam";
            if(Input.GetKeyDown(KeyCode.C)) action="smthelse";

            GenerateRabbitStats();
        }
    }
    // Update is called once per frame
    void FixedUpdate()
    {   
        if(action == "roam") RoamToRandomPosition();
        if(action == "food" && foodTarget != null) GoToSpecificPosition(foodTarget.transform.position);
        else action = "roam";
    }
    private void OnTriggerStay(Collider other) {
        Debug.Log("rabbit triggered with " + other.name);
        if(other.tag == "Food" && rabbitHunger > 20f) 
        {
            foodTarget = other.gameObject;
            action = "food";
        }
        
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Food")
        {
            rabbitHunger -= 10f;
            rabbitHealth += 10f;
            Destroy(other.gameObject);
        }
    }

    void GenerateRabbitStats() 
    {
        rabbitHealth = Mathf.Clamp(rabbitHealth, 0f, 100f);
        rabbitHunger = Mathf.Clamp(rabbitHunger, 0f, 100f);
        //hunger will grow over time
        rabbitHunger += Time.deltaTime * multiplier;
        if(rabbitHunger >= 20f)
        {
            rabbitHealth -= Time.deltaTime * (multiplier / 2f);
        }
        
        debugTextUI.text = (int)rabbitHealth + "<- health      hunger->" + (int)rabbitHunger;
    }

    // private void OnTriggerExit(Collider other) {
    //     Debug.Log("rabbit not triggered with " + other.name);
    //     if(other.name == "Food") 
    //     {
    //         //foodTarget = other.gameObject;
    //         action = "roam";
    //     }
    // }
    void RoamToRandomPosition()
    {
        if(!currentlyMoving) GenerateRandomPosition();
        Vector3 target = new Vector3(randomX, transform.position.y, randomZ);
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.fixedDeltaTime);
        currentlyMoving = true;
        if(transform.position == target) currentlyMoving = false;
            

        
    }

    void GoToSpecificPosition(Vector3 target)
    {
        transform.LookAt(target);
        transform.position = Vector3.MoveTowards(transform.position, target, movingSpeed * Time.fixedDeltaTime);
        currentlyMoving = true;
        if(transform.position == target) currentlyMoving = false;
    }

    void GenerateRandomPosition()
    {
            randomX = Random.Range(-25f, 25f);
            randomZ = Random.Range(-25f, 25f);
            //Vector3 target = new Vector3(randomX, transform.position.y, randomZ);
            //Instantiate(targetObj, target, Quaternion.identity);
    }
}
