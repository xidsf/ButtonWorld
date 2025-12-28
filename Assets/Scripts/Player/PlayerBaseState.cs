using UnityEngine;

public enum PlayerStateType
{
    Idle,
    Move,
    Air,
    Death,
    Menu,
    Clear,
    Count
}

public abstract class PlayerBaseState
{
    protected PlayerController player;
    public abstract PlayerStateType GetName();

    protected string runAnimString = "isRun";
    protected string jumpAnimString = "isJump";
    protected string fallingAnimString = "isFalling";
    protected string airAnimString = "isAir";
    protected string deathAnimString = "isDeath";
    protected string clearAnimString = "isClear";

    public virtual void OnMoveStarted(Vector2 dir)
    {

    }

    public virtual void OnMovePerformed(Vector2 dir)
    {

    }

    public virtual void OnMoveCanceled()
    {

    }

    public virtual void OnJump()
    {

    }

    public virtual void OnInteract()
    {

    }

    public virtual void OnRestart()
    {

    }

    public virtual void OnEscape()
    {
    }

    public virtual void OnEnter(PlayerController player)
    {
        this.player = player;
    }

    public virtual void OnExit()
    {

    }

    public virtual void OnUpdate()
    {

    }

    public virtual void OnFixedUpdate()
    {
    }

    
}
