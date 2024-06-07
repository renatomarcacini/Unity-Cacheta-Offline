using Nato.StateMachine;
using UnityEngine;

public class GameplayState : BaseState<UIMatchStateManager>
{
    public override void OnStart(UIMatchStateManager gameManager)
    {
        base.OnStart(gameManager);

        Manager.UIPanels.GameplayUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();

        Manager.UIPanels.GameplayUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();
    }
}