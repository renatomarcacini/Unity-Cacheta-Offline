using System;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuUI : BaseUI
{
    [Header("Buttons")]
    [SerializeField] private Button playButton;

    private void OnEnable()
    {
        playButton.onClick.AddListener(OnButtonPlayClicked);
    }

    private void OnDisable()
    {
        playButton.onClick.RemoveListener(OnButtonPlayClicked);
    }

    private void OnButtonPlayClicked()
    {
        SceneManager.LoadScene("02_Game");
    }
}
