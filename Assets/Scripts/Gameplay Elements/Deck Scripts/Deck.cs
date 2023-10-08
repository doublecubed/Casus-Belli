// -------------------------------------
// Casus Belli Digital
// Written by: Onur Ereren - August 2023
// -------------------------------------


using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using Random = UnityEngine.Random;

public class Deck : MonoBehaviour, ICardContainer
{
    #region REFERENCES

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private Transform _cardParent;

    [SerializeField] private CardSO[] _startingCards;

    #endregion

    #region VARIABLES

    [field: SerializeField] public CardType CardType { get; private set; }
    [field: SerializeField] public DeckType DeckType { get; private set; }
    [field: SerializeField] public Affiliation Faction { get; private set; }

    #endregion

    #region EVENTS

    public event Action OnDeckUpdated;

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        EditorApplication.hierarchyChanged += OnDeckUpdated;
    }

    #endregion



    public int CardsInDeck()
    {
        return _cardParent.childCount;
    }


    public void DealDeck()
    {
        List<Transform> children = Enumerable.Range(0, _cardParent.childCount).Select(i => _cardParent.GetChild(i)).ToList();

        foreach(Transform child in children)
        {
            Destroy(child.gameObject);
        }

        for (int i = 0; i < _startingCards.Length; i++)
        {
            GameObject nextCard = Instantiate(_cardPrefab, _cardParent);
            nextCard.name = _startingCards[i].faction.ToString() + " " + _startingCards[i].cardName;
            nextCard.GetComponent<CardInitializer>().Initialize(_startingCards[i]);
            nextCard.transform.localPosition = Vector3.zero;
            nextCard.transform.forward = Vector3.down;
        }

        OnDeckUpdated?.Invoke();
    }

    public void Shuffle()
    {
        Transform[] children = new Transform[_cardParent.childCount];
        for (int i = 0; i < _cardParent.childCount; i++)
        {
            children[i] = _cardParent.GetChild(i);
        }

        foreach(Transform child in children)
        {
            child.SetParent(null);
        }

        for (int i = 0; i < children.Length; i++)
        {
            int randomIndex = Random.Range(0, children.Length);

            Transform temp = children[i];
            children[i] = children[randomIndex];
            children[randomIndex] = temp;
        }

        for (int i = 0; i < children.Length; i++)
        {
            children[i].SetParent(_cardParent);
        }

    }

    public void AddCard(Card card, DeckSide side)
    {
        PutCard(card, side);
    }

    public void PutCard(Card card, DeckSide side)
    {
        card.transform.SetParent(_cardParent);

        if (side == DeckSide.Top) return;

        if (side == DeckSide.Bottom) card.transform.SetAsFirstSibling();

        if (side == DeckSide.Random)
        {
            int randomIndex = Random.Range(0, _cardParent.childCount);
            card.transform.SetSiblingIndex(randomIndex);
        }


    }

    

    public Card DrawFrom(DeckSide side)
    {
        int childIndex = 0;

        if (side == DeckSide.Top) childIndex = _cardParent.childCount - 1;

        if (side == DeckSide.Random) childIndex = Random.Range(0, _cardParent.childCount - 1);

        Transform cardToReturn = _cardParent.GetChild(childIndex);
        cardToReturn.SetParent(null);

        return cardToReturn.GetComponent<Card>();
    }

    public List<Card> FindCards(List<CardSO> cardsToSearch)
    {
        List<Card> cardsFound = new List<Card>();

        Card[] children = new Card[_cardParent.childCount];
        for (int i = 0; i < _cardParent.childCount; i++)
        {
            children[i] = _cardParent.GetChild(i).GetComponent<Card>();
        }

        for (int i = 0; i < children.Length; i++)
        {
            for (int j = 0; j < cardsToSearch.Count; j++)
            {
                if (children[i].CardName == cardsToSearch[j].cardName)
                {
                    cardsFound.Add(children[i]);
                    children[i].transform.SetParent(null);
                }
            }
        }

        Shuffle();

        return cardsFound;
    }

    public List<Card> TakeCards(DeckSide side, int numberOfCards)
    {
        List<Card> takenCards = new List<Card>();

        for (int i = 0; i < numberOfCards; i++)
        {
            if (_cardParent.childCount == 0) break;

            int pickIndex = 0;

            if (side == DeckSide.Top) pickIndex = _cardParent.childCount - 1;
            if (side == DeckSide.Random) pickIndex = Random.Range(0, _cardParent.childCount);

            Transform cardPicked = _cardParent.GetChild(pickIndex);
            cardPicked.transform.SetParent(null);
            takenCards.Add(cardPicked.GetComponent<Card>());
        }
        
        return takenCards;
    }

    public List<Card> LookAtCards(DeckSide side, int numberOfCards)
    {
        List<Card> lookedCards = LookAtCards(side, numberOfCards, 0);
        return lookedCards;
    }

    public List<Card> LookAtCards(DeckSide side, int numberOfCards, int cardsToSkip)
    {
        if (side == DeckSide.Random) return new List<Card>();

        List<Card> lookedCards = new List<Card>();

        for (int i =  0; i < numberOfCards; i++)
        {
            int index = i + cardsToSkip;
            if (side == DeckSide.Top) index = _cardParent.childCount - 1 - i - cardsToSkip;
            if (index >= _cardParent.childCount - 1) break;

            lookedCards.Add(_cardParent.transform.GetChild(index).GetComponent<Card>());
        }

        return lookedCards;
    }
}

public enum DeckSide
{
    Top,
    Bottom,
    Random
}

public enum DeckType
{
    Draw,
    Trash
}