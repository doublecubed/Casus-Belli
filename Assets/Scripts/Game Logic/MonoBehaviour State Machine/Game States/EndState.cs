using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndState : GameStateBase
{
    [SerializeField] private CameraController _cameraController;
    [SerializeField] private PlayerKnowledge _playerKnowledge;
    [SerializeField] private PlayerKnowledge _opponentKnowledge;



    protected override void OnEnable()
    {
        base.OnEnable();

        _cameraController.MoveCameraTo(_cameraController.GeneralPosition);

        if (_playerKnowledge.ArmyDeckSelf.CardsInDeck() == 0 && _playerKnowledge.SupportDeckSelf.CardsInDeck() == 0)
        {
            UIManager.Instance.SetWinningPlayer("Yeşil Kazandı");
            _stateMachine.StopMachine();
        }

        if (_opponentKnowledge.ArmyDeckSelf.CardsInDeck() == 0 && _opponentKnowledge.SupportDeckOpponent.CardsInDeck() == 0)
        {
            UIManager.Instance.SetWinningPlayer("Kırmızı Kazandı");
            _stateMachine.StopMachine();
        }

        _isDone = true;

    }
}
