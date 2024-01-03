using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class OkcuArmyToTrash : AbilityBase
{
    private Card _selfCard;
    private CardMover _mover;

    private Affiliation _targetFaction;
    private Deck _opponentArmyDeck;
    private Deck _opponentArmyTrash;

    private int _numberOfCardsMoved;
    private int _numberOfCardsToMove;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);

        _targetFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentArmyDeck = knowledge.ArmyDeck(_targetFaction);
        _opponentArmyTrash = knowledge.ArmyTrash(_targetFaction);

        base._abilityPhase.Add(MoveArmyCardsToTrash);

        base.Initialize(knowledge);
    }

    private void MoveArmyCardsToTrash()
    {
        int cardsToCheck = Mathf.Min(3, _opponentArmyDeck.NumberOfCardsInDeck());

        List<Card> armyTopCards = _opponentArmyDeck.LookAtCards(DeckSide.Top, cardsToCheck);

        List<Card> selectedCards = armyTopCards.Where(x => x.CardType == CardType.Army && x.Power <= 3).ToList();

        _numberOfCardsToMove = selectedCards.Count;
        _numberOfCardsMoved = 0;

        if (_numberOfCardsToMove <= 0)
        {
            AbilityCompleted();
        }

        _mover.OnCardMovementCompleted += CardMovementCompleted;

        for (int i = 0; i < _numberOfCardsToMove; i++)
        {
            _mover.MoveCard(selectedCards[i], _opponentArmyTrash, _opponentArmyTrash.transform.position, PlacementFacing.Up, _knowledge.LookDirection(_targetFaction));
        }

    }


    private void CardMovementCompleted(Card card)
    {
        _numberOfCardsMoved++;
        if (_numberOfCardsMoved >= _numberOfCardsToMove)
        {
            _mover.OnCardMovementCompleted -= CardMovementCompleted;
            AbilityCompleted();
        }

    }

}
