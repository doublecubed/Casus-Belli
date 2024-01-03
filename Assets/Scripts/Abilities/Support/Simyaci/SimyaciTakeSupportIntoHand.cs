using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimyaciTakeSupportIntoHand : AbilityBase
{
    private Card _selfCard;
    private PlayerStateVariables _selfVariables;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _selfVariables = knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpTakeSupportFromTrash);

        base.Initialize(knowledge);
    }

    private void SetUpTakeSupportFromTrash()
    {
        _selfVariables.UpdateState(PlayerStateVariable.TakeSupportFromTrash, 2);
        base.AbilityCompleted();
    }
}
