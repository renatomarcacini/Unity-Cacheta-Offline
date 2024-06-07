using DG.Tweening;
using Nato.PubSub;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static DiscardDeckEvents;
using static GameManagerEvents;
using static PlayerEvents;

public class HumanPlayerVisual : PlayerVisualBase
{
    [SerializeField] private Button knockButton;
    [SerializeField] private Button organizeCardsButton;

    protected override void OnEnable()
    {
        base.OnEnable();
        knockButton.onClick.AddListener(OnButtonKnockClicked);
        organizeCardsButton.onClick.AddListener(OnButtonOrganizeCardsClicked);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        knockButton.onClick.RemoveListener(OnButtonKnockClicked);
        organizeCardsButton.onClick.RemoveListener(OnButtonOrganizeCardsClicked);
    }

    private void OnButtonKnockClicked()
    {
        PlayerWonMatchEvent playerWonMatchEvent = new(player, player.Cards);
        EventManager<PlayerWonMatchEvent>.Publish(playerWonMatchEvent);
    }

    private void OnButtonOrganizeCardsClicked()
    {
        List<Card> organizedCards = player.HandChecker.OrganizeCards(player.Cards);
        player.RemakeListCard(organizedCards);
        hand.OrganizeHandVisual(player.Cards);
    }

    protected override void HandlePlayerTurnChanged(PlayerTurnChangedEvent playerTurnChangedEvent)
    {
        PlayerBase currentPlayer = playerTurnChangedEvent.PlayerTurn;
        if(player.IsPlayerMe(currentPlayer))
        {
            organizeCardsButton.interactable = true;
            hand.transform.DOLocalMoveY(0, 0.3f);
            EnableButtonIfPlayerHasWon();
        }
        else
        {
            knockButton.interactable = false;
            hand.transform.DOLocalMoveY(-0.5f, 0.3f);
        }
    }

    protected override void HandlePlayerGetCardFromDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        base.HandlePlayerGetCardFromDeck(playerGetCardFromDeckEvent);
        EnableButtonIfPlayerHasWon();

    }

    protected override void HandlePlayerCardRemoved(PlayerCardRemovedEvent playerCardRemovedEvent)
    {
        base.HandlePlayerCardRemoved(playerCardRemovedEvent);
        EnableButtonIfPlayerHasWon();
    }

    private void EnableButtonIfPlayerHasWon()
    {
        if (player.IsMyTurn)
        {
            knockButton.interactable = player.HandChecker.CheckIfPlayerHasWon(player.Cards);
        }
    }

    protected override void HandlePlayerWonMatch(PlayerWonMatchEvent playerWonMatchEvent)
    {
        base.HandlePlayerWonMatch(playerWonMatchEvent);
        PlayerBase winnerPlayer = playerWonMatchEvent.Player;
        if (player.IsPlayerMe(winnerPlayer))
        {
            knockButton.interactable = false;
            organizeCardsButton.interactable = false;
        }
    }
}
