using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimyaciDrawTopSupportCard : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _selfMover;

    private Affiliation _targetFaction;
    private Deck _opponentSupportDeck;
    private Deck _opponentSupportTrash;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _selfMover = _knowledge.Mover(_selfCard.Faction);

        _targetFaction = _knowledge.OpponentFaction(_selfCard.Faction);
        _opponentSupportDeck = _knowledge.SupportDeck(_targetFaction);
        _opponentSupportTrash = _knowledge.SupportTrash(_targetFaction);

        base._abilityPhase.Add(CheckTopCard);

        base.Initialize();
    }

    private void CheckTopCard()
    {
        List<Card> cards = _opponentSupportDeck.LookAtCards(DeckSide.Top, 1);

        if (cards.Count <= 0)
        {
            base.AbilityCompleted();
            return;
        }

        Card selectedCard = _opponentSupportDeck.LookAtCards(DeckSide.Top, 1)[0];

        if (selectedCard.Priority == CardPriority.VerySlow || selectedCard.Priority == CardPriority.Slow)
        {
            _selfMover.MoveCard(selectedCard, _opponentSupportTrash, _opponentSupportTrash.transform.position, PlacementFacing.Up, DeckSide.Top, _knowledge.LookDirection(_targetFaction));
        }

        base.AbilityCompleted();
    }
}
