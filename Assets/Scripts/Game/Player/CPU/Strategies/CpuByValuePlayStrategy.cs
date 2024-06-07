using System.Collections.Generic;
using System.Linq;

public class CpuByValuePlayStrategy : IPlayStrategy
{
    private PlayerBase player;

    public void ChooseMovement(PlayerBase player)
    {
        this.player = player;
        Card discardDeckCard = GameManager.Instance.DiscardDeck.LastCardOnDeck;

        if (player.HandChecker.CanFormTriples(player.Cards, discardDeckCard) ||
            player.HandChecker.CanFormDoubles(player.Cards, discardDeckCard))
            GameManager.Instance.DiscardDeck.PlayerGetCardFromDeck(player);
        else
            GameManager.Instance.PurchaseDeck.PlayerGetCardFromDeck(player);
    }

    public void Play(PlayerBase player)
    {
        this.player = player;
        Card uselessCard = GetUselessCard(player.Cards);
        uselessCard.DiscardCard();
        GameManager.Instance.NextTurn();
    }

    private Card GetUselessCard(List<Card> cards)
    {
        var groups = player.HandChecker.GetGroupsByValue(cards);
        var tripleCards = groups.Where(g => g.Type == HandChecker.GroupType.TRIPLES)
                                .SelectMany(g => g.Cards)
                                .ToList();

        foreach (var card in cards)
        {
            if (!tripleCards.Contains(card))
            {
                if (!player.HandChecker.CouldFormPotentialTriples(cards, card))
                {
                    return card;
                }
            }
        }

        foreach (var card in cards)
        {
            if (!tripleCards.Contains(card))
            {
                if (player.HandChecker.CouldFormPotentialTriples(cards, card))
                {
                    return card;
                }
            }
        }

        return cards.LastOrDefault();
    }
}
