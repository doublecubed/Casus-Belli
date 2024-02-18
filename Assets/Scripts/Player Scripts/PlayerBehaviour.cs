using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    [SerializeField] GlobalKnowledge _globalKnowledge;
    public Affiliation SelfFaction;

    [SerializeField] private CardMover _cardMover;
    private PlayerStateVariables _selfStates;

    private void Awake()
    {
        _selfStates = GetComponent<PlayerStateVariables>();
    }

    public void DrawFromDeckToHand(Deck deckDrawnFrom)
    {
        if (deckDrawnFrom.NumberOfCardsInDeck() == 0) return;

        Card drawnCard = deckDrawnFrom.DrawFrom(DeckSide.Top);
        Transform targetParent = _globalKnowledge.Hand(SelfFaction).transform;
        Vector3 targetPosition = _globalKnowledge.Hand(SelfFaction).PlacementPosition();
        Vector3 targetRotation = new Vector3(-126f, 0f, 180f);

        PlacementFacing facing = SelfFaction == Affiliation.Red ? PlacementFacing.FromCamera : PlacementFacing.ToCamera;

        _cardMover.MoveCard(drawnCard, _globalKnowledge.Hand(SelfFaction), targetPosition, facing, _globalKnowledge.LookDirection(SelfFaction));

    }

    public Card PutFromDeckToPlay(Deck deckDrawnFrom)
    {
        if (deckDrawnFrom.NumberOfCardsInDeck() == 0) return null;
        
        Card drawnCard = deckDrawnFrom.DrawFrom(DeckSide.Top);
        PlayArea targetArea = _globalKnowledge.PlayArea(SelfFaction);
        Vector3 targetPosition = _globalKnowledge.PlayArea(SelfFaction).PlacementPosition();

        _cardMover.MoveCard(drawnCard, targetArea, targetPosition, PlacementFacing.Up, _globalKnowledge.LookDirection(SelfFaction));

        return drawnCard;
    }

    public void PutFromHandToPlay(Card cardToPlay)
    {
        Transform targetParent = _globalKnowledge.PlayArea(SelfFaction).transform;
        Vector3 targetPosition = _globalKnowledge.PlayArea(SelfFaction).PlacementPosition();
        Vector3 targetRotation = targetParent.rotation.eulerAngles;

        _cardMover.MoveCard(cardToPlay, _globalKnowledge.PlayArea(SelfFaction), targetPosition, PlacementFacing.Down, _globalKnowledge.LookDirection(SelfFaction));

        //_cardMover.MoveCard(cardToPlay, targetParent, targetPosition, PlacementFacing.Up);
    }

    public void FlipCardOver(Card card)
    {

    }


    public void PutBackAtDeckBottom(Card card)
    {
        _cardMover.MoveCard(card, card.StartingDeck, card.StartingDeck.transform.position, PlacementFacing.Down, _globalKnowledge.LookDirection(SelfFaction));
    }

    public void SelectAbility(Card card, AbilityBase[] abilities)
    {
        Debug.Log($"Player name is {gameObject.name}");

        if (!_selfStates.AIPlayer)
        {
            Debug.Log("Player is human");
            List<Card> cardList = new List<Card>();
            List<string> abilityDefinition = new List<string>();

            for (int i = 0; i < abilities.Length; i++)
            {
                cardList.Add(card);
                abilityDefinition.Add(card.Abilities[i].GetComponent<AbilityBase>().AbilityDescription);
            }

            UIManager.Instance.GetComponent<CardSelectionDisplayer>().DisplaySelection(cardList, card, abilityDefinition);
        } else
        {
            Debug.Log("Player is AI");
            abilities[0].UseAbility();
        }
    }
}
