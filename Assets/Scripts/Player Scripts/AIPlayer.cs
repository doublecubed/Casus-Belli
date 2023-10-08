using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private PlayerKnowledge _playerKnowledge;
    private PlayerBehaviour _playerBehaviour;
    private PlayerStats _playerStats;

    public bool DoneDrawing {  get; private set; }
    public bool DonePlaying { get; private set; }

    private void Awake()
    {
        _playerKnowledge = GetComponent<PlayerKnowledge>();
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _playerStats = GetComponent<PlayerStats>();
    }


    public void DrawCards()
    {
        StartCoroutine(DrawCardsToHand());
    }

    public void PlayCards()
    {
        int numberToPlay = Random.Range(1, _playerKnowledge.HandSelf.CardsInHand.Count + 1);

        StartCoroutine(PlayCardsOnTable(numberToPlay));

    }



    private IEnumerator PlayCardsOnTable(int numberToPlay)
    {
        for (int i = 0; i < numberToPlay; i++)
        {
            _playerBehaviour.PutFromHandToPlay(_playerKnowledge.HandSelf.CardsInHand[0]);

            yield return new WaitForSeconds(1);
        }

        DonePlaying = true;
    }

    private IEnumerator DrawCardsToHand()
    {
        for (int i = 0; i < _playerStats.MaxCardsPerRound; i++)
        {
            if ( i == 0)
            {
                DrawFromDeck(_playerKnowledge.ArmyDeckSelf);
            }
            else
            {
                if (_playerKnowledge.ArmyDeckSelf.CardsInDeck() == 0 && _playerKnowledge.SupportDeckSelf.CardsInDeck() == 0)
                {
                    continue;
                }

                int chooseDeck = Random.Range(0, 2);
                DrawFromDeck(chooseDeck == 0 ? _playerKnowledge.ArmyDeckSelf : _playerKnowledge.SupportDeckSelf);
            }

            yield return new WaitForSeconds(1);
        }

        DoneDrawing = true;

    }

    public void SendUnPlayedCardsToDecks()
    {
        int numberOfCards = _playerKnowledge.HandSelf.CardsInHand.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            _playerBehaviour.PutBackAtDeckBottom(_playerKnowledge.HandSelf.CardsInHand[0]);
        }
    }

    public void ResetPlayedFlag()
    {
        DonePlaying = false;
    }

    private void DrawFromDeck(Deck deck)
    {
        if (deck.CardsInDeck() == 0)
        {
            _playerBehaviour.DrawFromDeckToHand(deck == _playerKnowledge.ArmyDeckSelf ? _playerKnowledge.SupportDeckSelf : _playerKnowledge.ArmyDeckSelf);
        }
        else
        {
            _playerBehaviour.DrawFromDeckToHand(deck);
        }
    }
}
