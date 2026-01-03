using DG.Tweening;
using UnityEngine;
using UnityEngine.Windows;

public class PlayerPlayingState : PlayerBaseState
{
    public override PlayerStateType GetName() { return PlayerStateType.Playing; }

    public override void OnFixedUpdate()
    {
        base.OnFixedUpdate();
        if(player.MySensors.IsStucked())
        {
            bool isVirticallyStucked = (player.MySensors.IsTopContacted() && player.MySensors.IsBottomContacted());
            bool isHorizontallyStucked = (player.MySensors.IsRightContacted() && player.MySensors.IsLeftContacted());
            if (isVirticallyStucked)
            {
                player.transform.localScale = new Vector3(1, 0.9f, 1);
            }
            else if (isHorizontallyStucked)
            {
                player.transform.localScale = new Vector3(0.9f, 1, 1);

            }
            DOTween.KillAll();
            player.SwitchState(PlayerStateType.Death);
        }
    }

    public override void OnEscape()
    {
        base.OnEscape();
        if (!UIManager.Instance.IsOpenIngameMenu)
        {
            UIManager.Instance.EnableIngameMenuUI();
        }
        else
        {
            UIManager.Instance.DisableIngameMenuUI();
        }
    }

    public override void OnInteract()
    {
        base.OnInteract();

        float halfColliderWidth = player.MyColli.bounds.extents.x;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(player.MyColli.bounds.center, halfColliderWidth * 2f, player.ButtonLayerMask());
        foreach (Collider2D col in colliders)
        {
            ButtonController button = col.GetComponentInParent<ButtonController>();
            if (button != null)
            {
                button.ReleaseSameColorPressedButton();
            }
        }
    }

    public override void OnRestart()
    {
        base.OnRestart();
        if (GameManager.Instance.IsStageResetable)
        {
            player.MyInput.currentActionMap.Disable();
            GameManager.Instance.ResetCurrStage();
        }
    }
}
