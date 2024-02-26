using Cysharp.Threading.Tasks;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using DG.Tweening;

public class HighlightAction : GameAction
{
    private Card _card;
    private Transform _highlightTransform;
    private float _scaleAmount;
    private float _highlightDuration;
    private CancellationToken _cancellationToken;


    public HighlightAction(Card card, Transform highlightTransform, float scaleAmount, float highlightDuration, CancellationToken cancellationToken)
    {
        _card = card;
        _highlightTransform = highlightTransform;
        _scaleAmount = scaleAmount;
        _highlightDuration = highlightDuration;
        _cancellationToken = cancellationToken;
    }

    public override async UniTask ExecuteAction()
    {
        float originalScale = _highlightTransform.localScale.x;

        await _highlightTransform.DOScale(originalScale * _scaleAmount, _highlightDuration).SetLoops(1, LoopType.Yoyo).WithCancellation(_cancellationToken);

    }
}
