using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrensesTakeKingAndPrince : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayArea _opponentPlayArea;

    private Affiliation _opponentFaction;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;

        _opponentFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentPlayArea = _knowledge.PlayArea(_opponentFaction);

        base._abilityPhase.Add(CheckForKingAndPrince);

        base.Initialize();
    }

    private void CheckForKingAndPrince()
    {
        List<Card> opponentCardsInPlay = _opponentPlayArea.CardsInPlay;
        // TODO: Add the ability here

        base.AbilityCompleted();
    }

}
