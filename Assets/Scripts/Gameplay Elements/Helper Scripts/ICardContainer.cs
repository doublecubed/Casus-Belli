using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ICardContainer
{
    public void AddCard(Card card, DeckSide side);
}
