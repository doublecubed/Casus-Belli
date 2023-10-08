using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrensAbility : AbilityBase
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    [SerializeField] private bool _abilityCancelled;


    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(UpdatePower);
        _abilityPhase.Add(LowerCard);

        base.Initialize();
    }


    public override void CancelAbility()
    {
        _abilityCancelled = true;
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
        List<Card> cardsInPlay = _knowledge.PlayArea(_selfCard.Faction).CardsInPlay;

        if (!_abilityCancelled)
        {
            for (int i = 0; i < cardsInPlay.Count; i++)
            {
                if (cardsInPlay[i].CardType == CardType.Army && cardsInPlay[i] != _selfCard)
                {
                    if (cardsInPlay[i].gameObject.GetComponentInChildren<BarbarAbility>()) continue;
                    cardsInPlay[i].SetPower(cardsInPlay[i].Power + 3);
                }
            }
        }

        _phaseCompleted = true;

        _abilityCancelled = false;
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
