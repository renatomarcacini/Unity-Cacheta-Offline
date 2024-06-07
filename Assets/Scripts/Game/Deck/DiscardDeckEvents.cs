public class DiscardDeckEvents 
{
    public class CardAddedToDeckEvent
    {
        public Card Card {  get; private set; }
        public CardAddedToDeckEvent(Card card)
        {
            Card = card;
        }
    }

    public class PlayerGetCardFromDeckEvent
    {
        public PlayerBase Player { get; private set; }
        public Card Card { get; private set; }
        public PlayerGetCardFromDeckEvent(PlayerBase player, Card card)
        {
            Player = player;
            Card = card;
        }
    }
}
