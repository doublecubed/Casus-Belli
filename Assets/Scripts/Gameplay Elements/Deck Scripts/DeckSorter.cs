using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeckSorter : MonoBehaviour
{
    private Deck _deck;

    [SerializeField] private Transform _cardParent;
    [SerializeField] private float _cardInterval;

    private void Awake()
    {
        _deck = GetComponent<Deck>();
        _deck.OnDeckUpdated += SortCards;
    }

    public void SortCards()
    {
        for (int i = 0; i < _cardParent.childCount; i++)
        {
            _cardParent.GetChild(i).localPosition = Vector3.up * _cardInterval * i;
        }
    }

}
