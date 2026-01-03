using UnityEngine;

public class PlayerAirFallState : PlayerAirState
{
    public override PlayerStateType GetName() { return PlayerStateType.AirFall; }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        player.MyAnim.SetBool(fallingAnimString, true);
    }
    public override void OnExit()
    {
        base.OnExit();
        player.MyAnim.SetBool(fallingAnimString, false);
    }

}
