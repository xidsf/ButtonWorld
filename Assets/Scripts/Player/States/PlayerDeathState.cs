using System.Collections;
using UnityEngine;

public class PlayerDeathState : PlayerBaseState
{
    override public PlayerStateType GetName() { return PlayerStateType.Death; }

    private Coroutine deathCoroutine;

    const string deathString = "isDeath";

    public override void OnEnter(PlayerController player)
    {
        base.OnEnter(player);

        deathCoroutine = player.StartCoroutine(DeathCoroutine());

    }

    public override void OnExit()
    {
        base.OnExit();
        player.StopCoroutine(deathCoroutine);
    }

    IEnumerator DeathCoroutine()
    {
        UIManager.Instance.DisableIngameMenuUI();
        player.MyRigid.constraints = RigidbodyConstraints2D.FreezeAll;
        player.MyAnim.SetTrigger(deathString);
        player.MyInput.currentActionMap.Disable();
        yield return new WaitForSeconds(0.5f);
        //onDeath?.Invoke(false);
    }
}
