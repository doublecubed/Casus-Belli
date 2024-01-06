using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatPhase : GameStateBase
{
    #region VARIABLES

    [SerializeField] private PlayerStateVariables _playerStateVariables;
    [SerializeField] private PlayerStateVariables _opponentStateVariables;
    private GameStateBase _resolveState;

    [SerializeField] private CardMover _cardMover;

    [SerializeField] private List<Card> _playerCards;
    [SerializeField] private List<Card> _opponentCards;

    [SerializeField] private List<Card> _playerArmyCards;
    [SerializeField] private List<Card> _playerSupportCards;

    [SerializeField] private List<Card> _opponentArmyCards;
    [SerializeField] private List<Card> _opponentSupportCards;

    [SerializeField] private Deck _playerArmyDeck;
    [SerializeField] private Deck _playerArmyTrash;
    [SerializeField] private Deck _playerSupportTrash;

    [SerializeField] private Deck _opponentArmyDeck;
    [SerializeField] private Deck _opponentArmyTrash;
    [SerializeField] private Deck _opponentSupportTrash;

    private int _playerPower;
    private int _opponentPower;

    #endregion

    #region MONOBEHAVIOUR

    protected override void Start()
    {
        base.Start();

        //_playerArmyDeck = _knowledge.ArmyDeck(_knowledge.HumanFaction());
        //_playerArmyTrash = _knowledge.ArmyTrash(_knowledge.HumanFaction());
        //_playerSupportTrash = _knowledge.SupportTrash(_knowledge.HumanFaction());

        //_opponentArmyDeck = _knowledge.ArmyDeck(_knowledge.ComputerFaction());
        //_opponentArmyTrash = _knowledge.ArmyTrash(_knowledge.ComputerFaction());
        //_opponentSupportTrash = _knowledge.SupportTrash(_knowledge.ComputerFaction());
    }

    protected override void OnEnable()
    {
        base.OnEnable();

        SetCardLists();

        CalculatePowers();

        ResolveCombat();

        SendCardsToTrash(_playerSupportCards, _playerSupportTrash);
        SendCardsToTrash(_opponentSupportCards, _opponentSupportTrash);

        _isDone = true;
    }

    #endregion

    #region METHODS

    private void SendCardsToTrash(List<Card> cards, Deck trashDeck)
    {
        for (int i = 0; i < cards.Count;i++)
        {
            _cardMover.MoveCard(cards[i], trashDeck, trashDeck.transform.position, PlacementFacing.Up, DeckSide.Top, trashDeck.transform.forward);
        }
    }

    private void SendCardsToDeck(List<Card> cards, Deck armyDeck)
    {
        for (int i = 0; i < cards.Count; i++)
        {
            _cardMover.MoveCard(cards[i], armyDeck, armyDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, armyDeck.transform.forward);
        }
    }

    private void SetCardLists()
    {
        _playerCards = _knowledge.PlayArea(_knowledge.HumanFaction()).CardsInPlay;
        _opponentCards = _knowledge.PlayArea(_knowledge.ComputerFaction()).CardsInPlay;

        _playerArmyCards = _playerCards.Where(x => x.CardType == CardType.Army).ToList();
        _playerSupportCards = _playerCards.Where(x => x.CardType == CardType.Support).ToList();

        Debug.Log($"Player has {_playerArmyCards.Count} army cards and {_playerSupportCards.Count} support cards");

        _opponentArmyCards = _opponentCards.Where(x => x.CardType == CardType.Army).ToList();
        _opponentSupportCards = _opponentCards.Where(x => x.CardType == CardType.Support).ToList();

        Debug.Log($"Opponent has {_opponentArmyCards.Count} army cards and {_opponentSupportCards.Count} support cards");
    }

    private void CalculatePowers()
    {
        _playerPower = 0;
        _opponentPower = 0;

        for (int i = 0; i < _playerArmyCards.Count; i++)
        {
            _cardMover.RiseInPlace(_playerArmyCards[i]);

            int powerToAdd = _playerStateVariables.SetArmiesToOne >= 1 ? 1 : _playerArmyCards[i].Power;

            _playerPower += powerToAdd;
        }

        for (int i = 0; i < _opponentArmyCards.Count; i++)
        {
            _cardMover.RiseInPlace(_opponentArmyCards[i]);

            int powerToAdd = _opponentStateVariables.SetArmiesToOne >= 1 ? 1 : _opponentArmyCards[i].Power;
            _opponentPower += powerToAdd;
        }

        Debug.Log("Player power is: " + _playerPower);
        Debug.Log("Opponent power is: " + _opponentPower);
    }

    private void ResolveCombat()
    {
        if (_playerPower == _opponentPower) // both sides lose
        {
            SendCardsToTrash(_playerArmyCards, _playerArmyTrash);
            SendCardsToTrash(_opponentArmyCards, _opponentArmyTrash);
        }

        if (_playerPower > _opponentPower)
        {
            if (_opponentArmyCards.Count != 0)
            {
                int opponentMin = _opponentArmyCards.OrderBy(card => card.Power).First().Power;
                Debug.Log("Opponent's lowest card power is: " + opponentMin);

                List<Card> toTrash = new List<Card>();
                List<Card> toDeck = new List<Card>();
                for (int i = 0; i < _playerArmyCards.Count; i++)
                {
                    if (_playerArmyCards[i].Power < opponentMin)
                    {
                        toTrash.Add(_playerArmyCards[i]);
                    }
                    else
                    {
                        toDeck.Add(_playerArmyCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _playerArmyTrash);
                SendCardsToDeck(toDeck, _playerArmyDeck);
            }
            SendCardsToTrash(_opponentArmyCards, _opponentArmyTrash);

        }

        if (_opponentPower > _playerPower)
        {
            if (_playerArmyCards.Count != 0)
            {
                int playerMin = _playerArmyCards.OrderBy(card => card.Power).First().Power;
                Debug.Log("Opponent's lowest card power is: " + playerMin);

                List<Card> toTrash = new List<Card>();
                List<Card> toDeck = new List<Card>();

                for (int i = 0; i < _opponentArmyCards.Count; i++)
                {
                    if (_opponentArmyCards[i].Power < playerMin)
                    {
                        toTrash.Add(_opponentArmyCards[i]);
                    }
                    else
                    {
                        toDeck.Add(_opponentArmyCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _opponentArmyTrash);
                SendCardsToDeck(toDeck, _opponentArmyDeck);
            }
            SendCardsToTrash(_playerArmyCards, _playerArmyTrash);
        }

    }

    #endregion
}
