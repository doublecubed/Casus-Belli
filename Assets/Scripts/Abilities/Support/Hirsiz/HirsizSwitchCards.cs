using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class HirsizSwitchCards : AbilityBase, IButtonClickReceiver
{
    [SerializeField] private List<Sprite> _sprites;

    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;

    private bool _topSelected;

    private Affiliation _targetFaction;
    private Deck _opponentArmyDeck;
    private Deck _opponentSupportDeck;

    private int _cardsMoved;
    private int _totalCardsToMove;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentArmyDeck = _knowledge.ArmyDeck(_targetFaction);
        _opponentSupportDeck = _knowledge.SupportDeck(_targetFaction);

        _abilityPhase.Add(SelectTopOrBottom);
        _abilityPhase.Add(SwitchCards);

        base.Initialize();
    }

    private void SelectTopOrBottom()
    {
        UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(_sprites, this);
    }



    private void SwitchCards()
    {
        int armyCardCount = Mathf.Min(5, _opponentArmyDeck.NumberOfCardsInDeck());
        int supportCardCount = Mathf.Min(5, _opponentSupportDeck.NumberOfCardsInDeck());

        _totalCardsToMove = armyCardCount + supportCardCount;
        _cardsMoved = 0;

        DeckSide switchSide = _topSelected ? DeckSide.Top : DeckSide.Bottom;

        List<Card> cardsFromArmy = _opponentArmyDeck.LookAtCards(switchSide, armyCardCount);
        List<Card> cardsFromSupport = _opponentSupportDeck.LookAtCards(switchSide, supportCardCount);

        _mover.OnCardMovementCompleted += CardMovementCompleted;

        for (int i = 0; i < cardsFromArmy.Count; i++)
        {
            _mover.MoveCard(cardsFromArmy[i], _opponentSupportDeck, _opponentSupportDeck.transform.position, PlacementFacing.Down, switchSide, _knowledge.LookDirection(_targetFaction));
        }

        for (int i = 0; i < cardsFromSupport.Count; i++)
        {
            _mover.MoveCard(cardsFromSupport[i], _opponentArmyDeck, _opponentArmyDeck.transform.position, PlacementFacing.Down, switchSide, _knowledge.LookDirection(_targetFaction));
        }
    }

    public void ButtonClicked(int index)
    {
        _topSelected = index == 0;
        _phaseCompleted = true;
    }

    private void CardMovementCompleted(Card card)
    {
        _cardsMoved++;
        if (_cardsMoved >= _totalCardsToMove)
        {
            _mover.OnCardMovementCompleted -= CardMovementCompleted;
            AbilityCompleted();
        }
    }
}
