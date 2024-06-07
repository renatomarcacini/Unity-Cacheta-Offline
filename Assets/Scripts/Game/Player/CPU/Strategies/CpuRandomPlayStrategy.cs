using UnityEngine;

public class CpuRandomPlayStrategy : IPlayStrategy
{
    public void ChooseMovement(PlayerBase player)
    {
        if (Random.value > 0.5f)
        {
            GameManager.Instance.PurchaseDeck.PlayerGetCardFromDeck(player);
        }
        else
        {
            GameManager.Instance.DiscardDeck.PlayerGetCardFromDeck(player);
        }
    }

    public void Play(PlayerBase player)
    {
        Card randomCard = player.Cards[Random.Range(0, player.Cards.Count)];
        randomCard.DiscardCard();
        GameManager.Instance.NextTurn();
    }
}
