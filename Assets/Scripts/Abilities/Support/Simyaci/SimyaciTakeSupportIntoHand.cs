using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimyaciTakeSupportIntoHand : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayerStateVariables _selfVariables;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfVariables = _knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpTakeSupportFromTrash);

        base.Initialize();
    }

    private void SetUpTakeSupportFromTrash()
    {
        _selfVariables.UpdateState(PlayerStateVariable.TakeSupportFromTrash, 1);
        base.AbilityCompleted();
    }
}
