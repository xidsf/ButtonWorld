using UnityEngine;

public class PlayerMoveState : PlayerGroundedState
{
    public override PlayerStateType GetName() { return PlayerStateType.Move; }
    
    Vector2 moveDir;

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyAnim.SetBool(runAnimString, true);
        moveDir = player.InputDir;
    }

    public override void OnMove(Vector2 dir)
    {
        base.OnMove(dir);
        moveDir = dir;
    }

    public override void OnMoveCanceled()
    {
        base.OnMoveCanceled();
        player.SwitchState(PlayerStateType.Idle);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.MyAnim.SetBool(runAnimString, false);
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ApplyMovement();
    }

    public override void OnEscape()
    {
        base.OnEscape();
        player.SwitchState(PlayerStateType.Idle);
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
