using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KoyluAbility : AbilityBase
{
    private ActionSequencer _sequencer;

    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _sequencer = knowledge.Sequencer;

        _abilityPhase.Add(SinglePhase);

        //_abilityPhase.Add(RiseCard);
        //_abilityPhase.Add(UpdatePower);
        //_abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private void SinglePhase()
    {
        UniTask[] koyluTasks = new UniTask[3];

        RiseAction riseKoylu = new RiseAction(_selfCard, 1f, 0.5f, true, this.GetCancellationTokenOnDestroy());
        koyluTasks[0] = _sequencer.InsertAction(riseKoylu);

        List<Card> cardsInPlay = _knowledge.PlayArea(_selfCard.Faction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardType == CardType.Army && cardsInPlay[i] != _selfCard)
            {
                ChangePowerAction changePower = new ChangePowerAction(_selfCard, 4);
                koyluTasks[1] = _sequencer.InsertAction(changePower);
            }
        }

        RiseAction lowerKoylu = new RiseAction(_selfCard, 1f, 0.5f, false, this.GetCancellationTokenOnDestroy());
        koyluTasks[2] = _sequencer.InsertAction(lowerKoylu);

        UniTask.WhenAll(koyluTasks);

        AbilityCompleted();

    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        RiseAction riseKoylu = new RiseAction(_selfCard, 1f, 0.5f, true, this.GetCancellationTokenOnDestroy());
        _sequencer.InsertAction( riseKoylu );
        //_mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void UpdatePower()
    {
        Debug.Log($"Updating Power for {_selfCard.name}");
        List<Card> cardsInPlay = _knowledge.PlayArea(_selfCard.Faction).CardsInPlay;

        for (int  i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardType == CardType.Army && cardsInPlay[i] != _selfCard)
            {
                ChangePowerAction changePower = new ChangePowerAction(_selfCard, 4);
                _sequencer.InsertAction(changePower );
                //_selfCard.SetPower(4);
            }
        }

        _phaseCompleted = true;
    }

    private void LowerCard()
    {
        Debug.Log($"Lowering Card {_selfCard.name}");
        RiseAction lowerKoylu = new RiseAction(_selfCard, 1f, 0.5f, false, this.GetCancellationTokenOnDestroy());
        _sequencer.InsertAction(lowerKoylu);
        //_mover.LowerInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void CardMovementDone(Card card)
    {
        _phaseCompleted = true;
        _mover.OnCardMovementCompleted -= CardMovementDone;

        if (_phaseIndex == 2)
        {
            AbilityCompleted();
        }
    }


}

