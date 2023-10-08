using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardInitializer : MonoBehaviour
{
    [field: SerializeField] public CardSO CardObject;

    private Card _card;
    private CardDisplayer _displayer;

    private void Awake()
    {
        _card = GetComponent<Card>();
        _displayer = GetComponent<CardDisplayer>();
    }

    public void Initialize(CardSO cardSO)
    {
        CardObject = cardSO;

        _card.Initialize(CardObject);
        _displayer.Initialize(CardObject);
        
    }

}
