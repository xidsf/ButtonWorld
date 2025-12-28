using UnityEngine;

public class PlayerMoveState : PlayerBaseState
{
    public override PlayerStateType GetName() { return PlayerStateType.Move; }
    
    Vector2 moveDir;


    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyAnim.SetBool(runAnimString, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.MyRigid.linearVelocityX = 0;
        player.MyAnim.SetBool(runAnimString, false);
    }


    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        GroundCheck();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();

        ApplyMovement();
    }

    public override void OnMovePerformed(Vector2 dir)
    {
        base.OnMoveStarted(dir);

        moveDir = dir;
    }

    public override void OnMoveCanceled()
    {
        base.OnMoveCanceled();
        player.SwitchState(PlayerStateType.Idle);

    }

    public override void OnJump()
    {
        base.OnJump();
        player.MyAnim.SetTrigger(jumpAnimString);
        player.MyRigid.linearVelocityY = player.JumpForce;
    }

    

    private void GroundCheck()
    {
        if(!player.MySensors.IsBottomContacted())
        {
            player.SwitchState(PlayerStateType.Air);
        }
    }

    private void ApplyMovement()
    {
        player.MyRigid.linearVelocityX = moveDir.x * player.MoveSpeed;
        if (moveDir.x > 0)
        {
            player.transform.localScale = new Vector3(1, 1, 1);
        }
        else if (moveDir.x < 0)
        {
            player.transform.localScale = new Vector3(-1, 1, 1);
        }
    }
}
