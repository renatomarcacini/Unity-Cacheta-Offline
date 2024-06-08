using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class HandCheckerTest
{
    private Card CreateCard(Card.SUIT suit, Card.VALUE value)
    {
        GameObject cardObject = new GameObject("Card");
        Card cardComponent = cardObject.AddComponent<Card>();
        cardComponent.Suit = suit;
        cardComponent.Value = value;
        return cardComponent;
    }

    [Test]
    public void TestHandWithNineCards_Win()
    {
        HandChecker handChecker = new HandChecker();

        List<List<Card>> winningHands = new List<List<Card>>()
        {
            new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE ),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO ),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE ),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE ),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO ),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE ),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE ),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO ),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.SIX)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.SPADES,  Card.VALUE.THREE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.THREE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.FIVE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.SIX),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS,  Card.VALUE.TWO)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.JOKER),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.FIVE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.SIX),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS,  Card.VALUE.JOKER)
            }
        };

        foreach (var hand in winningHands)
        {
            bool hasWon = handChecker.CheckIfPlayerHasWon(hand) && handChecker.GetWinnerCards(hand).Count == 9;
            Assert.IsTrue(hasWon, $"The hand {string.Join(", ", hand)} should win");
        }
    }

    [Test]
    public void TestHandWithNineCards_Lose()
    {
        HandChecker handChecker = new HandChecker();

        List<List<Card>> losingHands = new List<List<Card>>()
        {
            new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.FIVE)
            },
              new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FIVE)
            },
        };

        foreach (var hand in losingHands)
        {
            bool hasWon = handChecker.CheckIfPlayerHasWon(hand) && handChecker.GetWinnerCards(hand).Count == 9;
            Assert.IsFalse(hasWon, $"The hand{string.Join(", ", hand)} shouldn't win");
        }
    }

    [Test]
    public void TestHandWithTenCards_Win()
    {
        HandChecker handChecker = new HandChecker();

        List<List<Card>> winningHands = new List<List<Card>>()
        {
            new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FIVE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FIVE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.SIX)
            },
        };

        foreach (var hand in winningHands)
        {
            bool hasWon = handChecker.CheckIfPlayerHasWon(hand) && handChecker.GetWinnerCards(hand).Count == 10;
            Assert.IsTrue(hasWon, $"The hand {string.Join(", ", hand)} should win");
        }
    }

    [Test]
    public void TestHandWithTenCards_Lose()
    {
        HandChecker handChecker = new HandChecker();

        List<List<Card>> losingHands = new List<List<Card>>()
        {
            new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.EIGHT)
            },
              new List<Card>()
            {
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.ACE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FIVE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
                CreateCard(Card.SUIT.HEART, Card.VALUE.NINE)
            },
            new List<Card>()
            {
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.ACE),
                CreateCard(Card.SUIT.SPADES, Card.VALUE.TWO),
                CreateCard(Card.SUIT.DIAMONDS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.CLUBS, Card.VALUE.TWO),
                CreateCard(Card.SUIT.HEART, Card.VALUE.THREE),
                CreateCard(Card.SUIT.HEART, Card.VALUE.FOUR),
            },
        };

        foreach (var hand in losingHands)
        {
            bool hasWon = handChecker.CheckIfPlayerHasWon(hand) && handChecker.GetWinnerCards(hand).Count == 10;
            Assert.IsFalse(hasWon, $"The hand{string.Join(", ", hand)} shouldn't win");
        }
    }
}
