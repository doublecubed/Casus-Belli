using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class DeckCounter : MonoBehaviour
{
    [SerializeField] private TextMeshPro _counterText;

    private Deck _deck;

    private void Awake()
    {
        _deck = GetComponent<Deck>();
        _deck.OnDeckUpdated += UpdateCounter;
    }

    private void UpdateCounter()
    {
        _counterText.text = _deck.NumberOfCardsInDeck().ToString();
    }


}
