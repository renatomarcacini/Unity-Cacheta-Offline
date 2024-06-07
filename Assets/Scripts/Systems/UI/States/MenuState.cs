using Nato.StateMachine;

public class MenuState : BaseState<UIMenuStateManager>
{
    public override void OnStart(UIMenuStateManager manager)
    {
        base.OnStart(manager);
        Manager.UIPanels.MenuUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();

        Manager.UIPanels.MenuUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();
    }
}
