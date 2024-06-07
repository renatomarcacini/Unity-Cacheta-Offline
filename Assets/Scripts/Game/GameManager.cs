using Nato.PubSub;
using Nato.Singleton;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameManagerEvents;
using static PlayerEvents;

public class GameManager : Singleton<GameManager>
{
    [SerializeField] private MatchData matchData;
    [SerializeField] private PlayerBase humanPlayerPrefab;
    [SerializeField] private PlayerBase cpuPlayerPrefab;
    [SerializeField] private Transform[] playerPositions;
    [field: SerializeField] public PurchaseDeck PurchaseDeck { get; private set; }
    [field: SerializeField] public DiscardDeck DiscardDeck { get; private set; }
    public List<PlayerBase> PlayersInGame { get; private set; }

    [SerializeField] private PlayerBase currentPlayerTurn;
    [SerializeField] private int playerTurn;

    [field: SerializeField] public HandVisual WinnerHand { get; private set; }

    private IEnumerator Start()
    {
        PurchaseDeck.MakeDeck(quantity: 2);
        PurchaseDeck.ShuffleDeck();
        yield return null;
        yield return InstantiatePlayersCoroutine(allCpus: false);
        yield return DistributeCardToPlayersCoroutine();
        yield return DistributeFirstCardToDiscardDeckCoroutine();
        yield return StartMatchCoroutine();
    }

    private IEnumerator InstantiatePlayersCoroutine(bool allCpus = false)
    {
        PlayersInGame = new List<PlayerBase>();
        for (int i = 0; i < matchData.AmountOfPlayers; i++)
        {
            PlayerBase player = null;
            if (!allCpus)
            {
                if (i == 0)
                    player = Instantiate(humanPlayerPrefab, playerPositions[i]);
                else
                    player = Instantiate(cpuPlayerPrefab, playerPositions[i]);
            }
            else
            {
                player = Instantiate(cpuPlayerPrefab, playerPositions[i]);
            }

            player.SetScore(value: 5);
            PlayersInGame.Add(player);
            yield return new WaitForSeconds(seconds: 0.1f);
        }
    }

    private IEnumerator DistributeCardToPlayersCoroutine()
    {
        float cardMoveDuration = 0.3f;
        for (int j = 0; j < 9; j++)
        {
            for (int i = 0; i < PlayersInGame.Count; i++)
            {
                Card card = PurchaseDeck.GetFirstCardFromDeck();
                PurchaseDeck.CardsInGame.Add(card);

                PlayerBase currentPlayer = PlayersInGame[i];
                CardDistributedEvent cardDistributedEvent = new(card, currentPlayer, cardMoveDuration);
                EventManager<CardDistributedEvent>.Publish(cardDistributedEvent);
                yield return new WaitForSeconds(seconds: 0.03f);
            }
        }
    }

    private IEnumerator DistributeFirstCardToDiscardDeckCoroutine()
    {
        Card card = PurchaseDeck.GetFirstCardFromDeck();
        PurchaseDeck.CardsInGame.Add(card);
        DiscardDeck.AddCardToDiscardDeck(card);
        yield return new WaitForSeconds(seconds: 0.3f);
    }

    private IEnumerator StartMatchCoroutine()
    {
        yield return new WaitForSeconds(seconds: 0.5f);
        playerTurn = 0;
        SetPlayerTurn(PlayersInGame[playerTurn]);
    }

    private void SetPlayerTurn(PlayerBase player)
    {
        PlayerTurnChangedEvent playerTurnChangedEvent = new(player);
        EventManager<PlayerTurnChangedEvent>.Publish(playerTurnChangedEvent);
        currentPlayerTurn = player;
    }

    public void NextTurn()
    {
        StartCoroutine(WaitNextTurnCoroutine());
    }

    private IEnumerator WaitNextTurnCoroutine()
    {
        yield return new WaitForSeconds(1);
        playerTurn = (playerTurn + 1) % PlayersInGame.Count;
        PlayerBase currentPlayerTurn = PlayersInGame[playerTurn];
        SetPlayerTurn(currentPlayerTurn);
    }

    private void OnEnable()
    {
        EventManager<PlayerWonMatchEvent>.Subscribe(HandlePlayerWonMatch);
    }

    private void OnDisable()
    {
        EventManager<PlayerWonMatchEvent>.Subscribe(HandlePlayerWonMatch);
    }

    private void HandlePlayerWonMatch(PlayerWonMatchEvent playerWonMatchEvent)
    {
        PlayerBase winnerPlayer = playerWonMatchEvent.Player;
        List<Card> winnerCards = playerWonMatchEvent.Cards;
        int quantityCards = winnerCards.Count;

        StartCoroutine(PlayerWonCoroutine(winnerPlayer, quantityCards));
    }

    private IEnumerator PlayerWonCoroutine(PlayerBase winnerPlayer, int quantityCards)
    {
        yield return new WaitForSeconds(seconds: 0.3f);
        ClearPlayerCards();
        AdjustPlayerScores(winnerPlayer, quantityCards);
        yield return new WaitForSeconds(seconds: 3f);
        RemovePlayersWithNoScore();
        CheckForMatchEnd(winnerPlayer);
        ResetMatchIfNecessary();
    }

    private void AdjustPlayerScores(PlayerBase winnerPlayer, int quantityCards)
    {
        foreach (var player in PlayersInGame)
        {
            if (!player.IsPlayerMe(winnerPlayer))
            {
                if (quantityCards == 10)
                    player.SubScore(value: 2);
                else
                    player.SubScore(value: 1);
            }
        }
    }

    private void ClearPlayerCards()
    {
        foreach (var player in PlayersInGame)
        {
            player.Cards.Clear();
        }
    }

    private void RemovePlayersWithNoScore()
    {
        for (int i = PlayersInGame.Count - 1; i >= 0; i--)
        {
            if (PlayersInGame[i].Score <= 0)
            {
                var player = PlayersInGame[i];
                PlayersInGame.Remove(player);
                player.gameObject.SetActive(false);
            }
        }
    }

    private void CheckForMatchEnd(PlayerBase winnerPlayer)
    {
        if (PlayersInGame.Count == 1)
        {
            UIMatchStateManager.Instance.StateMachine.TransitionTo<MatchEndState>();
        }
    }

    private void ResetMatchIfNecessary()
    {
        if (PlayersInGame.Count > 1)
        {
            StartCoroutine(WaitToResetMatch());
        }
    }

    private IEnumerator WaitToResetMatch()
    {
        yield return new WaitForSeconds(seconds: 0.3f);
        DiscardDeck.Cards.Clear();
        PurchaseDeck.AddListOfCardToDeck(PurchaseDeck.CardsInGame);
        PurchaseDeck.CardsInGame.Clear();
        PurchaseDeck.OrganizeCardsPositionInDeck();
        yield return DistributeCardToPlayersCoroutine();
        yield return DistributeFirstCardToDiscardDeckCoroutine();
        yield return StartMatchCoroutine();
    }
}
