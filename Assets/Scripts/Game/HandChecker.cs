using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class HandChecker
{
    public enum GroupType
    {
        None,
        DOUBLES,
        TRIPLES,
        SEQUENCE
    }

    public class Group
    {
        public GroupType Type;
        public List<Card> Cards;

        public Group(GroupType type, List<Card> cards)
        {
            Type = type;
            Cards = cards;
        }
    }

    public List<Group> Groups { get; private set; }

    public class CardOrganizerResult
    {
        public List<Group> Groups { get; set; }
        public List<Card> RemainingCards { get; set; }
    }

    public CardOrganizerResult OrganizeCardsByValueThenSequence(List<Card> cards)
    {
        var groupsByValue = GetGroupsByValue(cards);
        var remainingCardsByValue = cards.Except(groupsByValue.SelectMany(g => g.Cards)).ToList();
        var groupsBySequence = GetGroupsBySequence(remainingCardsByValue);
        var remainingCardsBySequence = remainingCardsByValue.Except(groupsBySequence.SelectMany(g => g.Cards)).ToList();

        var groups = new List<Group>();
        groups.AddRange(groupsByValue);
        groups.AddRange(groupsBySequence);

        return new CardOrganizerResult
        {
            Groups = groups,
            RemainingCards = remainingCardsBySequence
        };
    }

    public CardOrganizerResult OrganizeCardsBySequenceThenValue(List<Card> cards)
    {
        var groupsBySequence = GetGroupsBySequence(cards);
        var remainingCardsBySequence = cards.Except(groupsBySequence.SelectMany(g => g.Cards)).ToList();
        var groupsByValue = GetGroupsByValue(remainingCardsBySequence);
        var remainingCardsByValue = remainingCardsBySequence.Except(groupsByValue.SelectMany(g => g.Cards)).ToList();

        var groups = new List<Group>();
        groups.AddRange(groupsBySequence);
        groups.AddRange(groupsByValue);

        return new CardOrganizerResult
        {
            Groups = groups,
            RemainingCards = remainingCardsByValue
        };
    }

    public List<Card> OrganizeCards(List<Card> cards)
    {
        var resultByValueThenSequence = OrganizeCardsByValueThenSequence(cards);
        var resultBySequenceThenValue = OrganizeCardsBySequenceThenValue(cards);

        List<Card> organizedCards;
        if (resultByValueThenSequence.RemainingCards.Count <= resultBySequenceThenValue.RemainingCards.Count)
        {
            Groups = resultByValueThenSequence.Groups;
            organizedCards = resultByValueThenSequence.Groups.SelectMany(g => g.Cards).ToList();
            organizedCards.AddRange(resultByValueThenSequence.RemainingCards);
        }
        else
        {
            Groups = resultBySequenceThenValue.Groups;
            organizedCards = resultBySequenceThenValue.Groups.SelectMany(g => g.Cards).ToList();
            organizedCards.AddRange(resultBySequenceThenValue.RemainingCards);
        }

        return organizedCards;
    }

    public bool CheckIfPlayerHasWon(List<Card> cards)
    {
        var resultByValueThenSequence = OrganizeCardsByValueThenSequence(cards);
        //DebugGroups(resultByValueThenSequence.Groups, resultByValueThenSequence.RemainingCards);
        int quantityValueThenSequence = resultByValueThenSequence.Groups.SelectMany(g => g.Cards).ToList().Count;

        if (resultByValueThenSequence.RemainingCards.Count == 0)
        {

            Groups = resultByValueThenSequence.Groups;
            return true;
        }
        else if (resultByValueThenSequence.RemainingCards.Count == 1 && quantityValueThenSequence == 9)
        {

            Groups = resultByValueThenSequence.Groups;
            return true;
        }


        var resultBySequenceThenValue = OrganizeCardsBySequenceThenValue(cards);
        //DebugGroups(resultBySequenceThenValue.Groups, resultBySequenceThenValue.RemainingCards);
        int quantitySequenceThenValue = resultBySequenceThenValue.Groups.SelectMany(g => g.Cards).ToList().Count;
        if (resultBySequenceThenValue.RemainingCards.Count == 0)
        {
            Groups = resultBySequenceThenValue.Groups;
            return true;
        }
        else if (resultBySequenceThenValue.RemainingCards.Count == 1 && quantitySequenceThenValue == 9)
        {
            Groups = resultBySequenceThenValue.Groups;
            return true;
        }

        return false;
    }

    public List<Card> GetWinnerCards(List<Card> cards)
    {
        var resultByValueThenSequence = OrganizeCardsByValueThenSequence(cards);
        int quantityValueThenSequence = resultByValueThenSequence.Groups.SelectMany(g => g.Cards).ToList().Count;

        if (resultByValueThenSequence.RemainingCards.Count == 0)
        {
            Groups = resultByValueThenSequence.Groups;
            return resultByValueThenSequence.Groups.SelectMany(g => g.Cards).ToList();
        }
        else if (resultByValueThenSequence.RemainingCards.Count == 1 && quantityValueThenSequence == 9)
        {
            Groups = resultByValueThenSequence.Groups;
            return resultByValueThenSequence.Groups.SelectMany(g => g.Cards).ToList();
        }

        var resultBySequenceThenValue = OrganizeCardsBySequenceThenValue(cards);
        int quantitySequenceThenValue = resultBySequenceThenValue.Groups.SelectMany(g => g.Cards).ToList().Count;
        if (resultBySequenceThenValue.RemainingCards.Count == 0)
        {
            return resultBySequenceThenValue.Groups.SelectMany(g => g.Cards).ToList();
        }
        else if (resultBySequenceThenValue.RemainingCards.Count == 1 && quantitySequenceThenValue == 9)
        {
            return resultBySequenceThenValue.Groups.SelectMany(g => g.Cards).ToList();
        }

        return cards;

    }

    //public List<Group> GetGroupsByValue(List<Card> cards)
    //{
    //    var groups = new List<Group>();
    //    var groupedByValue = cards.GroupBy(c => c.Value).ToList();

    //    foreach (var group in groupedByValue)
    //    {
    //        if (group.Count() >= 3)
    //        {
    //            var trincaCandidates = group.ToList();
    //            var validTrinca = new List<Card>();
    //            var naipesUsados = new HashSet<Card.SUIT>();

    //            foreach (var card in trincaCandidates)
    //            {
    //                if (validTrinca.Count < 3 && !naipesUsados.Contains(card.Suit))
    //                {
    //                    validTrinca.Add(card);
    //                    naipesUsados.Add(card.Suit);
    //                }
    //            }

    //            if (validTrinca.Count == 3)
    //            {
    //                var reaminingCards = cards.Except(validTrinca).ToList();
    //                foreach (var card in reaminingCards)
    //                {
    //                    if (naipesUsados.Contains(card.Suit) && validTrinca.Any(c => c.Value == card.Value && c.Suit == card.Suit))
    //                    {
    //                        validTrinca.Add(card);
    //                    }
    //                }
    //                groups.Add(new Group(GroupType.TRIPLES, validTrinca));
    //            }
    //        }
    //    }

    //    return groups;
    //}

    public List<Group> GetGroupsByValue(List<Card> cards)
    {
        var groups = new List<Group>();
        var groupedByValue = cards.Where(c => c.Value != Card.VALUE.JOKER).GroupBy(c => c.Value).ToList();
        var jokers = cards.Where(c => c.Value == Card.VALUE.JOKER).ToList();

        foreach (var group in groupedByValue)
        {
            if (group.Key == Card.VALUE.JOKER)
                continue;

            var trincaCandidates = group.ToList();
            var validTrinca = new List<Card>();
            var naipesUsados = new HashSet<Card.SUIT>();

            foreach (var card in trincaCandidates)
            {
                if (validTrinca.Count < 3 && !naipesUsados.Contains(card.Suit))
                {
                    validTrinca.Add(card);
                    naipesUsados.Add(card.Suit);
                }
            }

            while (validTrinca.Count < 3 && jokers.Count > 0)
            {
                validTrinca.Add(jokers[0]);
                jokers.RemoveAt(0);
            }

            if (validTrinca.Count == 3)
            {
                var remainingCards = cards.Except(validTrinca).ToList();
                foreach (var card in remainingCards)
                {
                    if (naipesUsados.Contains(card.Suit) && validTrinca.Any(c => c.Value == card.Value && c.Suit == card.Suit))
                    {
                        validTrinca.Add(card);
                    }
                }
                groups.Add(new Group(GroupType.TRIPLES, validTrinca));
            }
        }

        return groups;
    }


    //private List<Group> GetGroupsBySequence(List<Card> cards)
    //{
    //    var groups = new List<Group>();
    //    var cardsBySuit = cards.GroupBy(c => c.Suit);

    //    foreach (var suitGroup in cardsBySuit)
    //    {
    //        var sortedCards = suitGroup.OrderBy(c => c.Value).ToList();
    //        var sequence = new List<Card>();

    //        for (int i = 0; i < sortedCards.Count; i++)
    //        {
    //            if (sequence.Count == 0 || sortedCards[i].Value == sequence.Last().Value + 1)
    //            {
    //                sequence.Add(sortedCards[i]);
    //            }
    //            else
    //            {
    //                if (sequence.Count >= 3)
    //                {
    //                    groups.Add(new Group(GroupType.SEQUENCE, new List<Card>(sequence)));
    //                }
    //                sequence.Clear();
    //                sequence.Add(sortedCards[i]);
    //            }
    //        }

    //        if (sequence.Count >= 3)
    //        {
    //            groups.Add(new Group(GroupType.SEQUENCE, sequence));
    //        }
    //    }

    //    return groups;
    //}

    public List<Group> GetGroupsBySequence(List<Card> cards)
    {
        var groups = new List<Group>();
        var cardsBySuit = cards.Where(c => c.Value != Card.VALUE.JOKER).GroupBy(c => c.Suit).ToList();
        var jokers = cards.Where(c => c.Value == Card.VALUE.JOKER).ToList();

        foreach (var suitGroup in cardsBySuit)
        {
            var sortedCards = suitGroup.OrderBy(c => c.Value).ToList();
            var sequence = new List<Card>();

            for (int i = 0; i < sortedCards.Count; i++)
            {
                if (sequence.Count == 0 || sortedCards[i].Value == sequence.Last().Value + 1)
                {
                    sequence.Add(sortedCards[i]);
                }
                else if (jokers.Count > 0)
                {
                    while (jokers.Count > 0)
                    {
                        sequence.Add(jokers[0]);
                        jokers.RemoveAt(0);
                    }
                    sequence.Add(sortedCards[i]);
                }
                else
                {
                    if (sequence.Count >= 3)
                    {
                        groups.Add(new Group(GroupType.SEQUENCE, new List<Card>(sequence)));
                    }
                    sequence.Clear();
                    sequence.Add(sortedCards[i]);
                }
            }

            if (sequence.Count >= 3)
            {
                groups.Add(new Group(GroupType.SEQUENCE, sequence));
            }
        }

        return groups;
    }

    public bool CanFormTriples(List<Card> cards, Card newCard)
    {
        return CanFormGroupValueByQuantity(cards, newCard, quantity: 3);
    }

    public bool CanFormDoubles(List<Card> cards, Card newCard)
    {
        return CanFormGroupValueByQuantity(cards, newCard, quantity: 2);
    }

    private bool CanFormGroupValueByQuantity(List<Card> cards, Card newCard, int quantity)
    {
        var groupedCards = cards.GroupBy(card => card.Value);
        quantity = quantity - 1;
        foreach (var group in groupedCards)
        {
            if (group.Count() == quantity &&
                group.First().Value == newCard.Value &&
                group.All(c => c.Suit != newCard.Suit))
            {
                return true;
            }
            else if (group.Count() == quantity && newCard.Value == Card.VALUE.JOKER)
                return true;
        }
        return false;
    }

    public bool CouldFormPotentialTriples(List<Card> cards, Card card)
    {
        var groupedCards = cards.GroupBy(card => card.Value);
        foreach (var group in groupedCards)
        {
            if (group.Count() == 2 && group.Contains(card))
            {
                return true;
            }
            else if (card.Value == Card.VALUE.JOKER)
                return true;
        }
        return false;
    }

    private void DebugGroups(List<Group> group, List<Card> remainingCards)
    {
        for (int i = 0; i < group.Count; i++)
        {
            Debug.Log($"{group[i].Type}");

            for (int j = 0; j < group[i].Cards.Count; j++)
            {

                Debug.Log($"Card: {group[i].Cards[j].Value}_{group[i].Cards[j].Suit}");
            }
            Debug.Log($"--");

        }

        for (int i = 0; i < remainingCards.Count; i++)
        {
            Debug.Log($"REMAINING CARD: {remainingCards[i].Value}_{remainingCards[i].Suit}");
        }
        Debug.Log($"---------------END---------------");
    }
}
