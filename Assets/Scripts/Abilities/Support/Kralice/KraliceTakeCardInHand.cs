using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraliceTakeCardInHand : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;

    private PlayerStateVariables _selfStates;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _selfStates = _knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpPickACardFromHand);


        base.Initialize();
    }

    private void SetUpPickACardFromHand()
    {
        _selfStates.UpdateState(PlayerStateVariable.PickACardFromHand, 2);
    }
}
