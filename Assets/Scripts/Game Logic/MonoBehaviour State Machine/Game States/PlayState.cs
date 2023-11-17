using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayState : GameStateBase
{
    #region REFERENCES

    [SerializeField] private HumanPlayer _humanPlayer;
    [SerializeField] private AIPlayer _aiPlayer;

    [SerializeField] private PlayerInput _playerInput;
    [SerializeField] private CardMover _playerCardMover;

    [SerializeField] private PlayerBehaviour _playerBehaviour;

    [SerializeField] private PlayerKnowledge _playerKnowledge;
    [SerializeField] private PlayerKnowledge _opponentKnowledge;

    private Affiliation _playerFaction = Affiliation.Green;
    private Affiliation _aiFaction = Affiliation.Red;

    private PlayerStateVariables _playerStates;

    private bool _hasMovingCard;
    private bool _playerIsDonePlaying;

    #endregion

    #region MONOBEHAVIOUR

    protected override void Start()
    {
        base.Start();
        _playerStates = _knowledge.PlayerStates(_playerFaction);
    }


    protected override void OnEnable()
    {
        base.OnEnable();

        _playerInput.OnCardClicked += CardClicked;

        _playerCardMover.OnCardMovementStarted += CardMovementStarted;
        _playerCardMover.OnCardMovementCompleted += CardMovementEnded;

        _playerIsDonePlaying = false;
        _aiPlayer.ResetPlayedFlag();

        _aiPlayer.PlayCards();
    }

    protected override void Update()
    {
        base.Update();

        if (_playerIsDonePlaying && _aiPlayer.DonePlaying)
        {
            base._isDone = true;
        }
    }

    private void OnDisable()
    {
        _playerInput.OnCardClicked -= CardClicked;

        _playerCardMover.OnCardMovementStarted -= CardMovementStarted;
        _playerCardMover.OnCardMovementCompleted -= CardMovementEnded;

        _humanPlayer.SendUnPlayedCardsToDecks();
        _aiPlayer.SendUnPlayedCardsToDecks();
    }

    #endregion

    #region METHODS

    private void CardClicked(Card card)
    {
        if (card.transform.parent = _playerKnowledge.HandSelf.transform)
        {
            if (!_playerStates.StateActive(PlayerStateVariable.CantPlaySupportCards) || card.CardType != CardType.Support)
            {
                _playerBehaviour.PutFromHandToPlay(card);
                UIManager.Instance.DisplayDoneButton(true);
            }
        }
    }

    private void CardMovementStarted(Card card)
    {
        _hasMovingCard = true;
    }

    private void CardMovementEnded(Card card)
    {
        _hasMovingCard = false;
    }

    public void DoneButtonClicked()
    {
        _playerIsDonePlaying = true;
    }

    #endregion
}
