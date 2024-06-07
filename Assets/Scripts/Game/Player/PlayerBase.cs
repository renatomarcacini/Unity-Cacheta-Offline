using Nato.PubSub;
using System.Collections.Generic;
using UnityEngine;
using static CardEvents;
using static DiscardDeckEvents;
using static GameManagerEvents;
using static PlayerEvents;

public abstract class PlayerBase : MonoBehaviour
{
    [field: SerializeField] public List<Card> Cards { get; private set; }
    [field: SerializeField] public int Score{ get; private set; }

    [field: SerializeField] public bool IsHuman { get; private set; }
    [field: SerializeField] public bool IsMyTurn { get; private set; }
    [field: SerializeField] public HandChecker HandChecker { get; private set; }


    [SerializeField] protected IPlayStrategy playStrategy;

    public virtual void Awake()
    {
        gameObject.name = Random.Range(0, 100).ToString();
        Cards = new List<Card>();
        HandChecker = new HandChecker();
    }

    public void SetPlayStrategy(IPlayStrategy strategy)
    {
        playStrategy = strategy;
    }

    private void OnEnable()
    {
        EventManager<CardDistributedEvent>.Subscribe(HandleCardDistributed);
        EventManager<CardDiscardEvent>.Subscribe(HandleCardDiscarded);
        EventManager<PlayerTurnChangedEvent>.Subscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Subscribe(HandlePlayerGetCardFromDiscardDeck);
        EventManager<PlayerWonMatchEvent>.Subscribe(HandlePlayerWonMatch);
    }


    private void OnDisable()
    {
        EventManager<CardDistributedEvent>.Unsubscribe(HandleCardDistributed);
        EventManager<CardDiscardEvent>.Unsubscribe(HandleCardDiscarded);
        EventManager<PlayerTurnChangedEvent>.Unsubscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Unsubscribe(HandlePlayerGetCardFromDiscardDeck);
        EventManager<PlayerWonMatchEvent>.Unsubscribe(HandlePlayerWonMatch);
    }

    private void HandleCardDistributed(CardDistributedEvent cardDistributedEvent)
    {
        if (!IsPlayerMe(cardDistributedEvent.Player))
            return;

        Cards.Add(cardDistributedEvent.Card);
    }

    protected virtual void HandleCardDiscarded(CardDiscardEvent cardDiscardEvent)
    {
        if (!IsCardOwnedByPlayer(this, cardDiscardEvent.Card))
            return;

        Cards.Remove(cardDiscardEvent.Card);

        PlayerCardRemovedEvent playerCardRemovedEvent = new(this, cardDiscardEvent.Card);
        EventManager<PlayerCardRemovedEvent>.Publish(playerCardRemovedEvent);
    }

    private void HandlePlayerTurnChanged(PlayerTurnChangedEvent playerTurnChangedEvent)
    {
        DisableTurn();
        if (IsPlayerMe(playerTurnChangedEvent.PlayerTurn))
        {
            EnableTurn();
        }
    }

    protected virtual void HandlePlayerGetCardFromDiscardDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        PlayerBase currentPlayer = playerGetCardFromDeckEvent.Player;
        if (IsPlayerMe(currentPlayer))
        {
            Cards.Add(playerGetCardFromDeckEvent.Card);
        }
    }
    protected virtual void HandlePlayerWonMatch(PlayerWonMatchEvent playerWonMatchEvent)
    {
        DisableTurn();
    }

    public bool IsCardOwnedByPlayer(PlayerBase player, Card card)
    {
        return player.Cards.Contains(card);
    }

    public bool IsPlayerMe(PlayerBase player)
    {
        return player == this;
    }

    public void RemakeListCard(List<Card> cards)
    {
        Cards = cards;
    }

    public void AddScore(int value)
    {
        Score += value;
        PlayerSubScoreEvent playerSubScoreEvent = new(this, Score);
        EventManager<PlayerSubScoreEvent>.Publish(playerSubScoreEvent);
    }

    public void SubScore(int value)
    {
        Score -= value;
        PlayerSubScoreEvent playerSubScoreEvent = new(this, Score);
        EventManager<PlayerSubScoreEvent>.Publish(playerSubScoreEvent);
    }

    public void SetScore(int value)
    {
        Score = value;
        PlayerSubScoreEvent playerSubScoreEvent = new(this, Score);
        EventManager<PlayerSubScoreEvent>.Publish(playerSubScoreEvent);
    }

    public virtual void EnableTurn()
    {
        IsMyTurn = true;
    }

    public virtual void DisableTurn()
    {
        IsMyTurn = false;
    }

    public void SetMyTurn(bool isMyTurn)
    {
        IsMyTurn = isMyTurn;
    }
}
