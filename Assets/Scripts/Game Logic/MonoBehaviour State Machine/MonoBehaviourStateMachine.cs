using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MonoBehaviourStateMachine : MonoBehaviour
{
    public bool repeating;
    public int repeatStartIndex;

    public GameStateBase[] GameStates;

    [Space(10)]
    public GameStateBase CurrentState;
    public GameStateBase PreviousState;

    

    public void ProceedToNextState()
    {
        int currentStateIndex = Array.IndexOf(GameStates, CurrentState);
        PreviousState = CurrentState;
        PreviousState.gameObject.SetActive(false);

        if (currentStateIndex >= GameStates.Length - 1)
        {
            if (repeating)
            {
                CurrentState = GameStates[repeatStartIndex];
                CurrentState.gameObject.SetActive(true);
            } else
            {
                if (transform.parent.TryGetComponent(out MonoBehaviourStateMachine upperMachine))
                {
                    CurrentState.gameObject.SetActive(false);
                    upperMachine.ProceedToNextState();
                }
            }
        }
        else
        {
            CurrentState = GameStates[currentStateIndex + 1];
            CurrentState.gameObject.SetActive(true);
        }

    }

    public void StartMachine()
    {
        FindStates();

        CurrentState = GameStates[0];
        CurrentState.gameObject.SetActive(true);
    }

    public void StopMachine()
    {
        foreach (GameStateBase state in GameStates) 
        {
            state.gameObject.SetActive(false);
        }
    }

    public void SwitchState(GameStateBase nextState)
    {
        PreviousState = CurrentState;
        PreviousState.gameObject.SetActive(false);

        CurrentState = nextState;
        CurrentState.gameObject.SetActive(true);
    }

    private void FindStates()
    {
        GameStates = new GameStateBase[transform.childCount];
        for (int i = 0; i < GameStates.Length; i++)
        {
            GameStates[i] = transform.GetChild(i).GetComponent<GameStateBase>();
        }
    }

}
