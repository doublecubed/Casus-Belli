using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DemirciTakeFromTrash : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private AbilityPlayPhase _playPhase;
    [SerializeField] private PlayerBehaviour _selfBehaviour;

    private PlayArea _selfPlayArea;
    private Deck _armyTrash;
    private Deck _supportTrash;

    private List<Card> _allArmyCardsInTrash;
    private Card _selectedCard;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);
        _playPhase = knowledge.AbilityPhase;
        _selfBehaviour = knowledge.Behaviour(_selfCard.Faction);
        _selfPlayArea = knowledge.PlayArea(_selfCard.Faction);

        _armyTrash = knowledge.ArmyTrash(_selfCard.Faction);
        _supportTrash = knowledge.SupportTrash(_selfCard.Faction);

        _abilityPhase.Add(ShowArmyCards);
        _abilityPhase.Add(PutIntoPlay);


        base.Initialize(knowledge);
    }

    private void ShowArmyCards()
    {
        Debug.Log("Demirci show army cards running");

        _allArmyCardsInTrash = new List<Card>();

        List<Card> cardsInArmyTrash = _armyTrash.LookAtCards(DeckSide.Top, _armyTrash.NumberOfCardsInDeck()).Where(x => x.CardType == CardType.Army).ToList();
        _allArmyCardsInTrash.AddRange(cardsInArmyTrash);

        List<Card> cardsInSupportTrash = _supportTrash.LookAtCards(DeckSide.Top, _supportTrash.NumberOfCardsInDeck()).Where(x => x.CardType == CardType.Army).ToList();
        _allArmyCardsInTrash.AddRange(cardsInSupportTrash);

        if (_allArmyCardsInTrash.Count <= 0)
        {
            AbilityCompleted();
            return;
        }

        if (_knowledge.HumanPlayer(_selfBehaviour))
        {
            Debug.Log("Human player, choose your card");
            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_allArmyCardsInTrash, this);
        } else
        {
            Debug.Log("AI Player, card chosen automatically");
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
        Debug.Log("Demirci card selection button clicked");
        _selectedCard = _allArmyCardsInTrash[index];
        _phaseCompleted = true;
    }
}
