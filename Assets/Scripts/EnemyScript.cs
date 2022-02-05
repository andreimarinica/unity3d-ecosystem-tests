using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyScript : MonoBehaviour
{
    // [Rabbit Stats and Conditionals]
    float rabbitHungerTimer = 0f;
    float rabbitAge;
    float rabbitHealth = 100f;
    float rabbitHunger = 1f;
    //float rabbitUrgeToReproduce = 1f;
    //bool isMale;
    //bool isFemale;
    //bool canReproduce;
    //bool isPregnant;
    //bool hasDisease;
    //[SerializeField] Transform target;
    NavMeshAgent nav;
    float randomX, randomZ;
    bool isMoving = false;
    bool isStuck = false;
    string action = "patrol";
    Transform foodDestination;
    float destinationTimer = 0f;
    TimeSystemHandler timeSystem;
    // Start is called before the first frame update
    void Start()
    {
        nav = GetComponent<NavMeshAgent>();
        timeSystem = FindObjectOfType<TimeSystemHandler>();
    }

    // Update is called once per frame
    void Update()
    {
        CreateRabbitStats();
        CheckStuck();
        if(action == "patrol") PatrolToRandom();
        if(action == "food" && foodDestination.gameObject != null) PatrolToDestination(foodDestination.position, 10f);
        else action = "patrol";
    }

    void CreateRabbitStats()
    {
        rabbitHungerTimer = rabbitHungerTimer + (Time.deltaTime * 1f);
        //Debug.Log(rabbitHungerTimer);
        if(rabbitHungerTimer >= 30f) // every 30s
        {
            rabbitHealth -= 10f;
            rabbitHunger += 10f;
            Debug.Log("Health: " + rabbitHealth + " Hunger: " + rabbitHunger);
            if(rabbitHunger >= 60f)
            {
                rabbitHealth -= 10f;
                Debug.Log("Health: " + rabbitHealth + " Hunger: " + rabbitHunger);
            }
            rabbitHungerTimer = 0f;
        }
        if(rabbitHealth <= 0f) 
            {
                GetComponent<Renderer>().material.SetColor("_Color", Color.black);
                nav.isStopped = true;
                action = "dead";
            }

    }

    private void PatrolToRandom()
    {
        
        GenerateRandomDirection();
        isStuck = false;
        Vector3 target = new Vector3(randomX, transform.position.y, randomZ);
        nav.SetDestination(target);
        isMoving = true;
        nav.speed = 5f;
        if (nav.remainingDistance < Mathf.Epsilon) isMoving = false;
        
    }

    private void PatrolToDestination(Vector3 destination, float timeToSpend)
    {
        if(destination != null)
        {
            destinationTimer += Time.deltaTime;
            nav.SetDestination(destination);
            isMoving = true;
            nav.speed = 6f;
            if ((nav.remainingDistance < Mathf.Epsilon) || destinationTimer >= timeToSpend || destination == null) 
            {
                destinationTimer = 0f;
                isMoving = false;
                action = "patrol";
            }
        }
        
    }

    private void OnTriggerEnter(Collider other) {
        if(other.tag == "Food")
        {
            if(rabbitHunger >= 30f)
            {
                foodDestination = other.transform;
                action = "food";
            }

        }
    }

    private void OnCollisionEnter(Collision other) {
        if(other.gameObject.tag == "Food")
        {
            rabbitHunger = 0;
            rabbitHealth += 30f;
            Instantiate(other.gameObject, new Vector3(Random.Range(-25f, 25f), 0.8f, Random.Range(-25f, 25f)), Quaternion.identity);
            //action = "patrol";
            //Destroy(other.gameObject);
        }

    }

    void GenerateRandomDirection()
    {
        if(!isMoving || isStuck)
        {
        randomX = Random.Range(-30f, 30f);
        randomZ = Random.Range(-30f, 30f);
        }
    }

    void CheckStuck()
    {
        if((nav.velocity.x < 0.2f) && (nav.velocity.y == 0.2f) && isMoving)
        {
            isStuck = true;
            isMoving = false;
            action = "patrol";
        }
    }

}
