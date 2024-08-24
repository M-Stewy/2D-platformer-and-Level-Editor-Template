using UnityEngine;

public class GrappleSpringState : GenericGrappleState
{
    public GrappleSpringState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }
    float xInput;
    public override void Checks()
    {
        base.Checks();
        xInput = player.inputHandler.moveDir.x;
    }

  

    public override void Enter()
    {
        base.Enter();
        ShootSwingPoint(playerData.SpringGrapDis, playerData.LaymaskSpringGrapple, false);
        player.spr.breakForce = playerData.SpringBreakF;
    }
    public override void Update()
    {
        base.Update();
        if (!player.spr.enabled)
        {
            player.lr.enabled = false;
            Debug.Log("Spring was destroyed");
            base.DestoryGrapPoints();
            playerStateMachine.ChangeState(player.inAirState);
        }
        if (graple)
        {
            player.spr.connectedAnchor = graple.transform.position;
            player.lr.SetPosition(1, graple.transform.position);
        }
        player.lr.SetPosition(0, player.hand.transform.position);
    }
    public override void FixedUpdate()
    {
        base.FixedUpdate();
        player.rb.AddForce(new Vector2(xInput * player.playerData.SpringGrapSpeed, 0));
    }
    public override void Exit()
    {
        base.Exit();
    }


    public override void ShootSwingPoint(float thisGrapDist, LayerMask thisGrapLayer, bool useDJ)
    {
        base.ShootSwingPoint(thisGrapDist, thisGrapLayer, useDJ);
    }
    public override void ConnectPlayerToPoint(GameObject point, bool useDJ)
    {
        base.ConnectPlayerToPoint(point, useDJ);
        player.spr.enabled = true;
        player.spr.distance = Vector2.Distance(player.transform.position, point.transform.position);
        player.spr.connectedAnchor = graple.transform.position;
    }

    public override void CreateGrapPoint(Vector2 point, Transform parentOBJ, bool useDJ)
    {
        base.CreateGrapPoint(point, parentOBJ, useDJ);
    }

    public override void DestoryGrapPoints()
    {
        base.DestoryGrapPoints();
        player.spr.enabled = false;
    }
    

  
}
