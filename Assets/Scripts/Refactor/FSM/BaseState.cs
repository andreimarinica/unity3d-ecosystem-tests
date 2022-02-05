using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnCollisionEnter(PlayerStateManager player);
    public abstract void OnTriggerStay(PlayerStateManager player, Collider other);
    public PlayerController PlayerController;
}
