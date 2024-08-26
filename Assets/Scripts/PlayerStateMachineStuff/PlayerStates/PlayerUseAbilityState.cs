using UnityEngine;

/// <summary>
/// the foundation to how the ability system works
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
                playerStateMachine.ChangeState(player.inAirState);
                break;
            case "Grappling":
                playerStateMachine.ChangeState(player.grappleBState);
                break;
            case "Grapple2":
                playerStateMachine.ChangeState(player.grapple2State);
                break;
            case "InverseGrapple":
               playerStateMachine.ChangeState(player.inverseGrapple);
               break;
            case "SpringGrapple":
                playerStateMachine.ChangeState(player.springGrapple);
                break;
            case "DyanmicGrapple":
                playerStateMachine.ChangeState(player.dynamicGrapple);
                break;
            default:
                Debug.Log("This should not be Printing >:(");
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
