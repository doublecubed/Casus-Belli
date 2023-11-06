using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemirciDoubleArmy : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayArea _selfPlayArea;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfPlayArea = _knowledge.PlayArea(_selfCard.Faction);

        _abilityPhase.Add(DoubleCardPowers);

        base.Initialize();
    }

    private void DoubleCardPowers()
    {
        List<Card> armyCardsInPlay = _selfPlayArea.CardsInPlay.Where(x => x.CardType == CardType.Army).ToList();

        for (int i = 0; i < armyCardsInPlay.Count; i++)
        {
            armyCardsInPlay[i].SetPower(armyCardsInPlay[i].Power * 2);
        }

        AbilityCompleted();
    }


}
