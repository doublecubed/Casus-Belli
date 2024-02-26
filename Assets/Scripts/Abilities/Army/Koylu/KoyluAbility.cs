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

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(UpdatePower);
        _abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
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
                _selfCard.SetPower(4);
            }
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

