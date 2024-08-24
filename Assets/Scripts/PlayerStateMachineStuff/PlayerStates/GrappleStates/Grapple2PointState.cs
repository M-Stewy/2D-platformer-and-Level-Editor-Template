using UnityEngine;
public class Grapple2PointState : GenericGrappleState
{
    public Grapple2PointState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
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
        ShootSwingPoint(playerData.Grap2Dis,playerData.LaymaskGrapple2, false);
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        
        if (startGrap && (currentDis > playerData.Grap2MaxDis))
        {
            if (player.rb.velocity.magnitude < playerData.Grap2MaxSpeed)
                player.rb.AddForce((graple.transform.position - player.transform.position).normalized * playerData.Graple2Speed, ForceMode2D.Impulse);
        }
    }

    public override void Update()
    {
        base.Update();

        if(startGrap)
        {
            player.lr.SetPosition(0, player.hand.transform.position);
            currentDis = (graple.transform.position - player.transform.position).magnitude;
        //    Debug.Log("Current Distance to point = " + currentDis);
            if (currentDis > playerData.Grap2MaxDis)
            {
                //player.rb.AddForce((graple.transform.position - player.transform.position) * playerData.Graple2Speed);
                //trying something
                player.rb.velocity = (graple.transform.position - player.transform.position).normalized * playerData.Grap2MaxSpeed;
            }
            if (currentDis <= playerData.Grap2MaxDis)
            {
                playerStateMachine.ChangeState(player.inAirState);
            }
        }
        
    }

    public override void ShootSwingPoint(float thisGrapDist, LayerMask thisGrapLayer, bool useDJ)
    {
        base.ShootSwingPoint(thisGrapDist, thisGrapLayer, useDJ);
        
    }
}
