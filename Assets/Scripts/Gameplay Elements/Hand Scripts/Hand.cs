using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Hand : MonoBehaviour, ICardContainer
{
    private CardArranger _arranger;

    [field: SerializeField] public List<Card> CardsInHand { get; private set; }


    #region MONOBEHAVIOUR

    private void Awake()
    {
        _arranger = GetComponent<CardArranger>();
    }

    private void OnEnable()
    {
        //HierarchySnapshotter.Instance.OnHierarchyChanged += RecreateHand;
    }

    private void OnDisable()
    {
        //HierarchySnapshotter.Instance.OnHierarchyChanged -= RecreateHand;
    }

    #endregion

    public Vector3 PlacementPosition()
    {
        return _arranger.PlacementPosition(CardsInHand);
    }


    private void RecreateHand()
    {
        CardsInHand = new List<Card>();
        for (int i = 0; i < transform.childCount; i++)
        {
            CardsInHand.Add(transform.GetChild(i).GetComponent<Card>());
        }
    }

    public void RemoveCard(Card card)
    {
        card.transform.parent = null;
        RecreateHand();
    }

    public void AddCard(Card card, DeckSide side)
    {
        card.transform.SetParent(transform);
        RecreateHand();
    }

}
