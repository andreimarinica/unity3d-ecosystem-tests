using UnityEngine;
using System.Collections;

public class HornyState : BaseState
{
    //private bool partenerFound = false;
    private bool DoitOnce = false;
    public override void EnterState(PlayerStateManager player)
    {
        //partenerFound = false;
        DoitOnce = false;

        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // set random location
        if(!PlayerController.Stats.partenerFound) PlayerController.Movement.targetLocation = PlayerController.Movement.GameArea.GetRandomPosition();
    }
    public override void UpdateState(PlayerStateManager player)
    {
        if(PlayerController.Stats.partenerFound && PlayerController.Movement.target == null) PlayerController.Stats.partenerFound = false;
        // check player stats
        if(!PlayerController.Stats.partenerFound) CheckPlayerStats(player);
        
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
            if((entity.Species == PlayerController.Stats.Species) && ((entity.sex == 1 && PlayerController.Stats.sex == 0) || (entity.sex == 0 && PlayerController.Stats.sex == 1)))
            {
                if(entity.GetComponent<PlayerStateManager>().currentState.ToString() == PlayerController.GetComponent<PlayerStateManager>().currentState.ToString() && !PlayerController.Stats.partenerFound && !entity.partenerFound)
                {
                    PlayerController.Stats.partenerFound = true;
                    entity.partenerFound = true;

                    PlayerController.Movement.target = entity.gameObject;
                    entity.GetComponent<Movement>().target = PlayerController.gameObject;

                    PlayerController.Movement.targetLocation = entity.transform.position;
                }

                if(PlayerController.Stats.partenerFound && entity.partenerFound)
                {
                    PlayerController.Movement.targetDistance = Vector3.Distance(PlayerController.transform.position, entity.transform.position);
                    if(PlayerController.Movement.targetDistance <= 4.5f)
                    {
                        PlayerController.transform.LookAt(entity.transform);
                        entity.transform.LookAt(PlayerController.transform);
                        PlayerController.Movement.targetLocation = PlayerController.transform.position;
                        PlayerController.Stats.reproductiveUrge = 0f;
                        PlayerController.Stats.reproductiveUrgeIncreaseRatio = 0f;
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
        yield return new WaitForSeconds(5f);
        PlayerController.Stats.partenerFound = false;
        if(PlayerController.Stats.sex == 0) player.SwitchState(player.Patrolling);
        if(PlayerController.Stats.sex == 1) player.SwitchState(player.Pregnant);

        yield return new WaitForSeconds(30f);
        PlayerController.Stats.reproductiveUrgeIncreaseRatio = 10f;

    }


    public override void OnTriggerExit(PlayerStateManager player, Collider other)
    {
        
    }
}
