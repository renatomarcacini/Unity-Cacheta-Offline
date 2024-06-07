using Nato.Singleton;
using Nato.StateMachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIMatchStateManager : Singleton<UIMatchStateManager>
{

    public UIMatchPanels UIPanels;
    public StateMachine<UIMatchStateManager> StateMachine { get; private set; }

    protected override void Awake()
    {
        base.Awake();
        StateMachine = new(this);
    }

    private void Start()
    {
        StateMachine.TransitionTo<GameplayState>();
    }

    private void Update()
    {
        StateMachine.OnTick();
    }
}
