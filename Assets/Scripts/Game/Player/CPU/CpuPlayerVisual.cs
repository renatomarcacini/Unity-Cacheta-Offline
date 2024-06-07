using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerEvents;

public class CpuPlayerVisual : PlayerVisualBase
{
    [SerializeField] private GameObject turnEnableEffect;
    [SerializeField] private GameObject turnDisableEffect;

    protected override void HandlePlayerTurnChanged(PlayerTurnChangedEvent playerTurnChangedEvent)
    {
        base.HandlePlayerTurnChanged(playerTurnChangedEvent);
        turnEnableEffect.SetActive(false);
        turnDisableEffect.SetActive(false);

        PlayerBase currentPlayer = playerTurnChangedEvent.PlayerTurn;
        if (player.IsPlayerMe(currentPlayer))
            turnEnableEffect.SetActive(true);
        else
            turnDisableEffect.SetActive(true);
    }
}
