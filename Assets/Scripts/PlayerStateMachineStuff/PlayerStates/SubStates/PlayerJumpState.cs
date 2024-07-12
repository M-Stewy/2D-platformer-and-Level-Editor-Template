using System;
using System.Diagnostics;
/// <summary>
/// Made by Stewy
/// 
/// this state gives the player an upward force 
/// the force is applied until either the timer goes beyond its limit,
/// or the player stops pressing the jump button
/// </summary>
public class PlayerJumpState : PlayerState
{
    public PlayerJumpState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }

    float jumpTimer = 0;
    float xInput;
    float startXinput = 0;
    //I dont think Im doing this in a very smart way right now and its 3AM so I will 
    // do the rest of this tomorrow I think
    //      ^ this guy is a liar!


    public override void Checks()
    {
        base.Checks();

        if (player.inputHandler.PressedAbility1)
        {
            playerStateMachine.ChangeState(player.useAbilityState);
        }

    }

    public override void Enter()
    {
        base.Enter();

        // testing this currently
        player.PlayAudioFile(playerData.JumpSFX, true);

        player.rb.drag = playerData.AirDrag;

        jumpTimer = 0;
        startXinput = player.inputHandler.moveDir.x;
    }

    public override void Exit()
    {
        
        base.Exit();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate();
        //player.rb.AddForce(new UnityEngine.Vector2(xInput * playerData.baseMoveSpeed, 0));
        if (!player.inputHandler.HoldingJump)
        {
            playerStateMachine.ChangeState(player.inAirState);
            return;
        }

        jumpTimer++;
       

        if (jumpTimer > playerData.JumpTime)
        {
            playerStateMachine.ChangeState(player.inAirState);
            return;
        }

        /*if (player.inputHandler.holdingSprint)
            player.rb.AddForce(new UnityEngine.Vector2((xInput * playerData.baseMoveSpeed) / 100, playerData.JumpPower / 20), UnityEngine.ForceMode2D.Impulse);
        else
            player.rb.AddForce(new UnityEngine.Vector2((xInput * playerData.baseMoveSpeed) / 500, playerData.JumpPower / 20), UnityEngine.ForceMode2D.Impulse);
        */
        if(startXinput == 0)
            player.rb.AddForce(UnityEngine.Vector2.up * playerData.JumpPower, UnityEngine.ForceMode2D.Impulse);
       else
            player.rb.AddForce(UnityEngine.Vector2.up * (playerData.JumpPower / (playerData.baseMoveSpeed /10)), UnityEngine.ForceMode2D.Impulse);
        /*
            if (player.inputHandler.holdingSprint)
                player.rb.AddForce(new UnityEngine.Vector2(xInput * playerData.SprintSpeed/2, 0), UnityEngine.ForceMode2D.Force);
            else
                player.rb.AddForce(new UnityEngine.Vector2(xInput * playerData.baseMoveSpeed/10, 0), UnityEngine.ForceMode2D.Force);
        */
    }

    public override void Update()
    {
        base.Update();
        xInput = player.inputHandler.moveDir.x;
        
    }
}
