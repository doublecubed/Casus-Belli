using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PreAbilityPhase : GameStateBase
{
    protected override void OnEnable()
    {
        base.OnEnable();
        _isDone = true;
    }
}
