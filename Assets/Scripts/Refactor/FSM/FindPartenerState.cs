using UnityEngine;

public class FindPartenerState : BaseState 
{
    public override void EnterState(PlayerStateManager player)
    {
        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set random location
        if(!PlayerController.Stats.partenerFound) PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // check player stats
        CheckPlayerStats(player);
        
        // move player to target
        PlayerController.Movement.MoveToTarget(PlayerController.Movement.targetLocation);

        // set distance
        PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, PlayerController.Movement.targetLocation);

        // set a new random spot? 
        if(!PlayerController.Stats.partenerFound && PlayerController.Movement.targetDistance <= 1f)
        {
            PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
        }
    }

    public override void CheckPlayerStats(PlayerStateManager player)
    {
        // check hunger
        if(PlayerController.Stats.hunger > 50f)
        {
            // do we care about hunger? it might hurt if not. if we care we need to reset the player
            player.SwitchState(player.Hungry);
        }

        // check health
        if(PlayerController.Stats.health <= 0f)
        {
            player.SwitchState(player.Dead);
        }

    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other)
    {
        // if we trigger an object with the Stats component make this object as entity
        if(other.TryGetComponent<Stats>(out Stats entity))
        {
            // if the object we met it's the same species as us and we are opposite sex we go further
            if((entity.Species == PlayerController.Stats.Species) && ((entity.sex == 1 && PlayerController.Stats.sex == 0) || (entity.sex == 0 && PlayerController.Stats.sex == 1)))
            {
                // see if the entity we met it's willing to mate
                if(entity.GetComponent<PlayerStateManager>().currentState.ToString() == PlayerController.GetComponent<PlayerStateManager>().currentState.ToString() && !PlayerController.Stats.partenerFound && !entity.partenerFound)
                {
                    // change state to mating state?

                    // set parteners and all that
                    PlayerController.Stats.partenerFound = true;
                    entity.partenerFound = true;
                    entity.GetComponent<Movement>().target = PlayerController.gameObject;
                    PlayerController.Movement.target = entity.gameObject;

                    // check if we are parteners before mating?
                    if(PlayerController.Movement.target == entity.gameObject && entity.GetComponent<Movement>().target == PlayerController.gameObject)
                    {
                        // change state to mating?
                        player.SwitchState(player.Mating);
                        entity.GetComponent<PlayerStateManager>().SwitchState(entity.GetComponent<PlayerStateManager>().Mating);
                    }  
                }
            }
        }
    }
    public override void OnTriggerExit(PlayerStateManager player, Collider other)
    {
        
    }
}
