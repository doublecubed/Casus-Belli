using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HirsizStealCard : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    private Affiliation _targetFaction;
    private PlayerInput _playerInput;
    private Deck _selectedDeck;
    private Card _selectedCard;
    private PlayerBehaviour _selfBehaviour;

    private List<Card> _targetCards;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _playerInput = _knowledge.PlayerInput;
        _selfBehaviour = _knowledge.Behaviour(_selfCard.Faction);

        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);

        _abilityPhase.Add(GetTargetCards);
        _abilityPhase.Add(SelectCardToSteal);
        _abilityPhase.Add(StealCard);

        base.Initialize();
    }

    private void GetTargetCards()
    {
        if (_knowledge.ArmyDeck(_targetFaction).NumberOfCardsInDeck() <= 0)
        {
            _selectedDeck = _knowledge.SupportDeck(_targetFaction);
            _phaseCompleted = true;
        }
        else if (_knowledge.SupportDeck(_targetFaction).NumberOfCardsInDeck() <= 0)
        {
            _selectedDeck = _knowledge.ArmyDeck(_targetFaction);
            _phaseCompleted = true;
        }
        else
        {
            if (_knowledge.AIPLayer(_selfCard.Faction))
            {
                _selectedDeck = _knowledge.ArmyDeck(_targetFaction);
                _phaseCompleted = true;
            }
            else
            {
                _playerInput.OnDeckClicked += DeckClicked;
            }
        }
    }

    private void SelectCardToSteal()
    {
        if (_targetCards.Count > 1)
        {
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_targetCards, this);
        } else
        {
            _selectedCard = _targetCards[0];
            _phaseCompleted = true;
        }
    }

    private void StealCard()
    {
        _mover.OnCardMovementCompleted += CardMoveCompleted;

        Deck targetDeck = _selectedCard.CardType == CardType.Army ? _knowledge.ArmyDeck(_selfCard.Faction) : _knowledge.SupportDeck(_selfCard.Faction);
        _mover.MoveCard(_selectedCard, targetDeck, targetDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_selfCard.Faction));
        _selectedCard.Faction = _selfCard.Faction;

        Deck targetTrashDeck = _knowledge.SupportTrash(_targetFaction);
        _mover.MoveCard(_selfCard, targetTrashDeck, targetTrashDeck.transform.position, PlacementFacing.Up, DeckSide.Top, _knowledge.LookDirection(_targetFaction));
        _selfCard.Faction = _targetFaction;


    }

    private void DeckClicked(Deck deck)
    {
        if (deck == _knowledge.ArmyDeck(_targetFaction) || deck == _knowledge.SupportDeck(_targetFaction))
        {
            if (deck.NumberOfCardsInDeck() <= 0) return; 

            _selectedDeck = deck;
            _playerInput.OnDeckClicked -= DeckClicked;
            _targetCards = new List<Card>();
            Card topCard = _selectedDeck.LookAtCards(DeckSide.Top, 1)[0];
            if (topCard.CardName == "Hırsız")
            {
                topCard = _selectedDeck.LookAtCards(DeckSide.Top, 1, 1)[0];
            }

            Card bottomCard = _selectedDeck.LookAtCards(DeckSide.Bottom, 1)[0];
            if (bottomCard.CardName == "Hırsız")
            {
                bottomCard = _selectedDeck.LookAtCards(DeckSide.Bottom, 1, 1)[0];
            }

            // TODO: There may be a problem here when there is only one card in deck and that is a Thief

            _targetCards.Add(topCard);
            if (topCard != bottomCard)
            {
                _targetCards.Add(bottomCard);
            }
        }
    }


    public void ButtonClicked(int index)
    {
        _selectedCard = _targetCards[index];
        _phaseCompleted = true;
    }

    public void CardMoveCompleted(Card card)
    {
        _mover.OnCardMovementCompleted -= CardMoveCompleted;
        AbilityCompleted();
    }
}
