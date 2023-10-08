using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class DrawState : GameStateBase
{
    #region REFERENCES

    [Header("Hands")]
    [SerializeField] private Hand _playerHand;
    [SerializeField] private Hand _opponentHand;

    [Header("Stats")]
    [SerializeField] private PlayerStats _playerStats;
    [SerializeField] private PlayerStats _opponentStats;

    [Header("Players")]
    [SerializeField] private AIPlayer _opponentPlayer;
    [SerializeField] private PlayerInput _playerInput;

    [SerializeField] private PlayerBehaviour _playerBehaviour;
    [SerializeField] private CardMover _playerCardMover;

    [SerializeField] private PlayerKnowledge _playerKnowledge;
    [SerializeField] private PlayerKnowledge _opponentKnowledge;

    public bool _hasMovingCard;

    #endregion

    #region MONOBEHAVIOUR

    protected override void OnEnable()
    {
        base.OnEnable();

        Debug.Log("Draw State started");

        _playerInput.OnDeckClicked += DeckClicked;
        _playerCardMover.OnCardMovementStarted += CardMovementStarted;
        _playerCardMover.OnCardMovementCompleted += CardMovementCompleted;

        _opponentPlayer.DrawCards();
    }

    protected override void Update()
    {
        base.Update();

        if (_opponentPlayer.DoneDrawing)
        {
            if (_playerHand.CardsInHand.Count < _playerStats.MaxCardsPerRound && _playerKnowledge.ArmyDeckSelf.CardsInDeck() == 0 && _playerKnowledge.SupportDeckSelf.CardsInDeck() == 0)
            {
                base._isDone = true;
            }

            if (_playerHand.CardsInHand.Count >= _playerStats.MaxCardsPerRound)
            {
                base._isDone = true;
            }
        }
    }

    private void OnDisable()
    {
        _playerCardMover.OnCardMovementStarted -= CardMovementStarted;
        _playerCardMover.OnCardMovementCompleted -= CardMovementCompleted;
        _playerInput.OnDeckClicked -= DeckClicked;
    }

    #endregion

    #region METHODS

    private void DeckClicked(Deck deck)
    {
        if (_hasMovingCard) return;

        if (deck != _playerKnowledge.ArmyDeckSelf && deck != _playerKnowledge.SupportDeckSelf) return;
        
        _playerBehaviour.DrawFromDeckToHand(deck);
        
    }

    private void CardMovementStarted(Card card)
    {
        _hasMovingCard = true;
    }

    private void CardMovementCompleted(Card card)
    {
        _hasMovingCard = false;
    }

    #endregion
}
