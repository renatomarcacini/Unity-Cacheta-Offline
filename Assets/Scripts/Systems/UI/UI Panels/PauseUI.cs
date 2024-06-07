using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private Button resumeButton;
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        resumeButton.onClick.AddListener(OnButtonResumeClicked);
        menuButton.onClick.AddListener(OnButtonMenuClicked);
    }

    private void OnDisable()
    {
        resumeButton.onClick.RemoveListener(OnButtonResumeClicked);
        menuButton.onClick.RemoveListener(OnButtonMenuClicked);
    }

    private void OnButtonMenuClicked()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("01_Menu");
    }

    private void OnButtonResumeClicked()
    {
        UIMatchStateManager.Instance.StateMachine.TransitionTo<GameplayState>();
    }
}
