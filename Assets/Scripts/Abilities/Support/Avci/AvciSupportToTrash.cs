using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class AvciSupportToTrash : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private AbilityPlayPhase _playPhase;

    private Affiliation _targetFaction;
    private Card _selectedCard;
    private List<Card> _opponentSupportCards;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _playPhase = _knowledge.AbilityPhase;

        _abilityPhase.Add(SelectOpponentSupportCard);
        _abilityPhase.Add(RemoveCardFromResolveStack);
        _abilityPhase.Add(MoveOpponentSupportCard);

        base.Initialize();
    }


    private void SelectOpponentSupportCard()
    {
        _targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        _opponentSupportCards = _knowledge.PlayArea(_targetFaction).CardsInPlay.Where(x => x.CardType == CardType.Support).ToList();

        if (_opponentSupportCards.Count == 0)
        {
            base.AbilityCompleted();
            return;
        } else if (_opponentSupportCards.Count == 1)
        {
            _selectedCard = _opponentSupportCards[0];
            _phaseCompleted = true;
        } else
        {
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_opponentSupportCards, this);
        }
    }

    public void ButtonClicked(int index)
    {
        _selectedCard = _opponentSupportCards[index];
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

    private void MoveOpponentSupportCard()
    {

        Deck opponentSupportTrash = _knowledge.SupportTrash(_targetFaction);

        _mover.OnCardMovementCompleted += CardMovementCompleted;
        _mover.MoveCard(_selectedCard, opponentSupportTrash, opponentSupportTrash.transform.position, PlacementFacing.Up, DeckSide.Top, _knowledge.LookDirection(_targetFaction));
    }

    private void CardMovementCompleted(Card card)
    {
        _mover.OnCardMovementCompleted -= CardMovementCompleted;
        _phaseCompleted = true;
        base.AbilityCompleted();
    }
}
