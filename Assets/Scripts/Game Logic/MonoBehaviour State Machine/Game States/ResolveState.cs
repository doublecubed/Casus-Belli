using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Threading;

public class ResolveState : GameStateBase
{
    private MonoBehaviourStateMachine _resolveStateMachine;

    [SerializeField] private CameraController _cameraController;

    [SerializeField] private PlayArea _playerArea;
    [SerializeField] private PlayArea _opponentArea;

    [SerializeField] private CardMover _playerMover;
    [SerializeField] private CardMover _opponentMover;

    [SerializeField] private GlobalKnowledge _globalKnowledge;

    private CancellationToken _cancellationToken;

    private const float _flipDuration = 0.5f;

    protected override void Awake()
    {
        base.Awake();
        _resolveStateMachine = GetComponent<MonoBehaviourStateMachine>();
        _cancellationToken = this.GetCancellationTokenOnDestroy();
    }

    protected async override void OnEnable()
    {
        base.OnEnable();

        await MoveCameraCloser();

        await FlipCardsInPlay();

        RotateOpponentCardsInPlay();

        _resolveStateMachine.StartMachine();
    }

    private async UniTask FlipCardsInPlay()
    {
        List<UniTask> tasks = new List<UniTask>();

        for (int i = 0; i < _playerArea.CardsInPlay.Count; i++)
        {
            FlipAction flip = new FlipAction(Camera.main, _playerArea.CardsInPlay[i], _flipDuration, PlacementFacing.Up, _cancellationToken, _knowledge.LookDirection(_knowledge.HumanFaction()));
            tasks.Add(flip.ExecuteAction());
            //_playerMover.FlipCardUp(_playerArea.CardsInPlay[i], _globalKnowledge.LookDirection(_globalKnowledge.HumanFaction()));
        }

        for (int i = 0; i < _opponentArea.CardsInPlay.Count; i++)
        {
            FlipAction flip = new FlipAction(Camera.main, _opponentArea.CardsInPlay[i], _flipDuration, PlacementFacing.Up, _cancellationToken, _knowledge.LookDirection(_knowledge.ComputerFaction()));
            tasks.Add(flip.ExecuteAction());
            //_opponentMover.FlipCardUp(_opponentArea.CardsInPlay[i], _globalKnowledge.LookDirection(_globalKnowledge.ComputerFaction()));
        }

        await UniTask.WhenAll(tasks);
    }

 

    private async UniTask MoveCameraCloser()
    {
        await _cameraController.MoveCameraTo(_cameraController.ResolvePosition);
    }

    private void RotateOpponentCardsInPlay()
    {

    }

}
