using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemirciTakeFromTrash : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private AbilityPlayPhase _playPhase;
    [SerializeField] private PlayerBehaviour _selfBehaviour;

    private PlayArea _selfPlayArea;
    private Deck _armyTrash;
    private Deck _supportTrash;

    private List<Card> _allArmyCardsInTrash;
    private Card _selectedCard;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);
        _playPhase = _knowledge.AbilityPhase;
        _selfBehaviour = _knowledge.Behaviour(_selfCard.Faction);
        _selfPlayArea = _knowledge.PlayArea(_selfCard.Faction);

        _armyTrash = _knowledge.ArmyTrash(_selfCard.Faction);
        _supportTrash = _knowledge.SupportTrash(_selfCard.Faction);

        _abilityPhase.Add(ShowArmyCards);
        _abilityPhase.Add(PutIntoPlay);


        base.Initialize();
    }

    private void ShowArmyCards()
    {
        _allArmyCardsInTrash = new List<Card>();

        List<Card> cardsInArmyTrash = _armyTrash.LookAtCards(DeckSide.Top, _armyTrash.NumberOfCardsInDeck()).Where(x => x.CardType == CardType.Army).ToList();
        _allArmyCardsInTrash.AddRange(cardsInArmyTrash);

        List<Card> cardsInSupportTrash = _supportTrash.LookAtCards(DeckSide.Top, _supportTrash.NumberOfCardsInDeck()).Where(x => x.CardType == CardType.Army).ToList();
        _allArmyCardsInTrash.AddRange(cardsInSupportTrash);

        if (_knowledge.HumanPlayer(_selfBehaviour))
        {
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_allArmyCardsInTrash, this);
        } else
        {
            _selectedCard = _allArmyCardsInTrash[0];
            _phaseCompleted = true;
        }
    }

    private void PutIntoPlay()
    {
        _mover.OnCardMovementCompleted += CardMovementCompleted;
        _mover.MoveCard(_selectedCard, _selfPlayArea, _selfPlayArea.PlacementPosition(), PlacementFacing.Up, _knowledge.LookDirection(_selfCard.Faction));
    }

    private void CardMovementCompleted(Card card)
    {
        _mover.OnCardMovementCompleted -= CardMovementCompleted;
        _playPhase.AddCardToStack(_selectedCard);
        AbilityCompleted();
    }


    public void ButtonClicked(int index)
    {
        _selectedCard = _allArmyCardsInTrash[index];
        _phaseCompleted = true;
    }
}
