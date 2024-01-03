using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkcuNegateDeckCards : AbilityBase
{
    private Card _selfCard;
    private CardMover _mover;
    private PlayerStateVariables _opponentStates;

    private Affiliation _targetFaction;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);

        _targetFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpCardNegation);

        base.Initialize(knowledge);
    }

    private void SetUpCardNegation()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CannotAffectDeck, 3);
        base.AbilityCompleted();
    }

}
