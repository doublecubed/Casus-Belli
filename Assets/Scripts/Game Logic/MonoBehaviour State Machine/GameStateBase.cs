using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GameStateBase : MonoBehaviour
{
    [SerializeField] protected MonoBehaviourStateMachine _stateMachine;

    [SerializeField] protected bool _isDone;

    protected virtual void Awake()
    {
        _stateMachine = GetComponentInParent<MonoBehaviourStateMachine>();
    }

    protected virtual void OnEnable()
    {
        _isDone = false;
    }

    protected virtual void Update()
    {
        if (_isDone) _stateMachine.ProceedToNextState();
    }
}
