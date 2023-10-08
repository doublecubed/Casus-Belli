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

    protected override void OnEnable()
    {
        base.OnEnable();

        _playerCards = _playerKnowledge.AreaSelf.CardsInPlay;
        _opponentCards = _opponentKnowledge.AreaSelf.CardsInPlay;

        int playerPower = 0;
        int opponentPower = 0;

        for (int i = 0;  i < _playerCards.Count; i++)
        {
            if (_playerCards[i].CardType == CardType.Army)
            {
                _cardMover.RiseInPlace(_playerCards[i]);
                playerPower += _playerCards[i].Power;
            }

        }

        for (int i = 0; i < _opponentCards.Count; i++)
        {
            if (_opponentCards[i].CardType == CardType.Army)
            {
                _cardMover.RiseInPlace(_opponentCards[i]);
                opponentPower += _opponentCards[i].Power;
            }
        }

        Debug.Log("Player power is: " + playerPower);
        Debug.Log("Opponent power is: " + opponentPower);

        if (playerPower == opponentPower) // both sides lose
        {
            SendCardsToTrash(_playerCards, _playerKnowledge.ArmyTrashSelf);
            SendCardsToTrash(_opponentCards, _opponentKnowledge.ArmyTrashSelf);
        }

        if (playerPower > opponentPower)
        {
            if (_opponentCards.Count != 0)
            {
                int opponentMin = _opponentCards.OrderBy(card => card.Power).First().Power;
                Debug.Log("Opponent's lowest card power is: " + opponentMin);

                List<Card> toTrash = new List<Card>();
                List<Card> toDeck = new List<Card>();
                for (int i = 0; i < _playerCards.Count; i++)
                {
                    if (_playerCards[i].Power < opponentMin)
                    {
                        toTrash.Add(_playerCards[i]);
                    } else
                    {
                        toDeck.Add(_playerCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _playerKnowledge.ArmyTrashSelf);
                SendCardsToDeck(toDeck, _playerKnowledge.ArmyDeckSelf);
            }
            SendCardsToTrash(_opponentCards, _opponentKnowledge.ArmyTrashSelf);
            
        }

        if (opponentPower > playerPower)
        {
            if (_playerCards.Count != 0)
            {
                int playerMin = _playerCards.OrderBy(card => card.Power).First().Power;
                Debug.Log("Opponent's lowest card power is: " + playerMin);

                List<Card> toTrash = new List<Card>();
                List<Card> toDeck = new List<Card>();

                for (int i = 0; i < _opponentCards.Count; i++)
                {
                    if (_opponentCards[i].Power < playerMin)
                    {
                        toTrash.Add(_opponentCards[i]);
                    } else
                    {
                        toDeck.Add(_opponentCards[i]);
                    }
                }

                SendCardsToTrash(toTrash, _opponentKnowledge.ArmyTrashSelf);
                SendCardsToDeck(toDeck, _opponentKnowledge.ArmyDeckSelf);
            }
            SendCardsToTrash(_playerCards, _playerKnowledge.ArmyTrashSelf);
        }

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
