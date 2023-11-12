using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralicePreventSupportCards : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private Affiliation _targetFaction;
    private PlayerStateVariables _opponentStates;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = _knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpNoSupportCards);

        base.Initialize();
    }

    private void SetUpNoSupportCards()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CantPlaySupportCards, 2, true);
        base.AbilityCompleted();
    }

}
