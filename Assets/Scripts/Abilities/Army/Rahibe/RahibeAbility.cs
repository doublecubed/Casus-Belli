using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class RahibeAbility : AbilityBase
{
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private Deck _selfArmyDeck;
    [SerializeField] private PlayArea _selfPlayArea;
    [SerializeField] private PlayerBehaviour _selfBehaviour;
    private CancellationToken _ct;
    private ActionSequencer _sequencer;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _selfArmyDeck = knowledge.ArmyDeck(_selfCard.Faction);
        _selfPlayArea = knowledge.PlayArea(_selfCard.Faction);
        _selfBehaviour = knowledge.Behaviour(_selfCard.Faction);
        _sequencer = knowledge.Sequencer;

        _abilityPhase.Add(AbilityPhase);

        //_abilityPhase.Add(RiseCard);
        //_abilityPhase.Add(BringInCard);
        //_abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private async void AbilityPhase()
    {
        await CardActions.RiseCard(_selfCard, _ct, _sequencer);


        await CardActions.LowerCard(_selfCard, _ct, _sequencer);
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void BringInCard()
    {
        Card drawnCard = _selfBehaviour.PutFromDeckToPlay(_selfArmyDeck);

        if (drawnCard != null)
        {
            _knowledge.AbilityPhase.AddCardToStack(drawnCard);
        }

        _phaseCompleted = true;
    }

    private void LowerCard()
    {
        Debug.Log($"Lowering Card {_selfCard.name}");
        _mover.LowerInPlace(_selfCard);
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
