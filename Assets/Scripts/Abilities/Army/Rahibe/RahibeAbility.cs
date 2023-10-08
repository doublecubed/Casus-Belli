using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RahibeAbility : AbilityBase
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private Deck _selfArmyDeck;
    [SerializeField] private PlayArea _selfPlayArea;
    [SerializeField] private PlayerBehaviour _selfBehaviour;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _selfArmyDeck = _knowledge.ArmyDeck(_selfCard.Faction);
        _selfPlayArea = _knowledge.PlayArea(_selfCard.Faction);
        _selfBehaviour = _knowledge.Behaviour(_selfCard.Faction);

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(BringInCard);
        _abilityPhase.Add(LowerCard);

        base.Initialize();
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
