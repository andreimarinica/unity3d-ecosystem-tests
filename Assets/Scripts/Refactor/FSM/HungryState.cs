using UnityEngine;

public class HungryState : BaseState
{
    float initialDistance;
    float newDistance;
    int minimumDistanceIndex;
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("bummer i'm hungry");

        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set random destination to look foor food
        PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // check player stats
        CheckPlayerStats(player);

        // if no food found move to random position
        if(PlayerController.Stats.foodList.Count == 0)
        {
            PlayerController.Movement.MoveToTarget(PlayerController.Movement.targetLocation);
            // set the distance
            PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, PlayerController.Movement.targetLocation);

            // check if we reached the target
            if(PlayerController.Movement.targetDistance <= 1f)
            {
                // if we reached target set a new random target
                PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
            }
        }
        else 
        {
            // we have a target assigned so move to it
            PlayerController.Movement.MoveToTarget(PlayerController.Movement.targetLocation);
        }
    }
    public override void CheckPlayerStats(PlayerStateManager player)
    {
        // check health
        if(PlayerController.Stats.health <= 0f)
        {
            player.SwitchState(player.Dead);
        }

    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other)
    {
        // get the items that have the stats class available
        if(other.TryGetComponent<Stats>(out Stats entity))
        {
            // if the species spotted is our diet we will add this to a list of foods
            if(entity.Species == PlayerController.Stats.Diet)
            {
                // check if we haven't already added this to our food list
                if(!PlayerController.Stats.foodList.Contains(other.gameObject))
                {
                    // if not, add it
                    PlayerController.Stats.foodList.Add(other.gameObject);
                }
                initialDistance = 10000f;
                // now that the list of foods is built we need to check which one is closest
                for (int i = 0; i < PlayerController.Stats.foodList.Count; i++)
                {
                    if(PlayerController.Stats.foodList[i] != null)
                    {
                        // check the distance between us and the food
                        newDistance = Vector3.Distance(PlayerController.transform.position, new Vector3(PlayerController.Stats.foodList[i].transform.position.x, PlayerController.transform.position.y, PlayerController.Stats.foodList[i].transform.position.z));

                        // if the distance is smaller than the previous distance
                        if(initialDistance >= newDistance)
                        {
                            // change the distance to show the current one
                            initialDistance = newDistance;
                            minimumDistanceIndex = i;             
                        }
                    }

                }
                // change our target location to the food location
                PlayerController.Movement.targetLocation = PlayerController.Stats.foodList[minimumDistanceIndex].transform.position;

                // change our target game object to the food game object
                PlayerController.Movement.target = PlayerController.Stats.foodList[minimumDistanceIndex].gameObject;

                // compute distance
                PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, new Vector3 (PlayerController.Movement.targetLocation.x, PlayerController.transform.position.y, PlayerController.Movement.targetLocation.z));

                // if we reach food
                if(PlayerController.Movement.targetDistance <= 2f)
                {
                    // hunger is 0
                    PlayerController.Stats.hunger = 0f;

                    if(entity.Species == Species.Food)
                    {
                        // instantiate new food
                        PlayerController.Instantiate(entity.gameObject, PlayerController.Movement.GameArea.GetRandomPosition(), Quaternion.identity);

                        // destroy food
                        if(entity.gameObject != null) PlayerController.Destroy(entity.gameObject);
                    }
                    else
                    {
                        //PlayerController.Stats.foodList[minimumDistanceIndex].gameObject.GetComponent<PlayerController>().Stats.health = 0f;
                        entity.health = 0f;
                    }
                    
                    // player target object is empty
                    PlayerController.Movement.target = null;

                    // change state
                    player.SwitchState(player.Patrolling);
                    

                }
            }
        }
        else 
        {
            // set blank list if no food in sight (avoids errors where food is left in list after is destroyed)
            PlayerController.Stats.foodList.Clear();
            PlayerController.Stats.foodList.TrimExcess();
        }

    }
    public override void OnTriggerExit(PlayerStateManager player, Collider other)
    {   
        // if we cannot see the food anymore, remove it from the list
        
        if(PlayerController.Stats.foodList.Contains(other.gameObject))
        {
            PlayerController.Stats.foodList.Remove(other.gameObject);
        }
    }
}
