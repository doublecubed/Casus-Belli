using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAbility : AbilityBase
{
    public override void Initialize()
    {

        _abilityPhase.Add(StartAbility);

        base.Initialize();
    }

    private void StartAbility()
    {
        UIManager.Instance.GetComponent<CardSelectionDisplayer>().ClearSelectionCards();
        UIManager.Instance.DisplayCardSelectionUI(false);

        AbilityCompleted();
    }

}
