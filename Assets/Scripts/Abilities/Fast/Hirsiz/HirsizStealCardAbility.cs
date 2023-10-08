using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirsizStealCardAbility : AbilityBase
{
    [SerializeField] private PlayerKnowledge _selfKnowledge;
    [SerializeField] private CardMover _cardMover;

    [SerializeField] private Card[] _cardsDisplayed;
    [SerializeField] private Deck _selectedDeck;
    [SerializeField] private Deck _deckToDiscardTo;
    private Card _selectedCard;
    private DeckSide _selectedSide;

    public override void Initialize()
    {
        _selfKnowledge = gameObject.GetComponentInParent<PlayerKnowledge>();
        _cardMover = gameObject.GetComponentInParent<CardMover>();
        _deckToDiscardTo = _selfKnowledge.SupportTrashOpponent;
    }

    public override void UseAbility()
    {
        List<Card> pickedCards = new List<Card>();

        // SELECT THE DECK
        _selectedDeck = _selfKnowledge.ArmyDeckOpponent;
        // ---------------

        pickedCards.AddRange(_selectedDeck.LookAtCards(DeckSide.Top, 1));
        pickedCards.AddRange(_selectedDeck.LookAtCards(DeckSide.Bottom, 1));

        if (pickedCards.Count == 0) return;

        // SELECT THE CARD -- SELECT THE INDEX
        int selectedIndex = 0;
        _selectedSide = selectedIndex == 0 ? DeckSide.Top : DeckSide.Bottom;
        _selectedCard = pickedCards[selectedIndex];

        

        CardType selectedType = _selectedCard.CardType;

        Deck selectedDeck = _selectedCard.CardType == CardType.Army ? _selfKnowledge.ArmyDeckSelf : _selfKnowledge.SupportDeckSelf;
        




    }
}
