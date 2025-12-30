using UnityEngine;

public class PlayerGroundedState : PlayerPlayingState
{
    public override PlayerStateType GetName() { return PlayerStateType.Grounded; }

    
    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        GroundCheck();
    }


    private void GroundCheck()
    {
        if (!player.MySensors.IsBottomContacted() || player.MyRigid.linearVelocityY > 0.05f)
        {
            if (player.MyRigid.linearVelocityY < 0f)
            {
                player.SwitchState(PlayerStateType.AirFall);
            }
            else
            {
                player.SwitchState(PlayerStateType.AirJump);
            }
        }
    }

    public override void OnJump()
    {
        base.OnJump();
        player.MyRigid.linearVelocityY = player.JumpForce;
        //player.SwitchState(PlayerStateType.AirJump);
    }
}
