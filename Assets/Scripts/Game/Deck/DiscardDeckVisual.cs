using Nato.PubSub;
using UnityEngine;
using static DiscardDeckEvents;

public class DiscardDeckVisual : MonoBehaviour
{
    private DiscardDeck discardDeck;
    [SerializeField] private HandVisual hand;

    private void Awake()
    {
        discardDeck = GetComponent<DiscardDeck>();
    }

    private void OnEnable()
    {
        EventManager<CardAddedToDeckEvent>.Subscribe(HandleCardAddedToDeck);
    }

    private void OnDisable()
    {
        EventManager<CardAddedToDeckEvent>.Unsubscribe(HandleCardAddedToDeck);
    }

    private void HandleCardAddedToDeck(CardAddedToDeckEvent cardAddedToDeckEvent)
    {
        hand.OrganizeHandVisual(discardDeck.Cards);
    }
}
