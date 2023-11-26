using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class AbilityBase : MonoBehaviour
{
    public event Action OnAbilityExecutionStarted;
    public event Action OnAbilityExecutionCompleted;

    protected int _numberOfPhases;
    protected int _phaseIndex;
    protected bool _phaseCompleted;

    protected List<Action> _abilityPhase = new List<Action>();

    protected bool _abilityCancelled;

    public virtual void Initialize()
    {
        _phaseIndex = 0;
        _numberOfPhases = _abilityPhase.Count;
    }

    public virtual void UseAbility()
    {
        Debug.Log($"Ability executing for {gameObject.name}");

        OnAbilityExecutionStarted?.Invoke();

        if (_abilityCancelled)
        {
            AbilityCompleted();
            return;
        }

        StartNextPhase();
    }

    public virtual void CancelAbility()
    {
        _abilityCancelled = true;
    }

    protected virtual void StartNextPhase()
    {
        Debug.Log("Phase started in base class");

        if (_phaseIndex < 0 || _phaseIndex >= _numberOfPhases) 
        { 
            return; 
        }

        _phaseCompleted = false;
        _abilityPhase[_phaseIndex]();
    }

    protected virtual void Update()
    {
        if (_phaseCompleted) TransitionToNextPhase();
    }

    protected virtual void TransitionToNextPhase()
    {
        _phaseIndex++;
        if (_phaseIndex >= _numberOfPhases)
        {
            OnAbilityExecutionCompleted?.Invoke();
            return;
        }
        StartNextPhase();
    }

    protected virtual void AbilityCompleted()
    {
        Debug.Log($"{gameObject.name} ability completed");

        OnAbilityExecutionCompleted?.Invoke();
        _abilityCancelled = false;
    }
}
