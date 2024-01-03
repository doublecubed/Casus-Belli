using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NinjaAbility : AbilityBase
{
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(UpdatePower);
        _abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void UpdatePower()
    {
        Debug.Log($"Updating Power for {_selfCard.name}");

        Affiliation targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        List<Card> cardsInPlay = _knowledge.PlayArea(targetFaction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardType == CardType.Army && cardsInPlay[i].Power % 2 == 0)
            {
                if (cardsInPlay[i].gameObject.GetComponentInChildren<BarbarAbility>()) continue;
                _selfCard.SetPower(2);
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
}
