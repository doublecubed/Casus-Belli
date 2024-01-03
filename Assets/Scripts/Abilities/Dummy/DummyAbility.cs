using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyAbility : AbilityBase
{
    public override void Initialize(GlobalKnowledge knowledge)
    {

        _abilityPhase.Add(StartAbility);

        base.Initialize(knowledge);
    }

    private void StartAbility()
    {
        UIManager.Instance.GetComponent<CardSelectionDisplayer>().ClearSelectionCards();
        UIManager.Instance.DisplayCardSelectionUI(false);

        AbilityCompleted();
    }

}
