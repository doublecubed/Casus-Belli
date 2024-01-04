using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BuyucuAbility : AbilityBase
{
    private Card _selfCard;
    private PlayArea _selfPlayArea;
    private CardMover _mover;
    private PlayerStateVariables _selfStates;
    private EndState _endState;

    private List<Card> _selfSupportCards;
    private Deck _selfSupportDeck;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _selfPlayArea = knowledge.PlayArea(_selfCard.Faction);
        _mover = knowledge.Mover(_selfCard.Faction);
        _selfStates = knowledge.PlayerStates(_selfCard.Faction);
        _endState = knowledge.EndState;
        _selfSupportDeck = knowledge.SupportDeck(_selfCard.Faction);

        base._abilityPhase.Add(RiseCard);
        base._abilityPhase.Add(GetCardsToReturnToHand);
        base._abilityPhase.Add(LowerCard);

        base.Initialize(knowledge);
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void GetCardsToReturnToHand()
    {
        _selfSupportCards = _selfPlayArea.CardsInPlay.Where(x => x.CardType == CardType.Support).ToList();
        _endState.OnTurnEnded += AbilityTriggered;
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


    private void AbilityTriggered()
    {
        for (int i = 0; i <  _selfSupportCards.Count; i++)
        {
            _mover.MoveCard(_selfSupportCards[i], _selfSupportDeck, _selfSupportDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfCard.Faction));
        }
    }
}
