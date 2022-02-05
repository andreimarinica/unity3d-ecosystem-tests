using UnityEngine;

public class HungryState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        Debug.Log("bummer i'm hungry");

        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set the list of foods to an empty list
        PlayerController.Stats.foodList.Clear();

        // set random destination to look foor food
        PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if(PlayerController.Stats.foodList.Count == 0)
        {
            // move to random
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
    }
    public override void OnTriggerStay(PlayerStateManager player, Collider other)
    {
        //Debug.Log("we see " + other.name);
        //get the items that have the stats class available
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
                // set our distance variable as a big value initially
                PlayerController.Movement.targetDistance = 1000f;
                
                // now that the list of foods is built we need to check which one is closest
                for (int i = 0; i < PlayerController.Stats.foodList.Count; i++)
                {
                    // null check ?

                    // check the distance etween us and the food
                    float newDistance = Vector3.Distance(PlayerController.transform.position, new Vector3(PlayerController.Stats.foodList[i].transform.position.x, PlayerController.transform.position.y, PlayerController.Stats.foodList[i].transform.position.z));

                    // if the distance is smaller than the previous distance
                    if(PlayerController.Movement.targetDistance >= newDistance)
                    {
                        // change the distance to show the current one
                        PlayerController.Movement.targetDistance = newDistance;

                        // change our target location to the food location
                        PlayerController.Movement.targetLocation = PlayerController.Stats.foodList[i].transform.position;

                        // change our target game object to the food game object
                        PlayerController.Movement.target = PlayerController.Stats.foodList[i].gameObject;
                    }

                }
                // if we have found food we move to the food
                PlayerController.Movement.MoveToTarget(PlayerController.Movement.targetLocation);

                // if we reach food
                if(PlayerController.Movement.targetDistance <= 1.5f)
                {
                    // hunger is 0
                    PlayerController.Stats.hunger = 0f;

                    // instantiate new food
                    PlayerController.InstantiateEntity(other.gameObject);

                    // destroy food
                    PlayerController.KillEntity(other.gameObject);

                    // change state
                    player.SwitchState(player.Patrolling);
                    

                }
            }
        }

    }
    public override void OnCollisionEnter(PlayerStateManager player)
    {
        
    }
}
