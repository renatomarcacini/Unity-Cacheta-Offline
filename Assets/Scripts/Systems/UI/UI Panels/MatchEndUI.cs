using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MatchEndUI : BaseUI
{
    [Header("Texts")]
    [SerializeField] private TextMeshProUGUI resultText;

    [Header("Buttons")]
    [SerializeField] private Button menuButton;

    private void OnEnable()
    {
        menuButton.onClick.AddListener(OnButtonMenuClicked);
    }

    private void OnDisable()
    {
        menuButton.onClick.RemoveListener(OnButtonMenuClicked);
    }

    private void OnButtonMenuClicked()
    {
        SceneManager.LoadScene("01_Menu");
    }

    internal void SetMatchResult(string resultText)
    {
        this.resultText.SetText(resultText);
    }
}
