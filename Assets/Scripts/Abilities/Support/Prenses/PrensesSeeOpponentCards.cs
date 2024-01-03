using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrensesSeeOpponentCards : AbilityBase
{
    private Card _selfCard;
    private PlayerStateVariables _opponentStates;

    private Affiliation _targetFaction;


    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _targetFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpSeeOpponentCards);

        base.Initialize(knowledge);
    }


    private void SetUpSeeOpponentCards()
    {
        _opponentStates.UpdateState(PlayerStateVariable.PlayHandOpen, 2, true);
        base.AbilityCompleted();
    }

}
