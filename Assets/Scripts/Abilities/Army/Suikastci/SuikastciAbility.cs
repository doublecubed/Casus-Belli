using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SuikastciAbility : AbilityBase
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(CancelCards);
        _abilityPhase.Add(LowerCard);

        base.Initialize();
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
}
