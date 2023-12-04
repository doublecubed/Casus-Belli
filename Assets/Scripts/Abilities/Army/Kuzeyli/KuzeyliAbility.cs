using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KuzeyliAbility : AbilityBase
{
    [SerializeField] private GlobalKnowledge _knowledge;
    [SerializeField] private CardMover _mover;
    [SerializeField] private Card _selfCard;
    [SerializeField] private Deck _opponentSupportDeck;

    [SerializeField] private Affiliation _targetFaction;
    [SerializeField] private Vector3 _deckLookDirection;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;

        _targetFaction = _selfCard.Faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;

        _mover = _knowledge.Mover(_targetFaction);
        _opponentSupportDeck = _knowledge.SupportDeck(_targetFaction);
        _deckLookDirection = _knowledge.LookDirection(_targetFaction);

        _abilityPhase.Add(RiseCard);
        _abilityPhase.Add(ReturnCards);
        _abilityPhase.Add(LowerCard);

        base.Initialize();
    }

    private void RiseCard()
    {
        Debug.Log($"Rising Card {_selfCard.name}");
        _mover.RiseInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void ReturnCards()
    {
        Debug.Log($"Updating Power for {_selfCard.name}");

        List<Card> cardsInPlay = _knowledge.PlayArea(_targetFaction).CardsInPlay;

        for (int i = 0; i < cardsInPlay.Count; i++)
        {
            if (cardsInPlay[i].CardType == CardType.Support && cardsInPlay[i].Priority == CardPriority.VerySlow)
            {
                _mover.MoveCard(cardsInPlay[i], _opponentSupportDeck, _opponentSupportDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _deckLookDirection);
                _knowledge.AbilityPhase.RemoveCardFromStack(cardsInPlay[i]);
            }
        }

        _phaseCompleted = true;
    }

    private void LowerCard()
    {
        Debug.Log($"Lowering Card {_selfCard.name}");
        _mover.LowerInPlace(_selfCard);
        _mover.OnCardMovementCompleted += CardMovementDone;
    }

    private void CardMovementDone(Card card)
    {
        Debug.Log("KuzeyliAbility card movement done");
        _phaseCompleted = true;
        _mover.OnCardMovementCompleted -= CardMovementDone;

        if (_phaseIndex == 2)
        {
            AbilityCompleted();
            Debug.Log("KuzeyliAbility completed");
        }
    }


}
