using UnityEngine;

public class FleeState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {

    }
    public override void UpdateState(PlayerStateManager player)
    {
        // check player stats
        CheckPlayerStats(player);
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

    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other) {}
    public override void OnTriggerExit(PlayerStateManager player, Collider other) {}
}
