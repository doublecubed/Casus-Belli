using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using DG.Tweening;

public class CancelAbilityAction : GameAction
{
    private Card _card;
    private int _abilityIndex;
    private CancellationToken _cancellationToken;
    private float _duration;
    private float _scale;

    public CancelAbilityAction(Card card, int abilityIndex, float scale, float duration, CancellationToken cancellationToken)
    {
        _card = card;
        _abilityIndex = abilityIndex;
        _duration = duration;
        _scale = scale;
    }

    public override async UniTask ExecuteAction()
    {
        CardDisplayer displayer = _card.GetComponent<CardDisplayer>();
        GameObject cancelIcon = _abilityIndex == 0 ? displayer.FirstAbilityCancelIcon : displayer.SecondAbilityCancelIcon;

        cancelIcon.SetActive(true);

        await cancelIcon.transform.DOScale(_scale, _duration).From().WithCancellation(_cancellationToken);

    }
}
