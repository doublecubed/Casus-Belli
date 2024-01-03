using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraliceTakeCardInHand : AbilityBase
{
    private Card _selfCard;
    private CardMover _mover;

    private PlayerStateVariables _selfStates;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();

        _mover = knowledge.Mover(_selfCard.Faction);
        _selfStates = knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpPickACardFromHand);

        base.Initialize(knowledge);
    }

    private void SetUpPickACardFromHand()
    {
        _selfStates.UpdateState(PlayerStateVariable.PickACardFromHand, 2);
        base.AbilityCompleted();
    }
}
