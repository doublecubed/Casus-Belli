using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralSetArmyToOne : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;
    private Affiliation _targetFaction;
    private PlayerStateVariables _opponentStates;


    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = _knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpArmyToOne);

        base.Initialize();
    }

    private void SetUpArmyToOne()
    {
        _opponentStates.UpdateState(PlayerStateVariable.SetArmiesToOne, 2, true);
        base.AbilityCompleted();
    }

}
