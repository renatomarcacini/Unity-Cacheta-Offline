using System;
using UnityEngine;
using UnityEngine.UI;

public class GameplayUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private Button pauseButton;

    private void OnEnable()
    {
        pauseButton.onClick.AddListener(OnButtonPauseClicked);
    }

    private void OnDisable()
    {
        pauseButton.onClick.RemoveListener(OnButtonPauseClicked);
    }

    private void OnButtonPauseClicked()
    {
        UIMatchStateManager.Instance.StateMachine.TransitionTo<PauseState>();
    }

    public override void Enable()
    {
        base.Enable();

    }

    public override void Disable()
    {
        base.Disable();
    }

}
