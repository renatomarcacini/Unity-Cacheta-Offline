using Nato.PubSub;
using Nato.StateMachine;
using System;
using static PlayerEvents;

public class MatchEndState : BaseState<UIMatchStateManager>
{
    private void Awake()
    {
        EventManager<PlayerWonMatchEvent>.Subscribe(HandlePlayerWonMatch);

    }

    private void OnDestroy()
    {
        EventManager<PlayerWonMatchEvent>.Unsubscribe(HandlePlayerWonMatch);

    }

    private void HandlePlayerWonMatch(PlayerWonMatchEvent playerWonMatchEvent)
    {
        if (playerWonMatchEvent.Player.IsHuman)
            Manager.UIPanels.MatchEndUI.SetMatchResult("YOU WON");
        else
            Manager.UIPanels.MatchEndUI.SetMatchResult("YOU LOSE");
    }

    public override void OnStart(UIMatchStateManager manager)
    {
        base.OnStart(manager);
        Manager.UIPanels.MatchEndUI.Enable();
    }

    public override void OnEnd()
    {
        base.OnEnd();
        Manager.UIPanels.MatchEndUI.Disable();
    }

    public override void OnTick()
    {
        base.OnTick();
    }
}
