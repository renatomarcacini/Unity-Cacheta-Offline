using UnityEngine;

[CreateAssetMenu(fileName = "NewDeck", menuName = "Cacheta Data/Deck Base Data", order = 1)]
public class DeckData : ScriptableObject
{
    public string DeckName = "Default";
    public Sprite Sprite_CardBack;
    [Header("ORDER: Diamonds, Spades, Hearts, Clubs - 2 to A")]
    public Sprite[] Sprites_CardFront;
}
