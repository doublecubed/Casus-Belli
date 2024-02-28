using System.Collections;
using System.Collections.Generic;
using System.Threading;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

public class KesisAbility : AbilityBase
{
    private Card _selfCard;
    private ActionSequencer _sequencer;
    private CancellationToken _ct;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _ct = this.GetCancellationTokenOnDestroy();

        _abilityPhase.Add(AbilityPhase);

        base.Initialize(knowledge);
    }

    private async void AbilityPhase()
    {
        await CardActions.RiseCard(_selfCard, _ct, _sequencer);

        await CheckForOtherCards();

        await CardActions.LowerCard(_selfCard, _ct, _sequencer);
    }

    private async UniTask CheckForOtherCards()
    {
        List<Card> cardsInPlay = _knowledge.PlayArea(_selfCard.Faction).CardsInPlay;

        if (cardsInPlay.Count == 1 && cardsInPlay[0] == _selfCard)
        {
            await CardActions.ChangePower(_selfCard, 6, _ct, _sequencer);
        }
    }

    #region OLD

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        //_mover.RiseInPlace(_selfCard);
        //_mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void UpdatePower()
    {
        Debug.Log($"Updating Power for {_selfCard.name}");
        List<Card> cardsInPlay = _knowledge.PlayArea(_selfCard.Faction).CardsInPlay;

        if (cardsInPlay.Count == 1 && cardsInPlay[0] == _selfCard)
        {
            _selfCard.SetPower(6);
        }

        _phaseCompleted = true;
    }

    private void LowerCard()
    {
        Debug.Log($"Lowering Card {_selfCard.name}");
        //_mover.LowerInPlace(_selfCard);
        //_mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void CardMovementDone(Card card)
    {
        _phaseCompleted = true;
        //_mover.OnCardMovementCompleted -= CardMovementDone;

        if (_phaseIndex == 2)
        {
            AbilityCompleted();
        }
    }

    #endregion
}
