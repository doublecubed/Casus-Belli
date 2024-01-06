using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralSetArmyToOne : AbilityBase
{
    private Card _selfCard;
    private CardMover _mover;
    private Affiliation _targetFaction;
    private PlayerStateVariables _opponentStates;


    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _targetFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentStates = knowledge.PlayerStates(_targetFaction);

        base._abilityPhase.Add(SetUpArmyToOne);

        base.Initialize(knowledge);
    }

    private void SetUpArmyToOne()
    {
        _opponentStates.UpdateState(PlayerStateVariable.SetArmiesToOne, 3, true);
        base.AbilityCompleted();
    }

}
