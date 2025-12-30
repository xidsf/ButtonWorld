using UnityEngine;
using UnityEngine.Windows;

public class PlayerPlayingState : PlayerBaseState
{
    public override PlayerStateType GetName() { return PlayerStateType.Playing; }

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

    public override void OnRestart()
    {
        base.OnRestart();
        if (UIManager.Instance.IsOpenIngameMenu) return;
        if (GameManager.Instance.IsStageResetable)
        {
            player.MyInput.currentActionMap.Disable();
            GameManager.Instance.ResetCurrStage();
        }
    }
}
