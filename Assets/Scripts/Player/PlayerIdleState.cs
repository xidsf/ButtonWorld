using UnityEngine;

public class PlayerIdleState : PlayerBaseState
{
    override public PlayerStateType GetName() { return PlayerStateType.Idle; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        GroundCheck();
    }

    public override void OnMoveStarted(Vector2 dir)
    {
        base.OnMoveStarted(dir);

        player.SwitchState(PlayerStateType.Move);
    }

    public override void OnJump()
    {
        base.OnJump();
        player.MyAnim.SetTrigger(jumpAnimString);
        player.MyRigid.linearVelocityY = player.JumpForce;
    }

    private void GroundCheck()
    {
        if (!player.MySensors.IsBottomContacted())
        {
            player.SwitchState(PlayerStateType.Air);
        }
    }
}
