using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaklabanReturnSupportToHand : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayerStateVariables _selfStates;


    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfStates = _knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpReturnSupportCardsToHand);

        base.Initialize();
    }

    private void SetUpReturnSupportCardsToHand()
    {
        _selfStates.UpdateState(PlayerStateVariable.ReturnPlayedSupportsToDeck, 2);
        AbilityCompleted();
    }


}
