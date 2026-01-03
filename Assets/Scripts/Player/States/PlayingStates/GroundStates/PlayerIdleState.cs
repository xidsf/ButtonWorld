using UnityEngine;

public class PlayerIdleState : PlayerGroundedState
{
    override public PlayerStateType GetName() { return PlayerStateType.Idle; }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyRigid.linearVelocityX = 0;
    }

    public override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);
        if(player.MySensors.IsBottomContacted())
            player.SwitchState(PlayerStateType.Move);
    }
}
