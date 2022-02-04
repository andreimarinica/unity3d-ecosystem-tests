using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerStateManager : MonoBehaviour
{
    [SerializeField] public PlayerController Player;
    BaseState currentState;
    public PatrollingState Patrolling = new PatrollingState();
    public HungryState Hungry = new HungryState();
    public HornyState Horny = new HornyState();
    public FleeState Flee = new FleeState();
    public DeadState Dead = new DeadState();
    public PregnantState Pregnant = new PregnantState();
    // Start is called before the first frame update
    void Start()
    {
        currentState = Patrolling;
        currentState.EnterState(this);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void SwitchState(BaseState state)
    {
        currentState = state;
        state.EnterState(this);
    }
}
