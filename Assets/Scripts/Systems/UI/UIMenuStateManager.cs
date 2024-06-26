using Nato.Singleton;
using Nato.StateMachine;

public class UIMenuStateManager : Singleton<UIMenuStateManager>
{

    public UIMenuPanels UIPanels;
    public StateMachine<UIMenuStateManager> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new(this);
    }

    private void Start()
    {
        StateMachine.TransitionTo<MenuState>();
    }

    private void Update()
    {
        StateMachine.OnTick();
    }
}
