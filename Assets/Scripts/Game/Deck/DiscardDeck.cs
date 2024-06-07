using Nato.PubSub;
using static CardEvents;
using static DiscardDeckEvents;
using static GameManagerEvents;

public class DiscardDeck : DeckBase
{
    private bool humanPlayerCanGetCard;
    private PlayerBase currentPlayerTurn;

    public Card LastCardOnDeck { get; private set; }

    private void OnEnable()
    {
        EventManager<CardDiscardEvent>.Subscribe(HandleCardDiscarded);
        EventManager<PlayerTurnChangedEvent>.Subscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Subscribe(HandlePlayerGetCardFromDeck);
    }

    private void OnDisable()
    {
        EventManager<CardDiscardEvent>.Unsubscribe(HandleCardDiscarded);
        EventManager<PlayerTurnChangedEvent>.Unsubscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Unsubscribe(HandlePlayerGetCardFromDeck);
    }

    private void HandleCardDiscarded(CardDiscardEvent cardDiscardEvent)
    {
        AddCardToDiscardDeck(cardDiscardEvent.Card);
    }

    public void AddCardToDiscardDeck(Card card)
    {
        LastCardOnDeck = card;
        card.ShowCard = true;
        card.CanDrag = false;
        card.CanDiscardToDiscardDeck = false;
        AddCardToDeck(card);
        CardAddedToDeckEvent cardAddedToDeckEvent = new(card);
        EventManager<CardAddedToDeckEvent>.Publish(cardAddedToDeckEvent);
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

    private void HandlePlayerGetCardFromDeck(PlayerGetCardFromDeckEvent @event)
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
        PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent = new(player, card);
        EventManager<PlayerGetCardFromDeckEvent>.Publish(playerGetCardFromDeckEvent);
    }
}
