using Nato.PubSub;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using static CardEvents;
using static DiscardDeckEvents;
using static GameManagerEvents;
using static PlayerEvents;

public class PlayerVisualBase : MonoBehaviour
{
    protected PlayerBase player;
    [SerializeField] protected HandVisual hand;
    [SerializeField] private TextMeshProUGUI scoreText;

    private void Awake()
    {
        player = GetComponent<PlayerBase>();
    }

    protected virtual void OnEnable()
    {
        EventManager<CardDistributedEvent>.Subscribe(HandleCardDistributed);
        EventManager<CardSwapEvent>.Subscribe(HandleCardSwaped);
        EventManager<PlayerCardRemovedEvent>.Subscribe(HandlePlayerCardRemoved);
        EventManager<PlayerTurnChangedEvent>.Subscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Subscribe(HandlePlayerGetCardFromDeck);
        EventManager<PlayerWonMatchEvent>.Subscribe(HandlePlayerWonMatch);
        EventManager<PlayerSubScoreEvent>.Subscribe(HandlePlayerSubScore);
    }

    protected virtual void OnDisable()
    {
        EventManager<CardDistributedEvent>.Unsubscribe(HandleCardDistributed);
        EventManager<CardSwapEvent>.Unsubscribe(HandleCardSwaped);
        EventManager<PlayerCardRemovedEvent>.Unsubscribe(HandlePlayerCardRemoved);
        EventManager<PlayerTurnChangedEvent>.Unsubscribe(HandlePlayerTurnChanged);
        EventManager<PlayerGetCardFromDeckEvent>.Unsubscribe(HandlePlayerGetCardFromDeck);
        EventManager<PlayerWonMatchEvent>.Unsubscribe(HandlePlayerWonMatch);
        EventManager<PlayerSubScoreEvent>.Unsubscribe(HandlePlayerSubScore);
    }



    private void HandleCardSwaped(CardSwapEvent swapCardEvent)
    {
        if(!player.IsCardOwnedByPlayer(player, swapCardEvent.CardA))
            return;

        if (swapCardEvent.CardA != null && swapCardEvent.CardB != null)
            MoveListCard(swapCardEvent.CardA, swapCardEvent.CardB);

        hand.OrganizeHandVisual(player.Cards);
    }

    public void MoveListCard(Card cardA, Card cardB)
    {
        if (ReferenceEquals(cardA, cardB) || cardA == null || cardB == null)
        {
            return;
        }

        List<Card> auxList = player.Cards;

        if (!auxList.Contains(cardA) || !auxList.Contains(cardB))
        {
            return;
        }

        int indexA = auxList.IndexOf(cardA);
        int indexB = auxList.IndexOf(cardB);

        auxList.RemoveAt(indexA);
        auxList.Insert(indexB, cardA);
        player.RemakeListCard(auxList);
    }

    private void HandleCardDistributed(CardDistributedEvent cardDistributedEvent)
    {
        if (!player.IsPlayerMe(cardDistributedEvent.Player))
            return;

        hand.OrganizeHandVisual(player.Cards);

        if (player.IsHuman)
            cardDistributedEvent.Card.ShowCard = true;
        else
            cardDistributedEvent.Card.ShowCard = false;

        if (player.Cards.Count == 9)
        {
            player.RemakeListCard(player.HandChecker.OrganizeCards(player.Cards));
            hand.OrganizeHandVisual(player.Cards);
        }
    }

    protected virtual void HandlePlayerCardRemoved(PlayerCardRemovedEvent playerCardRemovedEvent)
    {
        PlayerBase currentPlayer = playerCardRemovedEvent.Player;
        if (player.IsPlayerMe(currentPlayer))
            hand.OrganizeHandVisual(player.Cards);
    }

    protected virtual void HandlePlayerTurnChanged(PlayerTurnChangedEvent playerTurnChangedEvent)
    {
        
    }

    protected virtual void HandlePlayerGetCardFromDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        PlayerBase currentPlayer = playerGetCardFromDeckEvent.Player;
        Card card = playerGetCardFromDeckEvent.Card;
        if (player.IsPlayerMe(currentPlayer))
        {
            hand.OrganizeHandVisual(player.Cards);
            card.ShowCard = currentPlayer.IsHuman;

            if (!player.IsHuman)
            {
                player.RemakeListCard(player.HandChecker.OrganizeCards(player.Cards));
                hand.OrganizeHandVisual(player.Cards);
            }
        }
    }

    protected virtual void HandlePlayerWonMatch(PlayerWonMatchEvent playerWonMatchEvent)
    {
        PlayerBase winnerPlayer = playerWonMatchEvent.Player;
        if (player.IsPlayerMe(winnerPlayer))
        {
            List<Card> cards = playerWonMatchEvent.Cards;

            List<Card> organizedCards = winnerPlayer.HandChecker.OrganizeCards(cards);
            foreach(var card in organizedCards)
            {
                card.ShowCard = true;
                card.ShowOutline(Color.blue);
            }
       
            GameManager.Instance.WinnerHand.OrganizeHandVisual(organizedCards);
        }
    }

    private void HandlePlayerSubScore(PlayerSubScoreEvent playerSubScoreEvent)
    {
        PlayerBase currentPlayer = playerSubScoreEvent.Player; 
        int score = playerSubScoreEvent.Score;
        if (player.IsPlayerMe(currentPlayer))
        {
            scoreText.SetText(score.ToString());
        }
    }
}
