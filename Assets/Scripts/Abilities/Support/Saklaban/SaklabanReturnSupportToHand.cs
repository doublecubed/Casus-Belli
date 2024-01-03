using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaklabanReturnSupportToHand : AbilityBase
{
    private Card _selfCard;
    private PlayerStateVariables _selfStates;


    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _selfStates = knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpReturnSupportCardsToHand);

        base.Initialize(knowledge);
    }

    private void SetUpReturnSupportCardsToHand()
    {
        _selfStates.UpdateState(PlayerStateVariable.ReturnPlayedSupportsToDeck, 2);
        AbilityCompleted();
    }


}
