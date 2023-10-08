using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityStateMachine : MonoBehaviourStateMachine
{
    public static AbilityStateMachine Instance;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
        }
    }


    public void SwitchToState(int index)
    {
        PreviousState = CurrentState;
        PreviousState.gameObject.SetActive(false);

        CurrentState = GameStates[index];
        CurrentState.gameObject.SetActive(true);
    }
}
