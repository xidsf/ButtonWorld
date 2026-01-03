using DG.Tweening;
using UnityEngine;

public class PlayerClearState : PlayerBaseState
{
    override public PlayerStateType GetName() { return PlayerStateType.Clear; }

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);
        OnCollisionGoal();
    }

    private void OnCollisionGoal()
    {
        UIManager.Instance.DisableIngameMenuUI();
        var center = player.GoalCollider.bounds.center;
        player.MyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        player.MyInput.currentActionMap.Disable();
        player.MyAnim.SetTrigger(clearAnimString);
        player.transform.DOMove(center, 1f).SetEase(Ease.OutQuart);
    }
}
