using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;


public class CameraMoveAction : GameAction
{
    private Transform _camera;
    private Transform _targetTransform;
    private float _moveDuration;
    private CancellationToken _cancellationToken;

    public CameraMoveAction(Transform camera, Transform targetTransform, float moveDuration, CancellationToken cancellationToken)
    {
        _camera = camera;
        _targetTransform = targetTransform;
        _moveDuration = moveDuration;
        _cancellationToken = cancellationToken;
    }

    public override async UniTask ExecuteAction()
    {
        UniTask[] tasks = new UniTask[2];
        tasks[0] = _camera.DOMove(_targetTransform.position, _moveDuration).WithCancellation(_cancellationToken);
        tasks[1] = _camera.DORotateQuaternion(_targetTransform.rotation, _moveDuration).WithCancellation(_cancellationToken);

        await UniTask.WhenAll(tasks);
    }
}
