using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikastciAbility : AbilityBase
{
    #region REFERENCES

    private CardMover _mover;
    private Card _selfCard;
    private ActionSequencer _sequencer;

    #endregion

    #region METHODS

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _sequencer = knowledge.Sequencer;

        _abilityPhase.Add(AbilityPhase);

        base.Initialize(knowledge);
    }

    private async void AbilityPhase()
    {
        CancellationToken ct = this.GetCancellationTokenOnDestroy();

        await CardActions.RiseCard(_selfCard, ct, _sequencer);

        await CheckForKingAndPrince(ct);

        await CardActions.LowerCard(_selfCard, ct, _sequencer);

        AbilityCompleted();
    }

    private async UniTask CheckForKingAndPrince(CancellationToken ct)
    {
        Affiliation targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        List<Card> cardsInPlay = _knowledge.PlayArea(targetFaction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardName == "Prens" || cardsInPlay[i].CardName == "Kral")
            {
                await CardActions.CancelAbilities(cardsInPlay[i], ct, _sequencer);
            }
        }
    }

    #endregion

    #region OLD

    private async UniTask SinglePhase()
    {
        UniTask[] suikastciTasks = new UniTask[2];

        RiseAction riseSuikastci = new RiseAction(_selfCard, 1f, 0.5f, true, this.GetCancellationTokenOnDestroy());
        suikastciTasks[0] = _sequencer.InsertAction(riseSuikastci);

        Affiliation targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        List<Card> cardsInPlay = _knowledge.PlayArea(targetFaction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardName == "Prens" || cardsInPlay[i].CardName == "Kral")
            {
                AbilityBase[] abilities = cardsInPlay[i].GetComponentsInChildren<AbilityBase>();

                foreach (AbilityBase ability in abilities)
                {
                    ability.CancelAbility();
                }
            }
        }

        RiseAction lowerSuikastci = new RiseAction(_selfCard, 1f, 0.5f, true, this.GetCancellationTokenOnDestroy());
        suikastciTasks[1] = _sequencer.InsertAction(lowerSuikastci);

        await UniTask.WhenAll(suikastciTasks);

        AbilityCompleted();

    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void CancelCards()
    {
        Debug.Log($"Cancelling cards for {_selfCard.name}");

        Affiliation targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        List<Card> cardsInPlay = _knowledge.PlayArea(targetFaction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardName == "Prens" || cardsInPlay[i].CardName == "Kral")
            {
                AbilityBase[] abilities = cardsInPlay[i].GetComponentsInChildren<AbilityBase>();

                foreach(AbilityBase ability in abilities)
                {
                    ability.CancelAbility();
                }
            }             
        }

        _phaseCompleted = true;
    }

    private void LowerCard()
    {
        Debug.Log($"Lowering Card {_selfCard.name}");
        _mover.LowerInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void CardMovementDone(Card card)
    {
        _phaseCompleted = true;
        _mover.OnCardMovementCompleted -= CardMovementDone;

        if (_phaseIndex == 2)
        {
            AbilityCompleted();
        }
    }

    #endregion
}
