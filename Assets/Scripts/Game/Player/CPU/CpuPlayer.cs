using Nato.PubSub;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static DiscardDeckEvents;
using static PlayerEvents;

public class CpuPlayer : PlayerBase
{
    public enum CPUStrategy
    {
        RANDOM,
        BY_VALUE,
        BY_SEQUENCE,
        VALUE_AND_SEQUENCE
    }

    [SerializeField] private CPUStrategy cpuStrategyType;

    public override void Awake()
    {
        base.Awake();

        IPlayStrategy strategy = new CpuRandomPlayStrategy();
        switch (cpuStrategyType)
        {
            case CPUStrategy.RANDOM:
                strategy = new CpuRandomPlayStrategy();
                break;
            case CPUStrategy.BY_VALUE:
                strategy = new CpuByValuePlayStrategy();
                break;
        }

        SetPlayStrategy(strategy);
    }

    public override void EnableTurn()
    {
        base.EnableTurn();
        if (!IsWon() && IsMyTurn)
        {
            StartCoroutine(WaitCpuChooseMovementCoroutine());
        }
    }

    private IEnumerator WaitCpuChooseMovementCoroutine()
    {
        yield return new WaitForSeconds(seconds: 1f);
        playStrategy.ChooseMovement(this);
    }

    protected override void HandlePlayerGetCardFromDiscardDeck(PlayerGetCardFromDeckEvent playerGetCardFromDeckEvent)
    {
        base.HandlePlayerGetCardFromDiscardDeck(playerGetCardFromDeckEvent);
        if (IsPlayerMe(playerGetCardFromDeckEvent.Player) && !IsWon())
        {
            StartCoroutine(WaitCpuToPlayCoroutine());
        }
    }

    private IEnumerator WaitCpuToPlayCoroutine()
    {
        yield return new WaitForSeconds(seconds: 1f);
        playStrategy.Play(this);
    }

    private bool IsWon()
    {
        if (HandChecker.CheckIfPlayerHasWon(Cards))
        {
            StartCoroutine(WaitToWonCoroutine());
            return true;
        }
        return false;
    }

    private IEnumerator WaitToWonCoroutine()
    {
        yield return new WaitForSeconds(seconds: 1f);
        List<Card> winnerCards = HandChecker.GetWinnerCards(Cards);
        PlayerWonMatchEvent playerWonMatchEvent = new(this, winnerCards);
        EventManager<PlayerWonMatchEvent>.Publish(playerWonMatchEvent);
    }
}
