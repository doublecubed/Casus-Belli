using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class AvciArmyToSelf : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private AbilityPlayPhase _playPhase;

    private Affiliation _targetFaction;
    private Card _selectedCard;
    private List<Card> _opponentArmyCards;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _playPhase = _knowledge.AbilityPhase;

        _abilityPhase.Add(SelectOpponentArmyCard);
        _abilityPhase.Add(RemoveCardFromResolveStack);
        _abilityPhase.Add(MoveOpponentArmyCard);

        base.Initialize();
    }



    private void SelectOpponentArmyCard()
    {
        _targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        _opponentArmyCards = _knowledge.PlayArea(_targetFaction).CardsInPlay.Where(x => x.CardType == CardType.Army).ToList();

        if (_opponentArmyCards.Count == 0)
        {
            base.AbilityCompleted();
            return;
        }
        else if (_opponentArmyCards.Count == 1)
        {
            _selectedCard = _opponentArmyCards[0];
            _phaseCompleted = true;
        }
        else
        {
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_opponentArmyCards, this);
        }
    }

    public void ButtonClicked(int index)
    {
        _selectedCard = _opponentArmyCards[index];
        _phaseCompleted = true;
    }

    private void RemoveCardFromResolveStack()
    {
        if (_selectedCard != null)
        {
            _playPhase.RemoveCardFromStack(_selectedCard);
        }

        _phaseCompleted = true;
    }

    private void MoveOpponentArmyCard()
    {
        Deck selfArmyDeck = _knowledge.SupportTrash(_selfCard.Faction);

        _mover.OnCardMovementCompleted += CardMovementCompleted;
        _mover.MoveCard(_selectedCard, selfArmyDeck, selfArmyDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(_targetFaction));

        _selectedCard.Faction = _selfCard.Faction;
    }

    private void CardMovementCompleted(Card card)
    {
        _mover.OnCardMovementCompleted -= CardMovementCompleted;
        _phaseCompleted = true;
        base.AbilityCompleted();
    }
}
