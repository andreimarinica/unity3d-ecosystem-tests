using UnityEngine;
using System.Collections;


public class MatingState : BaseState
{
    public bool onlyOnce = false;
    public bool reachedMate = false;
    public override void EnterState(PlayerStateManager player)
    {
        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        onlyOnce = false;
        reachedMate = false;
    }

    public override void UpdateState(PlayerStateManager player) 
    {
        // if we haven't reached the mate yet
        if(!reachedMate)
        {
            // and if our mate is still alive lol
            if(PlayerController.Movement.target != null)
            {
                // look at the mate and go towards it
                PlayerController.transform.LookAt(PlayerController.Movement.target.transform.position);
                PlayerController.Movement.MoveToTarget(PlayerController.Movement.target.transform.position);
            }
            else
            {
                // if our mate is dead we switch state to patrolling straight away
                player.SwitchState(player.Patrolling);
            }
            
        }
        // else if we have reached mate
        else
        {
            // since we are running a coroutine we want it to run only once so we use a bool checker
            if(!onlyOnce) 
            {
                // do something as we are facing each other
                PlayerController.StartCoroutine(Mating(player));
                onlyOnce = true;
            }
            
        }

        // again, making sure our mate is still alive
        if(PlayerController.Movement.target != null)
        {
            // get distance
            PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, PlayerController.Movement.target.transform.position);
        }

        // if mate dead switch state
        else
        {
            player.SwitchState(player.Patrolling);
        }
        
        // check distance between us and mate and make reachedMate true when this happens
        if(PlayerController.Movement.targetDistance <= 3f) reachedMate = true;
    }

    private IEnumerator Mating(PlayerStateManager player)
    {
        yield return new WaitForSeconds(5f);

        // after 5 seconds do the below
        PlayerController.Stats.partenerFound = false;
        PlayerController.Movement.target = null;
        PlayerController.Stats.reproductiveUrge = 0f;
        PlayerController.Stats.reproductiveUrgeIncreaseRatio = 0f;
        if(PlayerController.Stats.sex == 0) player.SwitchState(player.Patrolling);
        if(PlayerController.Stats.sex == 1) player.SwitchState(player.Pregnant);

        yield return new WaitForSeconds(30f);

        // and after 30 seconds we can start increasing our mating urge again
        PlayerController.Stats.reproductiveUrgeIncreaseRatio = 10f;

    }
    public override void CheckPlayerStats(PlayerStateManager player) {}
    public override void OnTriggerStay(PlayerStateManager player, Collider other) {}
    public override void OnTriggerExit(PlayerStateManager player, Collider other) {}

}
