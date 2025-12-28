using UnityEngine;

public class PlayerAirState : PlayerBaseState
{
    override public PlayerStateType GetName() { return PlayerStateType.Air; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        FallingCheck();
        GroundCheck();
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyAnim.SetBool(airAnimString, true);
    }

    private void FallingCheck()
    {
        bool isFall;
        var velocity = player.MyRigid.linearVelocityY;
        if(velocity < 0)
        {
            isFall = true;
        }
        else
        {
            isFall = false;
        }
        player.MyAnim.SetBool(fallingAnimString, isFall);
    }

    private void GroundCheck()
    {
        if (player.MySensors.IsBottomContacted())
        {
            player.SwitchState(PlayerStateType.Idle);
        }
    }

}
