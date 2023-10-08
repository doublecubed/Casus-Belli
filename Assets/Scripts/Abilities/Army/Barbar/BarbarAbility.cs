using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BarbarAbility : AbilityBase
{
    public override void Initialize()
    {
        _abilityPhase.Add(ByPass);

        base.Initialize();
    }

    private void ByPass()
    {
        _phaseCompleted = true;
    }



}
