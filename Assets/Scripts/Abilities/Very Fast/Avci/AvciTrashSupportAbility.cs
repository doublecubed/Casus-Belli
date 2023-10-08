using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AvciTrashSupportAbility : AbilityBase
{
    [SerializeField] private PlayerKnowledge _selfKnowledge;
    [SerializeField] private CardMover _cardMover;

    [SerializeField] private Deck _deckToSend;
    [SerializeField] private PlayArea _opponentPlayArea;

    [SerializeField] private Card _selectedCard;

    public override void Initialize()
    {
        _cardMover = GetComponentInParent<CardMover>();
        _selfKnowledge = GetComponentInParent<PlayerKnowledge>();
        _deckToSend = _selfKnowledge.SupportTrashOpponent;
        _opponentPlayArea = _selfKnowledge.AreaOpponent;
    }

    public override void UseAbility()
    {
        List<Card> opponentSupportCards = new List<Card>();
        for (int i = 0; i < _opponentPlayArea.transform.childCount; i++)
        {
            Card card = _opponentPlayArea.transform.GetChild(i).GetComponent<Card>();

            if (card.CardType != CardType.Support) continue;

            opponentSupportCards.Add(card);
        }

        if (opponentSupportCards.Count == 0) return;

        // SELECT CARD TO BE INSERTED HERE
        _selectedCard = opponentSupportCards[0];
        // -------------------------------

        _cardMover.MoveCard(_selectedCard, _deckToSend, _deckToSend.transform.position, PlacementFacing.Up, DeckSide.Top, _selfKnowledge.TableDirection);

    }
}
