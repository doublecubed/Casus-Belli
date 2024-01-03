using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralicePreventSupportCards : AbilityBase
{
    private Card _selfCard;
    private Affiliation _targetFaction;
    private PlayerStateVariables _opponentStates;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _targetFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpNoSupportCards);

        base.Initialize(knowledge);
    }

    private void SetUpNoSupportCards()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CantPlaySupportCards, 2, true);
        base.AbilityCompleted();
    }

}
