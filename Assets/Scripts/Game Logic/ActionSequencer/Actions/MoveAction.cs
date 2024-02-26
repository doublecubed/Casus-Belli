using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;
using DG.Tweening;
using System;
using UnityEngine.UIElements.Experimental;
using UnityEngine.UIElements;

public class MoveAction : GameAction
{
    private Camera _camera;
    private Card _card;
    private CardMover _mover;
    private ICardContainer _targetContainer;
    private Vector3 _containerPosition;
    private PlacementFacing _facing;
    private DeckSide _deckSide;
    private Vector3 _lookDirection;
    private CancellationToken _cancellationToken;
    private float _moveDuration;

    public event Action<Card> OnCardMovementCompleted;
    public event Action<Card> OnCardMovementStarted;

    public MoveAction(Card card, CardMover mover, ICardContainer targetContainer, Vector3 position, PlacementFacing facing, DeckSide order, Vector3 lookDirection, CancellationToken cancellationToken, float moveDuration)
    {
        _camera = Camera.main;
        _card = card;
        _mover = mover;
        _targetContainer = targetContainer;
        _containerPosition = position;
        _facing = facing;
        _deckSide = order;
        _lookDirection = lookDirection;
        _cancellationToken = cancellationToken;
        _moveDuration = moveDuration;
    }

    public override async UniTask ExecuteAction()
    {
        OnCardMovementStarted?.Invoke(_card);

        ICardContainer exitingContainer = _card.GetComponentInParent<ICardContainer>();
        exitingContainer.RemoveCard(_card);

        UniTask[] tasks = new UniTask[2]; 

        tasks[0] = _card.transform.DOMove(_containerPosition, _moveDuration).OnComplete(() =>
        {
            _targetContainer.AddCard(_card, _deckSide);
            OnCardMovementCompleted?.Invoke(_card);
        }).WithCancellation(_cancellationToken);
        tasks[1] = _card.transform.DORotateQuaternion(CardCalculations.CardRotation(_facing, _camera, _lookDirection), _moveDuration).WithCancellation(_cancellationToken);

        await UniTask.WhenAll(tasks);
    }

}
