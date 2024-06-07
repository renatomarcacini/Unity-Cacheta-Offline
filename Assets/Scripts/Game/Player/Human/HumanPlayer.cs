using System.Collections;
using UnityEngine;
using static CardEvents;
using static DiscardDeckEvents;

public class HumanPlayer : PlayerBase
{
    public override void Awake()
    {
        base.Awake();
        SetPlayStrategy(new HumanPlayStrategy());
    }

    public override void EnableTurn()
    {
        base.EnableTurn();
        EnableDragCards();
    }

    public override void DisableTurn()
    {
        base.DisableTurn();
        for (int i = 0; i < Cards.Count; i++)
            Cards[i].CanDiscardToDiscardDeck = false;
    }

    public void EnableDragCards()
    {
        foreach (Card card in Cards)
            card.CanDrag = true;
    }

    public void DisableDragCards()
    {
        foreach (Card card in Cards)
            card.CanDrag = false;
    }

    protected override void HandlePlayerGetCardFromDiscardDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        base.HandlePlayerGetCardFromDiscardDeck(playerGetCardFromDeckEvent);
        if (IsPlayerMe(playerGetCardFromDeckEvent.Player))
        {
            EnableDragCards();
            StartCoroutine(WaitHumanToPlayCoroutine());
        }
    }

    private IEnumerator WaitHumanToPlayCoroutine()
    {
        yield return new WaitForSeconds(0.3f);
        for (int i = 0; i < Cards.Count; i++)
            Cards[i].CanDiscardToDiscardDeck = true;
    }

    protected override void HandleCardDiscarded(CardDiscardEvent cardDiscardEvent)
    {
        base.HandleCardDiscarded(cardDiscardEvent);
        if (IsMyTurn)
        {
            for (int i = 0; i < Cards.Count; i++)
                Cards[i].CanDiscardToDiscardDeck = false;
            playStrategy.Play(this);
        }
    }

    protected override void HandlePlayerWonMatch(PlayerEvents.PlayerWonMatchEvent playerWonMatchEvent)
    {
        base.HandlePlayerWonMatch(playerWonMatchEvent);
        DisableDragCards();
    }
}

