using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrensesTakeKingAndPrince : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private PlayArea _opponentPlayArea;
    private PlayerStateVariables _selfStates;
    private AbilityPlayPhase _abilityPlayPhase;

    private Affiliation _opponentFaction;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfStates = _knowledge.PlayerStates(_selfCard.Faction);
        _abilityPlayPhase = _knowledge.AbilityPhase;

        _opponentFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentPlayArea = _knowledge.PlayArea(_opponentFaction);

        base._abilityPhase.Add(CheckForKingAndPrince);

        base.Initialize();
    }

    private void CheckForKingAndPrince()
    {
        List<Card> opponentCardsInPlay = _opponentPlayArea.CardsInPlay;
        
        foreach (Card card in opponentCardsInPlay)
        {
            if (card.CardName == "Prens")
            {
                _selfStates.UpdateState(PlayerStateVariable.TakePrinceInHand, 2);
                _selfStates.TargetPrince = card;
                _abilityPlayPhase.RemoveCardFromStack(card);
            }

            if (card.CardName == "Kral")
            {
                _selfStates.UpdateState(PlayerStateVariable.TakeKingInHand, 2);
                _selfStates.TargetKing = card;
                _abilityPlayPhase.RemoveCardFromStack(card);

            }
        }

        base.AbilityCompleted();
    }

}
