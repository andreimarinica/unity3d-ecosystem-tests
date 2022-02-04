using UnityEngine;

public class PatrollingState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        PlayerController = player.GetComponent<PlayerController>();
        //PlayerController.CollisionP.StartCol();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        //Debug.Log("Hello once a frame lol");
    }
    public override void OnCollisionEnter(PlayerStateManager player)
    {

    }
}
