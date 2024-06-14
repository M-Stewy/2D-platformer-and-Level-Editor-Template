using UnityEngine;

/// <summary>
/// Made by Stewy
///     Edited by:
/// 
/// Very unfinshed but serves as a foundation to how our ability system will (hopefull) work
/// we need to dicuss this further I think
/// </summary>
public class PlayerUseAbilityState : PlayerState
{
    public PlayerUseAbilityState(Player player, PlayerData playerData, PlayerStateMachine playerStateMachine, string playerAnim) : base(player, playerData, playerStateMachine, playerAnim)
    {
    }

    public override void Checks()
    {
        base.Checks();
    }

    public override void Enter()
    {
        base.Enter();

        switch (player.CurrentAbility.name)
        {
            case "NoAbility":
                playerStateMachine.ChangeState(player.idleState);
                break;
            case "Grappling":
                playerStateMachine.ChangeState(player.grapplingState);
                break;
            case "Grapple2":
                playerStateMachine.ChangeState(player.grapple2State);
                break;
                //case "Gun":
                //   playerStateMachine.ChangeState(player.shootGunState);
                //  break;
            default:
                Debug.Log("This should not be Printing");
                playerStateMachine.ChangeState(player.idleState);
                break;
        }
        
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void FixedUpdate()
    {
        base.FixedUpdate(); 
    }
}
