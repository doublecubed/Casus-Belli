using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RahipTakeCardFromTrash : AbilityBase, IButtonClickReceiver
{
    private Card _selfCard;
    private CardMover _mover;
    private Affiliation _opponentFaction;
    private Deck _opponentArmyTrash;
    private Deck _opponentSupportTrash;
    private Deck _selfArmyDeck;
    private Deck _selfSupportDeck;
    private PlayerBehaviour _selfBehaviour;


    private List<Card> _allCardsInTrash;
    private Card _selectedCard;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _opponentFaction = knowledge.OpponentFaction(_selfCard.Faction);
        _opponentArmyTrash = knowledge.ArmyTrash(_opponentFaction);
        _opponentSupportTrash = knowledge.SupportTrash(_opponentFaction);
        _selfArmyDeck = knowledge.ArmyDeck(_selfCard.Faction);
        _selfBehaviour = knowledge.Behaviour(_selfCard.Faction);

        base._abilityPhase.Add(DisplayTrashCards);
        base._abilityPhase.Add(MoveSelectedCardToDeck);

        base.Initialize(knowledge);
    }

    private void DisplayTrashCards()
    {
        _allCardsInTrash = new List<Card>();

        List<Card> cardsInArmyTrash = _opponentArmyTrash.LookAtCards(DeckSide.Top, _opponentArmyTrash.NumberOfCardsInDeck());
        _allCardsInTrash.AddRange(cardsInArmyTrash);
        
        List<Card> cardsInSupportTrash = _opponentSupportTrash.LookAtCards(DeckSide.Top, _opponentSupportTrash.NumberOfCardsInDeck());
        _allCardsInTrash.AddRange(cardsInSupportTrash);

        if (_allCardsInTrash.Count <= 0)
        {
            base.AbilityCompleted();
            return;
        }

        if (_selfBehaviour.TryGetComponent(out AIPlayer aiPlayer))
        {
            ButtonClicked(0);
        }
        else
        {
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_allCardsInTrash, this);
        }

    }

    private void MoveSelectedCardToDeck()
    {
        CardType selectedCardType = _selectedCard.CardType;

        Deck targetDeck = selectedCardType == CardType.Army ? _selfArmyDeck : _selfSupportDeck;

        _mover.OnCardMovementCompleted += CardMovementCompleted;
        _mover.MoveCard(_selectedCard, targetDeck, targetDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfCard.Faction));
    }

    public void ButtonClicked(int index)
    {
        _selectedCard = _allCardsInTrash[index];
        _phaseCompleted = true;
    }

    private void CardMovementCompleted(Card card)
    {
        _mover.OnCardMovementCompleted -= CardMovementCompleted;
        AbilityCompleted();
    }
}
