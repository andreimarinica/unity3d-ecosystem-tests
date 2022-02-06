using UnityEngine;
using System.Collections;

public class HornyState : BaseState
{
    private bool partenerFound = false;
    private bool DoitOnce = false;
    public override void EnterState(PlayerStateManager player)
    {
        partenerFound = false;
        DoitOnce = false;

        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set random location
        if(!partenerFound) PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
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
        if(!partenerFound && PlayerController.Movement.targetDistance <= 1f)
        {
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

    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other)
    {
        if(other.TryGetComponent<Stats>(out Stats entity))
        {
            Debug.Log("found entity");
            if((entity.Species == PlayerController.Stats.Species) && ((entity.sex == 1 && PlayerController.Stats.sex == 0) || (entity.sex == 0 && PlayerController.Stats.sex == 1)))
            {
                Debug.Log("found partener");
                partenerFound = true;
                if(entity.GetComponent<PlayerStateManager>().currentState.ToString() == PlayerController.GetComponent<PlayerStateManager>().currentState.ToString())
                {
                    Debug.Log("partener is horny as well :)");
                    //entity.GetComponent<Movement>().targetLocation = PlayerController.transform.position;
                    PlayerController.Movement.targetLocation = entity.transform.position;
                    PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, entity.transform.position);
                    if(PlayerController.Movement.targetDistance <= 4f)
                    {
                        PlayerController.Movement.targetLocation = PlayerController.transform.position;
                        if(!DoitOnce)
                        {
                            PlayerController.StartCoroutine(Mating(player));
                            DoitOnce = true;
                        } 
                        
                    }
                }
            }
        }
    }

    private IEnumerator Mating(PlayerStateManager player)
    {
        Debug.Log("waiting for 5 seconds");
        yield return new WaitForSeconds(5f);

        if(PlayerController.Stats.sex == 0) player.SwitchState(player.Patrolling);
        if(PlayerController.Stats.sex == 1) player.SwitchState(player.Pregnant);
    }


    public override void OnTriggerExit(PlayerStateManager player, Collider other)
    {
        
    }
}
