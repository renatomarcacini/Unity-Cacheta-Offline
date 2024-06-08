using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Class representing a deck of playing cards. Creating, shuffling, and managing the deck.
/// </summary>
public class DeckBase : MonoBehaviour
{
    [SerializeField] private Card cardPrefab;
    [SerializeField] private Vector3 offset;
    [field: SerializeField] public DeckData DeckData { get; private set; }
    [field: SerializeField]  public List<Card> Cards { get; private set; }
    [field: SerializeField]  public List<Card> CardsInGame { get; private set; }

    /// <summary>
    /// Creates a deck of cards, in which a deck represents 52 cards.
    /// </summary>
    /// <param name="quantity">The number of decks to create.</param>
    public void MakeDeck(int quantity = 1)
    {
        Vector3 offsetCard = transform.position;

        for (int q = 0; q < quantity; q++)
        {

            int i = 0;
            foreach (Card.SUIT suit in System.Enum.GetValues(typeof(Card.SUIT)))
            {
                foreach (Card.VALUE value in System.Enum.GetValues(typeof(Card.VALUE)))
                {
                    if (value == Card.VALUE.NOTHING || value == Card.VALUE.JOKER)
                        continue;
                    Card card = Instantiate(cardPrefab.gameObject, offsetCard, transform.rotation).GetComponent<Card>();
                    card.Setup(suit, value, DeckData.Sprite_CardBack, DeckData.Sprites_CardFront[i]);
                    card.ShowCard = false;
                    AddCardToDeck(card);
                    i++;
                    offsetCard.y += offset.y;
                    offsetCard.x += offset.x;
                    offsetCard.z += offset.z;
                    card.transform.SetParent(transform);
                }
            }
        }
    }

    /// <summary>
    /// Organizes the position of cards in the deck.
    /// </summary>
    public void OrganizeCardsPositionInDeck()
    {
        Vector3 offsetCard = transform.position;

        for (int i = 0; i < Cards.Count; i++)
        {
            offsetCard.y += offset.y;
            offsetCard.x += offset.x;
            offsetCard.z += offset.z;
            Cards[i].transform.position = offsetCard;
            Cards[i].transform.SetParent(transform);
            Cards[i].ShowCard = false;
            Cards[i].DisableOutline();
        }
    }

    /// <summary>
    /// Shuffles the deck.
    /// </summary>
    public void ShuffleDeck()
    {
        if (!IsValidCardOperation())
            return;

        for (int i = 0; i < 1000; i++)
        {
            for (int j = 0; j < Cards.Count; j++)
            {
                int randomCardIndex = Random.Range(0, Cards.Count);
                Card currentCard = Cards[j];
                Cards[j] = Cards[randomCardIndex];
                Cards[randomCardIndex] = currentCard;
            }
        }
    }

    /// <summary>
    /// Destroys the entire deck, clear lists.
    /// </summary>
    public void DestroyDeck()
    {
        foreach (Card card in Cards)
        {
            Destroy(card.gameObject);
        }
        Cards.Clear();
        CardsInGame.Clear();
    }

    /// <summary>
    /// Removes a specific card from the deck.
    /// </summary>
    /// <param name="card">The card to remove.</param>
    public void RemoveCardFromDeck(Card card)
    {
        if (!IsValidCardOperation())
            return;

        Cards.Remove(card);
    }

    /// <summary>
    /// Removes all cards with the same value from the deck.
    /// </summary>
    /// <param name="value">The value of the cards to remove.</param>
    public void RemoveCardsBySameValueFromDeck(Card.VALUE value)
    {
        if (!IsValidCardOperation())
            return;

        List<Card> cardsToDestroy = Cards.FindAll(item => item.Value == value);
        Cards.RemoveAll(item => item.Value == value);
        foreach (Card card in cardsToDestroy)
        {
            Destroy(card.gameObject);
        }
    }

    /// <summary>
    /// Adds a card to the deck.
    /// </summary>
    /// <param name="card">The card to add.</param>
    public void AddCardToDeck(Card card)
    {
        Cards.Add(card);
    }

    /// <summary>
    /// Adds a list of cards to the deck.
    /// </summary>
    /// <param name="listCards">The list of cards to add.</param>
    public void AddListOfCardToDeck(List<Card> listCards)
    {
        for (int i = 0; i < listCards.Count; i++)
        {
            Cards.Add(listCards[i]);
        }
    }

    /// <summary>
    /// Adds a card to the list of cards in the game.
    /// </summary>
    /// <param name="card">The card to add.</param>
    public void AddCardToListCardsInGame(Card card)
    {
        CardsInGame.Add(card);
    }

    /// <summary>
    /// Gets the first card from the list of card (deck).
    /// </summary>
    /// <returns>The first card in the deck.</returns>
    public Card GetFirstCardFromDeck()
    {
        if (!IsValidCardOperation())
            return null;

        Card card = Cards.First();
        Cards.Remove(card);
        return card;
    }

    /// <summary>
    /// Gets the last card from the list of card (deck).
    /// </summary>
    /// <returns>The last card in the deck.</returns>
    public Card GetLastCardFromDeck()
    {
        if (!IsValidCardOperation())
            return null;

        Card card = Cards.Last();
        Cards.Remove(card);
        return card;
    }

    /// <summary>
    /// Gets a card by its value and suit from the deck.
    /// </summary>
    /// <param name="value">The value of the card.</param>
    /// <param name="suit">The suit of the card.</param>
    /// <returns>The card matching the specified value and suit.</returns>
    public Card GetCardByValueAndSuit(Card.VALUE value, Card.SUIT suit)
    {
        return Cards.Where(card => card.Value == value && card.Suit == suit).First();
    }

    /// <summary>
    /// Selects cards from the deck based on a list of cards to select.
    /// </summary>
    /// <param name="cardsToSelect">The list of cards to select from the deck.</param>
    /// <returns>The selected cards.</returns>
    public List<Card> SelectCards(List<Card> cardsToSelect)
    {
        return Cards.Where(card => cardsToSelect.Any(c => c.Value == card.Value && c.Suit == card.Suit)).ToList();
    }

    /// <summary>
    /// Checks whether the current deck's operation is valid, such as whether the deck has at least one card.
    /// </summary>
    /// <returns>True if the operation is valid, otherwise false.</returns>
    public bool IsValidCardOperation()
    {
        if (Cards.Count == 0)
        {
            Debug.LogWarning("Invalid operation, the card list cannot be empty.");
            return false;
        }

        return true;
    }

    /// <summary>
    /// Resets the deck to its initial state.
    /// </summary>
    public void ResetDeck()
    {
        foreach (Card card in CardsInGame)
        {
            card.ShowCard = false;
            //card.DisableOutline();
            card.gameObject.SetActive(true);
            Cards.Add(card);
        }
        CardsInGame.Clear();
        OrganizeCardsPositionInDeck();
    }
}
