using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SaklabanSwitchCards : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;

    private Affiliation _selfFaction;
    private Affiliation _opponentFaction;

    private Deck _opponentSupportDeck;
    private Deck _selfArmyDeck;

    private int _numberOfCardsToMove;
    private int _numberOfCardsMoved;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _selfFaction = _selfCard.Faction;
        _opponentFaction = _knowledge.OpponentFaction(_selfCard.Faction);

        _opponentSupportDeck = _knowledge.SupportDeck(_opponentFaction);
        _selfArmyDeck = _knowledge.ArmyDeck(_selfFaction);

        base._abilityPhase.Add(SwitchCards);

        base.Initialize();
    }

    private void SwitchCards()
    {
        List<Card> _opponentSupportBottomCards = _opponentSupportDeck.LookAtCards(DeckSide.Bottom, 1);
        List<Card> _selfArmyBottomCards = _selfArmyDeck.LookAtCards(DeckSide.Bottom, 1);

        if (_opponentSupportBottomCards.Count <= 0 && _selfArmyBottomCards.Count <= 0)
        {
            base.AbilityCompleted();
            return;
        }

        _mover.OnCardMovementCompleted += CardMovementCompleted;
        _numberOfCardsToMove = _opponentSupportBottomCards.Count + _selfArmyBottomCards.Count;
        _numberOfCardsMoved = 0;

        if (_opponentSupportBottomCards.Count > 0 )
        {
            Card _opponentCard = _opponentSupportBottomCards[0];
            _mover.MoveCard(_opponentCard, _selfArmyDeck, _selfArmyDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfFaction));
            _opponentCard.Faction = _selfFaction;
        }

        if (_selfArmyBottomCards.Count > 0)
        {
            Card _selfCard = _selfArmyBottomCards[0];
            _mover.MoveCard(_selfCard, _opponentSupportDeck, _opponentSupportDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_opponentFaction));
            _selfCard.Faction = _opponentFaction;
        }
        
    }

    private void CardMovementCompleted(Card card)
    {
        _numberOfCardsMoved++;

        if (_numberOfCardsMoved >= _numberOfCardsToMove)
        {
            _mover.OnCardMovementCompleted -= CardMovementCompleted;
            base.AbilityCompleted();
        }
    }

}
