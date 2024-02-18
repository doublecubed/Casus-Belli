using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : GameStateBase
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private GlobalKnowledge _globalKnowledge;



    public event Action OnTurnEnded;

    protected override void OnEnable()
    {
        base.OnEnable();

        _cameraController.MoveCameraTo(_cameraController.GeneralPosition);

        if (_globalKnowledge.SupportDeck(_globalKnowledge.HumanFaction()).NumberOfCardsInDeck() <= 0 && _globalKnowledge.ArmyDeck(_globalKnowledge.HumanFaction()).NumberOfCardsInDeck() <= 0)
        {
            UIManager.Instance.SetWinningPlayer("Yeşil Kazandı");
            _stateMachine.StopMachine();
        }

        if (_globalKnowledge.SupportDeck(_globalKnowledge.ComputerFaction()).NumberOfCardsInDeck() <= 0 && _globalKnowledge.ArmyDeck(_globalKnowledge.ComputerFaction()).NumberOfCardsInDeck() <= 0)
        {
            UIManager.Instance.SetWinningPlayer("Kırmızı Kazandı");
            _stateMachine.StopMachine();
        }


        Invoke("EndTurn", 1f);

    }

    private void EndTurn()
    {
        OnTurnEnded?.Invoke();
        _isDone = true;
    }
}
