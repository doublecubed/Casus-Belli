using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening; 

public class FlipAction : GameAction
{
    private Camera _camera;
    private Card _card;
    private float _flipDuration;
    private PlacementFacing _facing;
    private CancellationToken _cancellationToken;
    private Vector3 _lookRotation;

    public FlipAction(Camera camera, Card card, float flipDuration, PlacementFacing facing, CancellationToken cancellationToken, Vector3 lookRotation)
    {
        _camera = camera;
        _card = card;
        _flipDuration = flipDuration;
        _facing = facing;
        _cancellationToken = cancellationToken;
        _lookRotation = lookRotation;
    }

    public override async UniTask ExecuteAction()
    {
        Quaternion newRotation = CardCalculations.CardRotation(_facing, _camera, _lookRotation);
        await _card.transform.DORotateQuaternion(newRotation, _flipDuration).WithCancellation(_cancellationToken);
    }
}
