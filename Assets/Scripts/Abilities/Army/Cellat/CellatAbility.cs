using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellatAbility : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private Affiliation _opponentFaction;
    private PlayerStateVariables _opponentStates;


    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _opponentFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = _knowledge.PlayerStates(_opponentFaction);

        base._abilityPhase.Add(SetUpCannotPlaySupport);

        base.Initialize();
    }


    private void SetUpCannotPlaySupport()
    {
        _opponentStates.UpdateState(PlayerStateVariable.CannotPlaySupportCards, 1);
        base.AbilityCompleted();
    }

}
