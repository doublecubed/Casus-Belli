using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvciTrapCardAbility : AbilityBase
{
    [SerializeField] private PlayerKnowledge _selfKnowledge;
    [SerializeField] private PlayArea _opponentPlayArea;

    [SerializeField] private CardMover _cardMover;
    [SerializeField] private Deck _deckToSend;
    [SerializeField] private Card _selectedCard;


    public override void Initialize()
    {
        _selfKnowledge = GetComponentInParent<PlayerKnowledge>();
        _cardMover = GetComponentInParent<CardMover>();
        _opponentPlayArea = _selfKnowledge.AreaOpponent;
        _deckToSend = _selfKnowledge.ArmyDeckSelf;
    }

    public override void UseAbility()
    {
        List<Card> opponentSupportCards = new List<Card>();
        for (int i = 0; i < _opponentPlayArea.transform.childCount; i++)
        {
            Card card = _opponentPlayArea.transform.GetChild(i).GetComponent<Card>();

            if (card.CardType != CardType.Army) continue;

            opponentSupportCards.Add(card);
        }

        if (opponentSupportCards.Count == 0) return;

        // SELECT CARD TO BE INSERTED HERE
        _selectedCard = opponentSupportCards[0];
        // -------------------------------

        _cardMover.MoveCard(_selectedCard, _deckToSend, _deckToSend.transform.position, PlacementFacing.Down, DeckSide.Bottom, _selfKnowledge.TableDirection);
    }


}
