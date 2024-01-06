using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : GameStateBase
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PlayerKnowledge _playerKnowledge;
    [SerializeField] private PlayerKnowledge _opponentKnowledge;

    public event Action OnTurnEnded;

    protected override void OnEnable()
    {
        base.OnEnable();

        _cameraController.MoveCameraTo(_cameraController.GeneralPosition);

        if (_playerKnowledge.ArmyDeckSelf.NumberOfCardsInDeck() == 0 && _playerKnowledge.SupportDeckSelf.NumberOfCardsInDeck() == 0)
        {
            UIManager.Instance.SetWinningPlayer("Yeşil Kazandı");
            _stateMachine.StopMachine();
        }

        if (_opponentKnowledge.ArmyDeckSelf.NumberOfCardsInDeck() == 0 && _opponentKnowledge.SupportDeckOpponent.NumberOfCardsInDeck() == 0)
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
