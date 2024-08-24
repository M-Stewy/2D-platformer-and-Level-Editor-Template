using UnityEngine;

public class BasicSwingGrappleState : GenericGrappleState
{
    public BasicSwingGrappleState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
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
        missedGrap = false;
        base.Enter();
        player.CurrentAbility.DoAction(player.hand.gameObject, true);
        ShootSwingPoint(playerData.GrappleDistance, playerData.LaymaskGrapple, true);
        //    GrapHitAble = playerData.LaymaskGrapple;
        //Debug.Log(missedGrap);
        //Debug.Log("GrapHit is currently: " + playerData.LaymaskGrapple.value);
        //Debug.Log("GroundLayer is currently: " + playerData.GroundLayer.value);
    }

    public override void Exit()
    {
        base.Exit();
        DestoryGrapPoints();
        player.CurrentAbility.DoAction(player.hand.gameObject, false);

        player.StopAudioFile(playerData.AirWooshSFX);
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        if (!graple) return;
        if (player.inputHandler.HoldingUp)
        {
            player.dj.distance -= playerData.GrappleReelSpeed * Time.deltaTime;
        }

        if (player.transform.position.y >= graple.transform.position.y + playerData.GrappleYTolarance) return; //can still pull closer if above point but not move further/L+R

        if (player.inputHandler.HoldingDown)
        {
            if (player.dj.distance > playerData.GrappleDistance)
                return;
            player.dj.distance += playerData.GrappleReelSpeed * Time.deltaTime;
        }

        player.rb.AddForce(new Vector2(xInput * player.playerData.GrappleSwingSpeed, 0));

        
        

    }

    public override void Update()
    {
        base.Update();
        

        if (!graple) return;

        base.SnapRope(playerData.GrappleDistance);
        player.dj.connectedAnchor = graple.transform.position;
        player.lr.SetPosition(1, graple.transform.position);
        player.lr.SetPosition(0, player.hand.transform.position);

        if (player.rb.velocity.magnitude > 15 || player.rb.velocity.magnitude < -15)
        {
            if (!player.audioS.isPlaying)
            {
                player.PlayAudioFile(playerData.AirWooshSFX, true);
            }
        }
        else
        {
            player.StopAudioFile(playerData.AirWooshSFX);
        }
        
    }

    public override void ShootSwingPoint(float thisGrapDist, LayerMask thisGrapLayer, bool useDJ)
    {
        base.ShootSwingPoint(thisGrapDist, thisGrapLayer, useDJ);

    }

    

    
}
