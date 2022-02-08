using UnityEngine;
using System.Collections;

public class DeadState : BaseState
{
    public override void EnterState(PlayerStateManager player)
    {
        // get a ref to our player controller
        PlayerController = player.GetComponent<PlayerController>();
        // process death
        player.StartCoroutine(ProcessDeath(player));
    }

    private IEnumerator ProcessDeath(PlayerStateManager player)
    {
        // Make entity uninteractable
        PlayerController.GetComponent<Rigidbody>().isKinematic = true;
        PlayerController.GetComponent<Rigidbody>().useGravity = false;
        PlayerController.GetComponent<SphereCollider>().enabled = false;
        PlayerController.GetComponent<BoxCollider>().enabled = false;

        // 
        if(PlayerController.Stats.partenerFound && PlayerController.Movement.target != null)
        {
            PlayerController.Movement.target.GetComponent<Stats>().partenerFound = false;
        }

        // change the material to black
        PlayerController.GetComponent<MeshRenderer>().material = PlayerController.Stats.deathMat;

        // remove the visual range in order to remove the gizmo
        PlayerController.Movement.visualRange = 0f;

        // Wait for 5 seconds and destroy entity
        yield return new WaitForSeconds(PlayerController.Stats.timeBodyDissapear);
        PlayerController.Destroy(player.gameObject);

        
    }

    public override void UpdateState(PlayerStateManager player) {}
    public override void CheckPlayerStats(PlayerStateManager player) {}
    public override void OnTriggerStay(PlayerStateManager player, Collider other) {}
    public override void OnTriggerExit(PlayerStateManager player, Collider other) {}

}
