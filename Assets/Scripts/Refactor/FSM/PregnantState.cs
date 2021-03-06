using UnityEngine;
using System.Collections;
using System;

public class PregnantState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();

        // start coroutine
        PlayerController.StartCoroutine(DeliverBaby(player));

        // Change state
        player.SwitchState(player.Patrolling); 
    }
    public override void UpdateState(PlayerStateManager player)
    {
        // check player stats
        CheckPlayerStats(player);
    }

    public override void CheckPlayerStats(PlayerStateManager player)
    {
        // check health
        if(PlayerController.Stats.health <= 0f)
        {
            player.SwitchState(player.Dead);
        }
    }

    private IEnumerator DeliverBaby(PlayerStateManager player)
    {
        // wait 20s
        yield return new WaitForSeconds(20f);

        // deliver baby
        var baby = PlayerController.Instantiate(PlayerController.gameObject, PlayerController.transform.position, Quaternion.identity);

        // set baby stats
        if(!(baby.gameObject is null)) SetBabyStats(baby.gameObject);

        yield return new WaitForSeconds(50f);
        
        if(!(baby.gameObject is null))
        {
        baby.GetComponent<PlayerController>().Stats.reproductiveUrgeIncreaseRatio = 10f;
        }
    }

    private void SetBabyStats(GameObject baby) // #DEV
    {
        if(baby is null) return; 

        baby.GetComponent<PlayerController>().Stats.hunger = 0f;
        baby.GetComponent<PlayerController>().Stats.health = 100f;
        baby.GetComponent<PlayerController>().Stats.reproductiveUrge = 0f;
        baby.GetComponent<PlayerController>().Stats.generation++;



    }

    public override void OnTriggerStay(PlayerStateManager player, Collider other) {}
    public override void OnTriggerExit(PlayerStateManager player, Collider other) {}
}
