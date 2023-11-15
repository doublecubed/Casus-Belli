using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AIPlayer : MonoBehaviour
{
    private GlobalKnowledge _knowledge;
    private PlayerBehaviour _playerBehaviour;
    private PlayerStateVariables _playerVariables;
    private Affiliation _faction;

    private Hand _selfHand;
    private Deck _selfArmyDeck;
    private Deck _selfSupportDeck;

    public bool DoneDrawing {  get; private set; }
    public bool DonePlaying { get; private set; }


    private void OnEnable()
    {
        _knowledge = GlobalKnowledge.Instance;
        _playerVariables = GetComponent<PlayerStateVariables>();
        Debug.Log("PlayerVariables = " + _playerVariables.name.ToString());
        _faction = _playerVariables.Faction;

        _playerBehaviour = GetComponent<PlayerBehaviour>();

        _selfHand = _knowledge.Hand(_faction);
        _selfArmyDeck = _knowledge.ArmyDeck(_faction);
        _selfSupportDeck = _knowledge.SupportDeck(_faction);
    }

    public void DrawCards()
    {
        StartCoroutine(DrawCardsToHand());
    }

    public void PlayCards()
    {
        int numberToPlay = Random.Range(1, _selfHand.CardsInHand.Count + 1);

        StartCoroutine(PlayCardsOnTable(numberToPlay));

    }


    private IEnumerator PlayCardsOnTable(int numberToPlay)
    {
        for (int i = 0; i < numberToPlay; i++)
        {
            _playerBehaviour.PutFromHandToPlay(_selfHand.CardsInHand[0]);

            yield return new WaitForSeconds(1);
        }

        DonePlaying = true;
    }

    private IEnumerator DrawCardsToHand()
    {
        Debug.Log($"Drawing {_playerVariables.CardsToDraw} cards to hand");

        for (int i = 0; i < _playerVariables.CardsToDraw; i++)
        {
            if ( i == 0)
            {
                DrawFromDeck(_selfArmyDeck);
            }
            else
            {
                if (_selfArmyDeck.NumberOfCardsInDeck() == 0 && _selfSupportDeck.NumberOfCardsInDeck() == 0)
                {
                    continue;
                }

                int chooseDeck = Random.Range(0, 2);
                DrawFromDeck(chooseDeck == 0 ? _selfArmyDeck : _selfSupportDeck);
            }

            yield return new WaitForSeconds(1);
        }

        DoneDrawing = true;

    }

    public void SendUnPlayedCardsToDecks()
    {
        int numberOfCards = _selfHand.CardsInHand.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            _playerBehaviour.PutBackAtDeckBottom(_selfHand.CardsInHand[0]);
        }
    }

    public void ResetPlayedFlag()
    {
        DonePlaying = false;
    }

    private void DrawFromDeck(Deck deck)
    {
        if (deck.NumberOfCardsInDeck() == 0)
        {
            _playerBehaviour.DrawFromDeckToHand(deck == _selfArmyDeck ? _selfSupportDeck : _selfArmyDeck);
        }
        else
        {
            _playerBehaviour.DrawFromDeckToHand(deck);
        }
    }
}
