using UnityEngine;

public class PlayerAirJumpState : PlayerAirState
{
    public override PlayerStateType GetName() { return PlayerStateType.AirJump; }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyAnim.SetTrigger(jumpAnimString);
    }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if (player.MyRigid.linearVelocityY <= 0)
        {
            player.SwitchState(PlayerStateType.AirFall);
        }
    }


}
