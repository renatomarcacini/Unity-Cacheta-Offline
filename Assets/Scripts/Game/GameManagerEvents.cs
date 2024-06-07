public class GameManagerEvents 
{
    public class PlayerTurnChangedEvent
    {
        public PlayerBase PlayerTurn { get; private set; }

        public PlayerTurnChangedEvent(PlayerBase playerTurn)
        {
            PlayerTurn = playerTurn;
        }
    }

    public class CardDistributedEvent
    {
        public Card Card { get; private set; }
        public PlayerBase Player { get; private set; }
        public float CardMoveDuration { get; private set; }

        public CardDistributedEvent(Card card, PlayerBase player, float cardMoveDuration)
        {
            Card = card;
            Player = player;
            CardMoveDuration = cardMoveDuration;
        }
    }
}
