using UnityEngine;

public abstract class BaseState
{
    public abstract void EnterState(PlayerStateManager player);
    public abstract void UpdateState(PlayerStateManager player);
    public abstract void OnTriggerExit(PlayerStateManager player, Collider other);
    public abstract void OnTriggerStay(PlayerStateManager player, Collider other);
    public PlayerController PlayerController;
}
