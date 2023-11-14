using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RahipTakeCardFromTrash : AbilityBase, IButtonClickReceiver
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;
    private Affiliation _opponentFaction;
    private Deck _opponentArmyTrash;
    private Deck _opponentSupportTrash;
    private Deck _selfArmyDeck;
    private Deck _selfSupportDeck;


    private List<Card> _allCardsInTrash;
    private Card _selectedCard;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _opponentFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentArmyTrash = _knowledge.ArmyTrash(_opponentFaction);
        _opponentSupportTrash = _knowledge.SupportTrash(_opponentFaction);
        _selfArmyDeck = _knowledge.ArmyDeck(_selfCard.Faction);

        base._abilityPhase.Add(DisplayTrashCards);
        base._abilityPhase.Add(MoveSelectedCardToDeck);

        base.Initialize();
    }

    private void DisplayTrashCards()
    {
        _allCardsInTrash = new List<Card>();

        List<Card> cardsInArmyTrash = _opponentArmyTrash.LookAtCards(DeckSide.Top, _opponentArmyTrash.NumberOfCardsInDeck());
        _allCardsInTrash.AddRange(cardsInArmyTrash);
        
        List<Card> cardsInSupportTrash = _opponentSupportTrash.LookAtCards(DeckSide.Top, _opponentSupportTrash.NumberOfCardsInDeck());
        _allCardsInTrash.AddRange(cardsInSupportTrash);

        UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_allCardsInTrash, this);

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
