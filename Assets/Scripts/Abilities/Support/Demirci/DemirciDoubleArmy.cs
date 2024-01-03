using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemirciDoubleArmy : AbilityBase
{
    private Card _selfCard;
    private PlayArea _selfPlayArea;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _selfPlayArea = knowledge.PlayArea(_selfCard.Faction);

        _abilityPhase.Add(DoubleCardPowers);

        base.Initialize(knowledge);
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
