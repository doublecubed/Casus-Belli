using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class CombatPhase : GameStateBase
{
    private GameStateBase _resolveState;

    [SerializeField] PlayerKnowledge _playerKnowledge;
    [SerializeField] PlayerKnowledge _opponentKnowledge;

    [SerializeField] CardMover _cardMover;

    [SerializeField] List<Card> _playerCards;
    [SerializeField] List<Card> _opponentCards;

    [SerializeField] List<Card> _playerArmyCards;
    [SerializeField] List<Card> _playerSupportCards;

    [SerializeField] List<Card> _opponentArmyCards;
    [SerializeField] List<Card> _opponentSupportCards;

    protected override void OnEnable()
    {
        base.OnEnable();

        _playerCards = _playerKnowledge.AreaSelf.CardsInPlay;
        _opponentCards = _opponentKnowledge.AreaSelf.CardsInPlay;

        _playerArmyCards = _playerCards.Where(x => x.CardType == CardType.Army).ToList();
        _playerSupportCards = _playerCards.Where(x => x.CardType != CardType.Support).ToList();

        _opponentArmyCards = _opponentCards.Where(x => x.CardType == CardType.Army).ToList();
        _opponentSupportCards = _opponentCards.Where(x => x.CardType == CardType.Support).ToList();

        int playerPower = 0;
        int opponentPower = 0;

        for (int i = 0;  i < _playerArmyCards.Count; i++)
        {
            _cardMover.RiseInPlace(_playerCards[i]);
            playerPower += _playerCards[i].Power;
        }

        for (int i = 0; i < _opponentArmyCards.Count; i++)
        {
            _cardMover.RiseInPlace(_opponentCards[i]);
            opponentPower += _opponentCards[i].Power;
        }

        Debug.Log("Player power is: " + playerPower);
        Debug.Log("Opponent power is: " + opponentPower);

        if (playerPower == opponentPower) // both sides lose
        {
            SendCardsToTrash(_playerArmyCards, _playerKnowledge.ArmyTrashSelf);
            SendCardsToTrash(_opponentArmyCards, _opponentKnowledge.ArmyTrashSelf);
        }

        if (playerPower > opponentPower)
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
                    } else
                    {
                        toDeck.Add(_playerArmyCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _playerKnowledge.ArmyTrashSelf);
                SendCardsToDeck(toDeck, _playerKnowledge.ArmyDeckSelf);
            }
            SendCardsToTrash(_opponentArmyCards, _opponentKnowledge.ArmyTrashSelf);
            
        }

        if (opponentPower > playerPower)
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
                    } else
                    {
                        toDeck.Add(_opponentArmyCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _opponentKnowledge.ArmyTrashSelf);
                SendCardsToDeck(toDeck, _opponentKnowledge.ArmyDeckSelf);
            }
            SendCardsToTrash(_playerArmyCards, _playerKnowledge.ArmyTrashSelf);
        }

        SendCardsToTrash(_playerSupportCards, _playerKnowledge.SupportTrashSelf);
        SendCardsToTrash(_opponentSupportCards, _opponentKnowledge.SupportTrashSelf);

        _isDone = true;

    }



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

}
