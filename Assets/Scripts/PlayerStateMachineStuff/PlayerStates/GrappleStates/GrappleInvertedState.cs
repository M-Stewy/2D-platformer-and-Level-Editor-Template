using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrappleInvertedState : GenericGrappleState
{
    public GrappleInvertedState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }
    LayerMask GrapHitted;
    float currentDis;
    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        missedGrap = false;
        startGrap = false;
        base.Enter();

        ShootSwingPoint(playerData.InverGrapDis, playerData.LaymaskInverGrapp, false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();

        if (startGrap)
        {
            if (player.rb.velocity.magnitude < playerData.InverGrapSpeed)
                player.rb.AddForce((graple.transform.position - player.transform.position).normalized * -playerData.InverGrapSpeed, ForceMode2D.Impulse);
        }
    }


    public override void Update()
    {
        base.Update();
        if (startGrap) {
            player.lr.SetPosition(0, player.hand.transform.position);
            currentDis = (graple.transform.position - player.transform.position).magnitude;
        }
        if (currentDis > playerData.InverMaxDis) {
            
            DestoryGrapPoints();
            playerStateMachine.ChangeState(player.inAirState);
        }
    }


    public override void ShootSwingPoint(float thisGrapDist, LayerMask thisGrapLayer, bool useDJ)
    {
        base.ShootSwingPoint(thisGrapDist, thisGrapLayer, useDJ);
    }
}
