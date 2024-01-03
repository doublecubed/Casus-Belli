using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellatAbility : AbilityBase
{
    private Card _selfCard;
    private Affiliation _opponentFaction;
    private PlayerStateVariables _opponentStates;
    private CardMover _mover;


    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _opponentFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = knowledge.PlayerStates(_opponentFaction);
        _mover = knowledge.Mover(_selfCard.Faction);

        base._abilityPhase.Add(RiseCard);
        base._abilityPhase.Add(SetUpCannotPlaySupport);
        base._abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void SetUpCannotPlaySupport()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CantPlaySupportCards, 2);
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
