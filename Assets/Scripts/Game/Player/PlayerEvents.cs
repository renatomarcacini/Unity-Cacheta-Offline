using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEvents
{
    public class PlayerCardRemovedEvent
    {
        public PlayerBase Player { get; private set; }
        public Card Card { get; private set; }

        public PlayerCardRemovedEvent(PlayerBase player, Card card)
        {
            Player = player;
            Card = card;
        }
    }

    public class PlayerWonMatchEvent
    {
        public PlayerBase Player { get; private set; }
        public List<Card> Cards { get; private set; }

        public PlayerWonMatchEvent(PlayerBase player, List<Card> cards)
        {
            Player = player;
            Cards = cards;
        }
    }

    public class PlayerSubScoreEvent
    {
        public PlayerBase Player { get; private set;}
        public int Score { get; private set; }

        public PlayerSubScoreEvent(PlayerBase player, int score)
        {
            Player = player;
            Score = score;
        }
    }
}
