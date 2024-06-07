using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewMatch", menuName = "Cacheta Data/Match Base Data", order = 1)]
public class MatchData : ScriptableObject
{
    [Range(2, 4)]
    public int AmountOfPlayers = 2;
}
