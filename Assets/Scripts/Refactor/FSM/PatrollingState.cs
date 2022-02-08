using UnityEngine;

public class PatrollingState : BaseState
{
    
    public override void EnterState(PlayerStateManager player)
    {
        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set random target
        PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
        
        
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // check player stats
        CheckPlayerStats(player);

        // move to the target that has been set
        PlayerController.Movement.MoveToTarget(PlayerController.Movement.targetLocation);

        // set the distance to the target
        PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, PlayerController.Movement.targetLocation);

        // check if we reached the target
        if(PlayerController.Movement.targetDistance <= 1f)
        {
            // if we reached target set a new random target
            PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
        }
    }

    public override void CheckPlayerStats(PlayerStateManager player)
    {
        // check hunger
        if(PlayerController.Stats.hunger > 50f)
        {
            player.SwitchState(player.Hungry);
        }

        // check health
        if(PlayerController.Stats.health <= 0f)
        {
            player.SwitchState(player.Dead);
        }

        // check reproductionUrge
        if(PlayerController.Stats.reproductiveUrge >= 50f)
        {
            player.SwitchState(player.FindPartener);
        }

    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other) {}
    public override void OnTriggerExit(PlayerStateManager player, Collider other) {}
}
