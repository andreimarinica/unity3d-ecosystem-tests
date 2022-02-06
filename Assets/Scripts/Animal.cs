using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// [Messages for later]
// NOTE: transform.LookAt(2 * transform.position - new Vector3(Target.position)) - object to look away - TRY
// BUG: gizmos not updating to the new range of the new generation
// BUG: dumb ai, running into entities that higher in the food chain
// BUG: if pregnant female dies before birth, her partener's stats are not resetting and cannot reproduce anymore
// NOTE: look to implement a finite state machine with inheritance to get rid of this ugly code and to be able to implement corutines.
// NOTE: look to implement genetic algorithms...

// [Messages for later]
public class Animal : MonoBehaviour
{
    // TITLE: [Animal Traits]

        // SUB: [Movement & Other]
    public bool isIdle = false;
    public float animalSpeed = 10f;
    public float animalVisionRadius = 5f;
    public int generation = 0;
        // SUB: [Hunger]
    public bool isHungry = false;
    public float animalHunger = 0f;
    public float animalHungerDecayRate = 10f;
    public float animalHungerDecreaseValue = 1f;
        // SUB: [Health]
    public bool isAlive = true;
    public float animalHealth = 100f;
    public float animalHealthDecayRate = 10f;
    public float animalHealthDecreaseValue = 1f;
        // SUB: [Reproduction]
    public bool isMale = true;
    public bool hasPartener = false;
    public bool recentlyReproducted = false;
    public bool reachedPartener = false;
    public bool isPregnant;
    public float pregnantCooldown = 20f;
    public float pregnantCooldownTimeStamp = 0f;
    public float animalReproductiveUrge = 50f;
    public float animalUrgeIncreaseValue = 1f;
    public float animalUrgeGrowthRate = 10f;
    private float animalReproductiveTime = 15f;
    public float distanceParteners;
    float animalReproductiveTimeStamp = 0f;
    GameObject myPartner;
    GameObject fatherOfMyChild;
        // SUB: [Genes]
    float babySpeedRand = 0f;
    float babyRangeRand = 0f;
    float animalRandomMutation = 0f; 
    float animalFathersSpeed = 0f;
    float animalFathersRange = 0f;
        // SUB: [TimeIncrement]
        // TODO: Check if can be done other way as we are using and resetting same vars for different operations
    public float animalTimeAdditive = 0f;
    public float animalTimeLimitIncrement = 10f;

    // public float animalSize;
    // public float animalLifeTime;
    // public float animalLifeExpectancy;

    // [/Animal Traits]
    // [Range(1f, 10f)]
    // float timeIncrement;
    bool foundInterest = false;
    bool danger = false;
    public Species species;
    public Species diet;
    float initialDist = 1000f;
    float newDist;
    int minimumDistanceIndex;
    Environment environment;
    TimeSystemHandler timeSystem;
    Counters counters;
    List<GameObject> triggeredFoodList = new List<GameObject>();
    Vector3 targetPosition = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        // initial set of variables
        if(Random.Range(0,2) == 1) isMale = false;
        else isMale = true;
        environment = FindObjectOfType<Environment>();
        timeSystem = FindObjectOfType<TimeSystemHandler>();
        counters = FindObjectOfType<Counters>();
        targetPosition = environment.GetRandomPosition();

        //add spherecollider and the radius only for generation 0
        if(generation == 0)
        {
            SphereCollider trigger = gameObject.AddComponent<SphereCollider>();
            trigger.radius = animalVisionRadius;
            trigger.isTrigger = true;
        }

        // [Counter of population when first alive]
        if(isMale && species == Species.blueCube) counters.blueMale++;
        if(!isMale && species == Species.blueCube) counters.blueFemale++;
        if(isMale && species == Species.greenCube) counters.greenMale++;
        if(!isMale && species == Species.greenCube) counters.greenFemale++;
        if(species == Species.blueCube) counters.bluePopulation++;
        if(species == Species.greenCube) counters.greenPopulation++;
        counters.totalPopulation++;
        
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
        //Debug.Log(distance);
        //IsAnimalStatic(distance);

        if(distance <= 1f)
        {
            if(!foundInterest)
            {
                targetPosition = environment.GetRandomPosition();
                danger = false;
            }
            
        }
        
        AnimalStatsGenerator();
        AnimalStatusControl();
        if(!isIdle) {
            Move(targetPosition);
        }
        
    }

    void FixedUpdate() 
    {
        AnimalSensors();
        if(isIdle) {
            // BUG: gameObject is getting pushed out of range in reproduction => always idle as connection is lost
            GetComponent<Rigidbody>().AddForce(Vector3.down, ForceMode.Force);
        }
    }

    void AnimalSensors()
    {
        
    }

    // private void OnDrawGizmos() {
    //     if(this.species == Species.blueCube)
    //     {
    //         Gizmos.color = new Color(1f, 0f, 0f, 0.2f);
    //         Gizmos.DrawSphere(transform.position, animalVisionRadius * 4);
    //     }
    //     else
    //     {
    //         if(this.species == Species.greenCube)
    //         {
    //             Gizmos.color = new Color(1f, 1f, 1f, 0.2f);
    //             Gizmos.DrawSphere(transform.position, animalVisionRadius * 2);
    //         }
    //     }
        
        
    // }

    void AnimalStatsGenerator()
    {
        //  [Hunger Stats Generator]

        if(!isHungry && isAlive)
        {
            if(animalTimeAdditive <= animalTimeLimitIncrement)
            {
                animalTimeAdditive += Time.deltaTime * animalHungerDecayRate;
            }
            else
            {
                animalHunger += animalHungerDecreaseValue;
                animalHunger = Mathf.Clamp(animalHunger, 0f, 100f);
                if(animalHunger >= 100)
                {
                    isHungry = true;
                }
                else    animalTimeAdditive = 0f;
            }
        }

        //  [Health Stats Generator]
        if(isHungry && isAlive)
        {
            if(animalTimeAdditive <= animalTimeLimitIncrement)
            {
                animalTimeAdditive += Time.deltaTime * animalHealthDecayRate;
            }
            else
            {
                animalHealth -= animalHealthDecreaseValue;
                animalHealth = Mathf.Clamp(animalHealth, 0f, 100f);
                if(animalHealth <= Mathf.Epsilon)
                {
                    isAlive = false;
                }
                else    animalTimeAdditive = 0f;
            }
        }

        // [Reproduction Stats Generator]

        // if recently had baby can move again
        if(recentlyReproducted) isIdle = false;
        // if recently had baby and is female -> pregnant
        if(recentlyReproducted && !isMale) 
        {
            isPregnant = true;
        }
        // handle reproductivity urge to go back up after birth
        if(!isPregnant && !recentlyReproducted)
        {
            if(animalTimeAdditive <= animalTimeLimitIncrement)
            {
                animalTimeAdditive += Time.deltaTime * animalUrgeGrowthRate;
            }
            else
            {
                animalReproductiveUrge += animalUrgeIncreaseValue;
                animalReproductiveUrge = Mathf.Clamp(animalReproductiveUrge, 0f, 50f);
                animalTimeAdditive = 0f;
            }
        }
        // hande pregnant status
        if(isPregnant)
        {
            //StartCoroutine(DeliverBaby());
            //wait time to deliver the baby
            pregnantCooldownTimeStamp += Time.deltaTime;
            if(pregnantCooldownTimeStamp > pregnantCooldown)
            {
                //TODO: gene algorithm here...
                //ACTION: baby time
                //if wait time is over - deliver baby
                //Debug.Log("BABYBABYBABY");
                counters.totalBirths++;
                // reset timer
                pregnantCooldownTimeStamp = 0f;
                animalReproductiveTimeStamp = 0f;
                isPregnant = false;
                recentlyReproducted = false;
                reachedPartener = false;
                // check for null otherwise if the father dies before the baby is born this will throw an error
                if(fatherOfMyChild != null)
                {
                    fatherOfMyChild.GetComponent<Animal>().reachedPartener = false;
                    fatherOfMyChild.GetComponent<Animal>().recentlyReproducted = false;
                }
                
                var baby = Instantiate(this.gameObject, transform.position, Quaternion.identity);
                Animal babyAnimal = baby.GetComponent<Animal>();
                // but baby stats are random between mother and father
                // TODO: get a better formula for the new generation; only the fittest will survive and replicate /w for genes
                // NOTE: working on formula...
                // animalRandomMutation + animalFathersSpeed + animalFathersRange
                animalRandomMutation = Random.Range(0.9f, 1.1f);
                babySpeedRand = ((animalSpeed + animalFathersSpeed) * animalRandomMutation) / 2f;
                babyRangeRand = ((animalVisionRadius + animalFathersRange) * animalRandomMutation) / 2f;

                // assign the better stats to child???
                if(babySpeedRand < animalSpeed) babySpeedRand = animalSpeed;
                if(babyRangeRand < animalVisionRadius) babyRangeRand = animalVisionRadius;

                // for first generation of babies it adds another collider..so destroy and re-add
                if(babyAnimal.generation == 0) {
                    Destroy(baby.GetComponent<SphereCollider>());
                    SphereCollider babySC = baby.AddComponent<SphereCollider>();
                    babySC.radius = babyRangeRand;
                    babySC.isTrigger = true;
                }

                
                // we can now increase generation and the collider issue will go away
                babyAnimal.generation++;
                
                // asign all stats to baby
                babyAnimal.animalSpeed = babySpeedRand;
                if(Random.Range(0,2) == 1) babyAnimal.isMale = false;
                else babyAnimal.isMale = true;
                babyAnimal.isHungry = false;
                babyAnimal.animalHealth = 100f;
                babyAnimal.animalHunger = 0f;
                // reset mother and father
                fatherOfMyChild = null;
                babyAnimal.fatherOfMyChild = null;
            }
         }



    }
    //NOTE: corutine will work if we add a state machine with inheritance
    // IEnumerator DeliverBaby()
    // {
    //     yield return new WaitForSeconds(pregnantCooldown);

    //     counters.totalBirths++;
    //             animalReproductiveTimeStamp = 0f;
    //             isPregnant = false;
    //             recentlyReproducted = false;
    //             reachedPartener = false;
    //             // check for null otherwise if the father dies before the baby is born this will throw an error
    //             if(fatherOfMyChild != null)
    //             {
    //                 fatherOfMyChild.GetComponent<Animal>().reachedPartener = false;
    //                 fatherOfMyChild.GetComponent<Animal>().recentlyReproducted = false;
    //             }
                
    //             var baby = Instantiate(this.gameObject, transform.position, Quaternion.identity);
    //             Animal babyAnimal = baby.GetComponent<Animal>();
    //             // but baby stats are random between mother and father
    //             // TODO: get a better formula for the new generation; only the fittest will survive and replicate /w for genes
    //             // NOTE: working on formula...
    //             // animalRandomMutation + animalFathersSpeed + animalFathersRange
    //             animalRandomMutation = Random.Range(0.9f, 1.1f);
    //             babySpeedRand = ((animalSpeed + animalFathersSpeed) * animalRandomMutation) / 2f;
    //             babyRangeRand = ((animalVisionRadius + animalFathersRange) * animalRandomMutation) / 2f;

    //             // assign the better stats to child???
    //             if(babySpeedRand < animalSpeed) babySpeedRand = animalSpeed;
    //             if(babyRangeRand < animalVisionRadius) babyRangeRand = animalVisionRadius;

    //             // for first generation of babies it adds another collider..so destroy and re-add
    //             if(babyAnimal.generation == 0) {
    //                 Destroy(baby.GetComponent<SphereCollider>());
    //                 SphereCollider babySC = baby.AddComponent<SphereCollider>();
    //                 babySC.radius = babyRangeRand;
    //                 babySC.isTrigger = true;
    //             }

                
    //             // we can now increase generation and the collider issue will go away
    //             babyAnimal.generation++;
                
    //             // asign all stats to baby
    //             babyAnimal.animalSpeed = babySpeedRand;
    //             if(Random.Range(0,2) == 1) babyAnimal.isMale = false;
    //             else babyAnimal.isMale = true;
    //             babyAnimal.isHungry = false;
    //             babyAnimal.animalHealth = 100f;
    //             babyAnimal.animalHunger = 0f;
    //             // reset mother and father
    //             fatherOfMyChild = null;
    //             babyAnimal.fatherOfMyChild = null;

    // }
    void AnimalStatusControl() 
    {
        if(!isAlive) 
        {
             // [Counter of population when first alive]
            if(isMale && species == Species.blueCube) counters.blueMaleDead++;
            if(!isMale && species == Species.blueCube) counters.blueFemaleDead++;
            if(isMale && species == Species.greenCube) counters.greenMaleDead++;
            if(!isMale && species == Species.greenCube) counters.greenFemaleDead++;
            if(species == Species.blueCube) counters.blueDeaths++;
            if(species == Species.greenCube) counters.greenDeaths++;
            counters.totalDeaths++;
            GetComponent<LiveEntity>().Die(CauseOfDeath.starvation);

        }
        if(animalHunger < 100f) isHungry = false;
    }

    void Move(Vector3 targetPosition)
    {
        Vector3 target = new Vector3(targetPosition.x, transform.position.y, targetPosition.z);
        transform.position = Vector3.MoveTowards(transform.position, target, animalSpeed * Time.deltaTime);
        transform.LookAt(target);
    }

    private void OnTriggerStay(Collider other) 
    {
                
        // see if the other object is a LiveEntity
        if(other.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            //  set the entity animal variable for easier access
            Animal entityAnimal = entity.GetComponent<Animal>();
            #region Reproduce
            // ACTION: REPRODUCE: if you and the other entity are both interested to mate
            // check if we are the same animal species and one of us is a male and the other one is a female
            if((entity.species == species) && ((isMale && !entityAnimal.isMale) || (!isMale && entityAnimal.isMale)))
            {
                // if we are both interested to mate and none of us have partner
                if(animalReproductiveUrge > animalHunger && entityAnimal
                .animalReproductiveUrge > entityAnimal.animalHunger && !hasPartener && !entityAnimal.hasPartener)
                {
                    hasPartener = true;
                    entityAnimal.hasPartener = true;
                    myPartner = entityAnimal.gameObject;
                    entityAnimal.myPartner = this.gameObject;
                    targetPosition = entity.transform.position;
                    entityAnimal.targetPosition = transform.position;
                    transform.LookAt(entity.transform); //look at partner
                    entity.transform.LookAt(transform); //partner to look at you
                    
                }
                // check distance between us to see when we are close - only if we not reached each other
                if(!reachedPartener)
                {
                    distanceParteners = Vector3.Distance(transform.position, entity.transform.position);
                } 
                // if we have a distance < 4 between us and we are partners we can mate
                if(distanceParteners < 4 && myPartner == entityAnimal.gameObject && entityAnimal.myPartner == this.gameObject)
                {
                    reachedPartener = true;
                    isIdle = true;
                    entityAnimal.isIdle = true;
                    animalReproductiveTimeStamp += Time.deltaTime;
                    //Debug.Log(animalReproductiveTimeStamp + " vs " + animalReproductiveTime);

                    // mating takes "animalReproductiveTime" so we have to wait; when this is done we finished mating
                    if(animalReproductiveTimeStamp >= animalReproductiveTime)
                    {
                        //Debug.Log("entered...");
                        recentlyReproducted = true;
                        entityAnimal.recentlyReproducted = true;
                        hasPartener = false;
                        entityAnimal.hasPartener = false;
                        myPartner = null;
                        entityAnimal.myPartner = null;
                        animalReproductiveUrge = 0f;
                        entityAnimal.animalReproductiveUrge = 0f;
                        targetPosition = environment.GetRandomPosition();
                        entityAnimal.targetPosition = environment.GetRandomPosition();
                        animalReproductiveTimeStamp = 0f;
                        //check for mother and assign the father
                        if(!isMale)
                        {
                            fatherOfMyChild = entityAnimal.gameObject;
                            animalFathersSpeed = fatherOfMyChild.GetComponent<Animal>().animalSpeed;
                            animalFathersRange = fatherOfMyChild.GetComponent<Animal>().animalVisionRadius;
                        }
                        else 
                        {
                            entityAnimal.fatherOfMyChild = this.gameObject;
                            entityAnimal.GetComponent<Animal>().animalFathersSpeed = animalSpeed;
                            entityAnimal.GetComponent<Animal>().animalFathersRange = animalVisionRadius;
                        }
                    }

                }
                
            }
            #endregion
            // ACTION: EAT: see if the other object, entity, is your diet (food)
            if(entity.species == diet && isHungry)
            {
                //is this new object?
                if(!triggeredFoodList.Contains(other.gameObject)) 
                {
                    //add it to the object list
                    triggeredFoodList.Add(other.gameObject); //Debug.Log(goList.Count);

                    //set the initial distance between this object and yourself
                    initialDist = Vector3.Distance(transform.position, new Vector3(other.transform.position.x, transform.position.y, other.transform.position.z));

                }
                    initialDist = 100000f;
                    //go through collection and see what is the minimum distance
                    for (int i = 0; i < triggeredFoodList.Count; i++)
                    {
                        if(triggeredFoodList[i] != null)
                        {             
                            newDist = Vector3.Distance(transform.position, new Vector3(triggeredFoodList[i].transform.position.x, transform.position.y, triggeredFoodList[i].transform.position.z));
                            if(initialDist >= newDist)
                            {
                                initialDist = newDist;
                                minimumDistanceIndex = i;    
                            }
                        }
                    }
                //minimum distance is now in goList[holdListI]; set the target
                targetPosition = triggeredFoodList[minimumDistanceIndex].transform.position;
                //we have an interest
                foundInterest = true;
                //calculate distance between yourself and the target
                float distance = Vector3.Distance(transform.position, new Vector3(targetPosition.x, transform.position.y, targetPosition.z));
                // BUG: shouldn't be distance instead of initialDist below???
                if(distance <= 3 && foundInterest && isHungry)
                {
                    isHungry = false;
                    Debug.Log("decateori");
                    animalHunger = 0f;
                    entity = triggeredFoodList[minimumDistanceIndex].gameObject.GetComponent<LiveEntity>();
                    triggeredFoodList.Remove(triggeredFoodList[minimumDistanceIndex].gameObject);
                    // if normal food - respawn
                    if(entity.species == Species.redCube)
                    {
                        Instantiate(entity.gameObject, environment.GetRandomPosition(), Quaternion.identity);
                    }
                    else 
                    {
                        // [Counter of dead population]
                        // TODO: ADD STATS FOR REASON AS WELL
                        if(entity.GetComponent<Animal>().isMale && entity.species == Species.blueCube) counters.blueMaleDead++;
                        if(!entity.GetComponent<Animal>().isMale && entity.species == Species.blueCube) counters.blueFemaleDead++;
                        if(entity.GetComponent<Animal>().isMale && entity.species == Species.greenCube) counters.greenMaleDead++;
                        if(!entity.GetComponent<Animal>().isMale && entity.species == Species.greenCube) counters.greenFemaleDead++;
                        if(entity.species == Species.blueCube) counters.blueDeaths++;
                        if(entity.species == Species.greenCube) counters.greenDeaths++;
                        counters.totalDeaths++;
                        // [Counter of dead population]
                    }
                    entity.Die(CauseOfDeath.beingEaten);   
                    foundInterest = false;
                }

            }
            // ACTION: FLEE: run away script if you encountered enemy that have you on their food list lol 
            // BUG: currently the AI is stupid and moves randomly, even towards enemies that are trying to eat // RIP         
            if(entity.species != Species.redCube && entity.gameObject.GetComponent<Animal>().diet == species && !danger)
            {
                targetPosition = environment.GetRunAwayPosition(entity.transform);
                foundInterest = true;
                danger = true;
            } 

        }
        
        else
        {   

            if(foundInterest)
            {
                foundInterest = false;
            }
        }
    }

    private void OnTriggerExit(Collider other) 
    {
        if(triggeredFoodList.Contains(other.gameObject))
        {
            triggeredFoodList.Remove(other.gameObject);
            if(triggeredFoodList.Count == 0) targetPosition = environment.GetRandomPosition();
        }


    }

    private void OnCollisionEnter(Collision other) {
        // BUGFIX: if a liveentity collides with a red cube it will change direction automatically
        // TODO: add the same for when a green hits blue or blue hits green
        // NOTE: be careful as might cause problems when a mating partener is found // 
        if(other.gameObject.TryGetComponent<LiveEntity>(out LiveEntity entity))
        {
            if(entity.species == Species.redCube)
            {
                targetPosition = environment.GetRandomPosition();
            }
                
        }
    }

}

