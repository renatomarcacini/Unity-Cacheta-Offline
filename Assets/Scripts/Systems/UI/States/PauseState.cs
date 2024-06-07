using Nato.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseState : BaseState<UIMatchStateManager>
{
    public override void OnStart(UIMatchStateManager gameManager)
    {
        base.OnStart(gameManager);
        Manager.UIPanels.PauseUI.Enable();
        Time.timeScale = 0f;
    }

    public override void OnEnd()
    {
        base.OnEnd();
        Manager.UIPanels.PauseUI.Disable();
        Time.timeScale = 1f;
    }

    public override void OnTick()
    {
        base.OnTick();
    }
}
