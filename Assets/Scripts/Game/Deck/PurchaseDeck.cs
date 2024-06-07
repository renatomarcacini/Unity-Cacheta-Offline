using Nato.PubSub;
using System;
using UnityEngine;
using static DiscardDeckEvents;
using static GameManagerEvents;

public class PurchaseDeck : DeckBase
{
    private bool humanPlayerCanGetCard;
    private PlayerBase currentPlayerTurn;

    [SerializeField] private DiscardDeck discardDeck;

    private void OnEnable()
    {
        EventManager<PlayerTurnChangedEvent>.Subscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Subscribe(HandlePlayerGetCardFromDeck);
    }

    private void OnDisable()
    {
        EventManager<PlayerTurnChangedEvent>.Unsubscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Unsubscribe(HandlePlayerGetCardFromDeck);

    }

    private void HandlePlayerTurnChanged(PlayerTurnChangedEvent playerTurnChangedEvent)
    {
        PlayerBase currentPlayer = playerTurnChangedEvent.PlayerTurn;
        currentPlayerTurn = currentPlayer;

        if (currentPlayer.IsHuman)
            humanPlayerCanGetCard = true;
        else
            humanPlayerCanGetCard = false;
    }

    private void HandlePlayerGetCardFromDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        humanPlayerCanGetCard = false;
    }

    private void OnMouseDown()
    {
        if (humanPlayerCanGetCard && currentPlayerTurn != null)
        {
            PlayerGetCardFromDeck(currentPlayerTurn);
        }
    }

    public void PlayerGetCardFromDeck(PlayerBase player)
    {
        Card card = GetLastCardFromDeck();
        CardsInGame.Add(card);
        PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent = new(player, card);
        EventManager<PlayerGetCardFromDeckEvent>.Publish(playerGetCardFromDeckEvent);
    }
}
