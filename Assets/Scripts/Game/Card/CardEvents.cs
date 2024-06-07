public class CardEvents 
{
    public class CardSwapEvent
    {
        public Card CardA {  get; private set; }
        public Card CardB { get; private set; }

        public CardSwapEvent(Card cardA, Card cardB)
        {
            CardA = cardA;
            CardB = cardB;
        }
    }

    public class CardDiscardEvent
    {
        public Card Card { get; private set; }

        public CardDiscardEvent(Card card)
        {
            Card = card;
        }
    }
}
