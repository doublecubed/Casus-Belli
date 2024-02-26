using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;

public class RiseAction : GameAction
{
    private Card _card;
    private float _riseDistance;
    private float _riseDuration;
    private bool _rise;
    private CancellationToken _cancellationToken;

    public event Action<Card> OnCardMovementCompleted;
    public event Action<Card> OnCardMovementStarted;

    public RiseAction(Card card, float distance, float duration, bool rise, CancellationToken cancellationToken)
    {
        _card = card;
        _riseDistance = distance;
        _riseDuration = duration;
        _rise = rise;
        _cancellationToken = cancellationToken;
    }

    public override async UniTask ExecuteAction()
    {
        OnCardMovementStarted?.Invoke(_card);

        await _card.transform.DOMove(Vector3.up * _riseDistance * (_rise ? 1f : -1f), _riseDuration).SetRelative(true).OnComplete(() =>
        {
            OnCardMovementCompleted?.Invoke(_card);
        }).WithCancellation(_cancellationToken);
    }
}
