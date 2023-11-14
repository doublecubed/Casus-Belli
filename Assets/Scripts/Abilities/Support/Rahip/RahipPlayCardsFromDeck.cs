using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RahipPlayCardsFromDeck : AbilityBase, IButtonClickReceiver
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;
    private AbilityPlayPhase _playPhase;

    private PlayArea _selfPlayArea;

    private Deck _selfArmyDeck;
    private Deck _selfSupportDeck;

    private List<Card> _armyCards;
    private List<Card> _supportCards;

    private Card _selectedArmyCard;
    private Card _selectedSupportCard;

    List<Card> _currentCards;
    private Card _currentSelectedCard;

    private int _numberOfCardsToMove;
    private int _numberOfCardsMoved;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _playPhase = _knowledge.AbilityPhase;

        _selfArmyDeck = _knowledge.ArmyDeck(_selfCard.Faction);
        _selfSupportDeck = _knowledge.SupportDeck(_selfCard.Faction);
        _selfPlayArea = _knowledge.PlayArea(_selfCard.Faction);
        

        base._abilityPhase.Add(SelectArmyCard);
        base._abilityPhase.Add(PlayArmyCard);
        base._abilityPhase.Add(SelectSupportCard);
        base._abilityPhase.Add(PlaySupportCard);
        base._abilityPhase.Add(EndAbility);

        base.Initialize();
    }

    private void SelectArmyCard()
    {
        _armyCards = _selfArmyDeck.LookAtCards(DeckSide.Top, 2);
        _currentCards = _armyCards;

        UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_currentCards, this);
    }

    private void PlayArmyCard() 
    {
        _selectedArmyCard = _currentSelectedCard;
        
        _numberOfCardsToMove = _armyCards.Count;
        _numberOfCardsMoved = 0;
        _mover.OnCardMovementCompleted += CardMovementCompleted;

        for (int i=0; i <_armyCards.Count; i++)
        {
            if (_armyCards[i] == _selectedArmyCard)
            {
                _mover.MoveCard(_armyCards[i], _selfPlayArea, _selfPlayArea.PlacementPosition(), PlacementFacing.Up, _knowledge.LookDirection(_selfCard.Faction));
                _playPhase.AddCardToStack(_selectedArmyCard);             

            } else
            {
                Deck targetDeck = _armyCards[i].CardType == CardType.Army ? _selfArmyDeck : _selfSupportDeck;
                _mover.MoveCard(_armyCards[i], targetDeck, targetDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfCard.Faction));
            }
        }
    }

    private void SelectSupportCard()
    {
        _supportCards = _selfSupportDeck.LookAtCards(DeckSide.Top, 2);
        _currentCards = _supportCards;

        UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_currentCards, this);
    }

    private void PlaySupportCard()
    {
        _selectedSupportCard = _currentSelectedCard;

        _numberOfCardsToMove = _supportCards.Count;
        _numberOfCardsMoved = 0;
        _mover.OnCardMovementCompleted += CardMovementCompleted;

        for (int i = 0; i < _supportCards.Count; i++)
        {
            if (_supportCards[i] == _selectedArmyCard)
            {
                _mover.MoveCard(_supportCards[i], _selfPlayArea, _selfPlayArea.PlacementPosition(), PlacementFacing.Up, _knowledge.LookDirection(_selfCard.Faction));
                _playPhase.AddCardToStack(_selectedArmyCard);

            }
            else
            {
                Deck targetDeck = _supportCards[i].CardType == CardType.Army ? _selfArmyDeck : _selfSupportDeck;
                _mover.MoveCard(_supportCards[i], targetDeck, targetDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfCard.Faction));
            }
        }
    }

    private void CardMovementCompleted(Card card)
    {
        _numberOfCardsMoved++;

        if (_numberOfCardsMoved >= _numberOfCardsToMove)
        {
            _mover.OnCardMovementCompleted -= CardMovementCompleted;
            _phaseCompleted = true;
        }
    }

    private void EndAbility()
    {
        base.AbilityCompleted();
    }

    public void ButtonClicked(int index)
    {
        _currentSelectedCard = _currentCards[index];
        _phaseCompleted = true;
    }
}
