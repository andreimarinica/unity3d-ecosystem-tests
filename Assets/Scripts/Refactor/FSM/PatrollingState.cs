using UnityEngine;

public class PatrollingState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        PlayerController = player.GetComponent<PlayerController>();
        PlayerController.ManageCollision.StartCol();
        // move to random direction within bounds
        
    }
    public override void UpdateState(PlayerStateManager player)
    {
        //Debug.Log("Hello once a frame lol");
    }
    public override void OnCollisionEnter(PlayerStateManager player)
    {

    }
}
