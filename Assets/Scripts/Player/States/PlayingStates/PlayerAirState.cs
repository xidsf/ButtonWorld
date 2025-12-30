using UnityEngine;

public class PlayerAirState : PlayerPlayingState
{
    override public PlayerStateType GetName() { return PlayerStateType.Air; }

    Vector2 moveDir;
    
    protected const float jumpBufferTime = 0.1f;
    protected float jumpBufferTimeCounter = 0f;
    protected bool isJumpBuffered;

    protected const float coyoteTime = 0.1f;
    protected float coyoteTimeCounter = 0f;
    protected bool isCoyoteJumpable;

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        GroundCheck();
    }

    public override void OnUpdate()
    {
        base.OnUpdate();
        ApplyMovement();
        CalcCoyoteTime();
        CalcJumpBufferTime();
    }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        isJumpBuffered = false;
        isCoyoteJumpable = true;
        moveDir = player.InputDir;

        player.MyAnim.SetBool(airAnimString, true);
    }

    public override void OnExit()
    {
        base.OnExit();
        player.MyAnim.SetBool(airAnimString, false);
    }

    public override void OnMove(Vector2 inputDir)
    {
        base.OnMove(inputDir);  
        moveDir = inputDir;
    }

    public override void OnMoveCanceled()
    {
        base.OnMoveCanceled();
        moveDir = Vector2.zero;
    }

    public override void OnJump()
    {
        base.OnJump();
        if(isCoyoteJumpable)
        {
            player.MyRigid.linearVelocityY = player.JumpForce;
            player.SwitchState(PlayerStateType.AirJump);
            return;
        }
        isJumpBuffered = true;
        jumpBufferTimeCounter = jumpBufferTime;
    }

    public override void OnEscape()
    {
        base.OnEscape();
        moveDir = Vector2.zero;
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

    private void CalcCoyoteTime()
    {
        if (isCoyoteJumpable)
        {
            coyoteTimeCounter -= Time.deltaTime;
            if (coyoteTimeCounter <= 0f)
            {
                isCoyoteJumpable = false;
            }
        }
    }

    private void CalcJumpBufferTime()
    {
        if (isJumpBuffered)
        {
            jumpBufferTimeCounter -= Time.deltaTime;
            if (jumpBufferTimeCounter <= 0f)
            {
                isJumpBuffered = false;
            }
        }
    }

    private void GroundCheck()
    {
        if (player.MySensors.IsBottomContacted())
        {
            if(isJumpBuffered)
            {
                player.MyRigid.linearVelocityY = player.JumpForce;
                player.SwitchState(PlayerStateType.AirJump);
            }
            else
            {
                if (player.InputDir != Vector2.zero)
                {
                    player.SwitchState(PlayerStateType.Move);
                }
                else
                {
                    player.SwitchState(PlayerStateType.Idle);
                }
            }
        }
    }
}
