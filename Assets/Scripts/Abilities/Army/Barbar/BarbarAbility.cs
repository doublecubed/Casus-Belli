using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarAbility : AbilityBase
{
    public override void Initialize(GlobalKnowledge knowledge)
    {
        _abilityPhase.Add(ByPass);

        base.Initialize(knowledge);
    }

    private void ByPass()
    {
        _phaseCompleted = true;
    }



}
