using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResolveState : GameStateBase
{
    private MonoBehaviourStateMachine _resolveStateMachine;

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private PlayArea _playerArea;
    [SerializeField] private PlayArea _opponentArea;

    [SerializeField] private CardMover _playerMover;
    [SerializeField] private CardMover _opponentMover;

    [SerializeField] private PlayerKnowledge _playerKnowledge;
    [SerializeField] private PlayerKnowledge _opponentKnowledge;

    protected override void Awake()
    {
        base.Awake();
        _resolveStateMachine = GetComponent<MonoBehaviourStateMachine>();
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        FlipCardsInPlay();

        MoveCameraCloser();

        RotateOpponentCardsInPlay();

        _resolveStateMachine.StartMachine();
    }

    private void FlipCardsInPlay()
    {
        for (int i = 0; i < _playerArea.CardsInPlay.Count; i++)
        {
            _playerMover.FlipCardUp(_playerArea.CardsInPlay[i], _playerKnowledge.TableDirection);
        }

        for (int i = 0; i < _opponentArea.CardsInPlay.Count; i++)
        {
            _opponentMover.FlipCardUp(_opponentArea.CardsInPlay[i], _playerKnowledge.TableDirection);
        }
    }

    private void MoveCameraCloser()
    {
        _cameraController.MoveCameraTo(_cameraController.ResolvePosition);
    }

    private void RotateOpponentCardsInPlay()
    {

    }

}
