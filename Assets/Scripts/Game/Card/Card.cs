using Nato.PubSub;
using UnityEngine;
using UnityEngine.UI;
using static CardEvents;

public class Card : MonoBehaviour
{
    public enum SUIT
    {
        DIAMONDS = 1,
        SPADES,
        HEART,
        CLUBS
    }

    public enum VALUE
    {
        ACE = 1, TWO, THREE, FOUR, FIVE, SIX, SEVEN, EIGHT, NINE, TEN, JACK, QUEEN, KING, NOTHING = 0
    }

    public SUIT Suit;
    public VALUE Value;

    [SerializeField] private Image cardImage;
    private Sprite cardFront;
    private Sprite cardBack;

    public bool ShowCard;
    public bool HideCard;

    [SerializeField] private GameObject outlineCard;

    private bool dragging;
    [SerializeField] private Vector3 offset;

    private Card handCard;

    public bool CanDrag;
    public bool CanClick;
    public bool CanDiscardToDiscardDeck;
    private bool discarded;

    private DiscardDeck discardDeck;


    public void Setup(SUIT suit, VALUE value, Sprite cardBackSprite, Sprite cardFrontSprite)
    {
        Suit = suit;
        Value = value;
        cardBack = cardBackSprite;
        cardFront = cardFrontSprite;
    }

    /// <summary>
    /// Sets the name of the game object based on the card's value and suit.
    /// </summary>
    private void Start()
    {
        gameObject.name = $"{Value}_{Suit}";
    }

    /// <summary>
    /// Updates the card's appearance based on the show state.
    /// </summary>
    private void Update()
    {
        CardShowState(ShowCard);

        if (dragging && CanDrag)
        {
            Quaternion targetRotation = Quaternion.identity;
            transform.localRotation = Quaternion.Lerp(transform.localRotation, targetRotation, 10 * Time.deltaTime);
            Vector3 newPosition = GetMouseWorldPos() + offset;
            newPosition.y = 0.1f;

            transform.position = Vector3.Lerp(transform.position, newPosition, 10 * Time.deltaTime);
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit[] hits = Physics.RaycastAll(ray, Mathf.Infinity);

            foreach (RaycastHit hit in hits)
            {
                if (hit.collider != null)
                {

                    if (hit.collider.TryGetComponent(out Card card))
                    {
                        if (card != null && card != this)
                        {
                            handCard = card;
                            break;
                        }
                        else
                        {
                            handCard = null;
                        }
                    }
                    if (hit.collider.TryGetComponent(out DiscardDeck discardDeck))
                    {
                        this.discardDeck = discardDeck;
                    }
                }
        
            }

            if(hits.Length == 0)
            {
                handCard = null;
                this.discardDeck = null;
            }
        }
    }

    /// <summary>
    /// Sets the card's display state.
    /// </summary>
    /// <param name="state">The state to set (true for showing the card, false for hiding it).</param>
    public void CardShowState(bool state)
    {
        if (HideCard)
        {
            cardImage.sprite = cardBack;
            return;
        }

        if (state)
            cardImage.sprite = cardFront;
        else
            cardImage.sprite = cardBack;
    }

    /// <summary>
    /// Shows an outline around the card with the specified color.
    /// </summary>
    /// <param name="outlineColor">The color of the outline.</param>
    public void ShowOutline(Color outlineColor)
    {
        outlineCard.GetComponent<Image>().color = outlineColor;
        outlineCard.SetActive(true);
    }

    /// <summary>
    /// Disables the outline around the card.
    /// </summary>
    public void DisableOutline()
    {
        outlineCard.SetActive(false);
    }

    private void OnMouseDown()
    {
        if (CanDrag)
            dragging = true;
    }


    private void OnMouseUp()
    {
        dragging = false;
        CardSwapEvent swapCardEvent = new(this, handCard);
        EventManager<CardSwapEvent>.Publish(swapCardEvent);

        if (discardDeck != null && CanDiscardToDiscardDeck)
        {
            DiscardCard();
        }

    }

    public void DiscardCard()
    {
        CardDiscardEvent cardDiscardEvent = new(this);
        EventManager<CardDiscardEvent>.Publish(cardDiscardEvent);
    }

    private Vector3 GetMouseWorldPos()
    {
        Vector3 mousePoint = Input.mousePosition;
        mousePoint.z = Camera.main.WorldToScreenPoint(transform.position).z;
        return Camera.main.ScreenToWorldPoint(mousePoint);
    }
}
