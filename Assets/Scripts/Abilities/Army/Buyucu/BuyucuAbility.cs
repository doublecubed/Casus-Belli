using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuyucuAbility : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayerStateVariables _selfStates;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfStates = _knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpReturnSupportToDeck);

        base.Initialize();
    }

    private void SetUpReturnSupportToDeck()
    {
        _selfStates.UpdateState(PlayerStateVariable.ReturnSupportsBuyucu, 1);
        base.AbilityCompleted();
    }


}
