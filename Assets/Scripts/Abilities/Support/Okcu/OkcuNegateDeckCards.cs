using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OkcuNegateDeckCards : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;
    private PlayerStateVariables _opponentStates;

    private Affiliation _targetFaction;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = _knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpCardNegation);

        base.Initialize();
    }

    private void SetUpCardNegation()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CannotAffectDeck, 3);
        base.AbilityCompleted();
    }

}
