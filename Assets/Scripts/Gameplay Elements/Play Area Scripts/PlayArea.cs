using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class PlayArea : MonoBehaviour, ICardContainer
{
    [field: SerializeField] public List<Card> CardsInPlay { get; private set; }

    private CardArranger _cardArranger;

    #region MONOBEHAVIOUR

    private void Awake()
    {
        _cardArranger = GetComponent<CardArranger>();
    }

    private void OnEnable()
    {
        EditorApplication.hierarchyChanged += RecreatePlayArea;
    }

    private void OnDisable()
    {
        EditorApplication.hierarchyChanged -= RecreatePlayArea;
    }


    #endregion

    public void Reset()
    {
        CardsInPlay = new List<Card>();
    }

    public Vector3 PlacementPosition()
    {
        return _cardArranger.PlacementPosition(CardsInPlay);
    }

    private void RecreatePlayArea()
    {
        CardsInPlay = new List<Card>();
        for (int i = 0; i < transform.childCount; i++)
        {
            CardsInPlay.Add(transform.GetChild(i).GetComponent<Card>());
        }

    }

    public void AddCard(Card card, DeckSide side)
    {
        card.transform.SetParent(transform);
    }

}
