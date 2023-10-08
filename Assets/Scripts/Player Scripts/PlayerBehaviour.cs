using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehaviour : MonoBehaviour
{
    private PlayerKnowledge _playerKnowledge;
    private CardMover _cardMover;

    private void Awake()
    {
        _playerKnowledge = GetComponent<PlayerKnowledge>();
        _cardMover = GetComponent<CardMover>();
    }


    public void DrawFromDeckToHand(Deck deckDrawnFrom)
    {
        if (deckDrawnFrom.CardsInDeck() == 0) return;

        Card drawnCard = deckDrawnFrom.DrawFrom(DeckSide.Top);
        Transform targetParent = _playerKnowledge.HandSelf.transform;
        Vector3 targetPosition = _playerKnowledge.HandSelf.PlacementPosition();
        Vector3 targetRotation = new Vector3(-126f, 0f, 180f);

        PlacementFacing facing = _playerKnowledge.ArmyDeckSelf.Faction == Affiliation.Red ? PlacementFacing.FromCamera : PlacementFacing.ToCamera;

        _cardMover.MoveCard(drawnCard, _playerKnowledge.HandSelf, targetPosition, facing, _playerKnowledge.TableDirection);

    }

    public Card PutFromDeckToPlay(Deck deckDrawnFrom)
    {
        if (deckDrawnFrom.CardsInDeck() == 0) return null;
        
        Card drawnCard = deckDrawnFrom.DrawFrom(DeckSide.Top);
        PlayArea targetArea = _playerKnowledge.AreaSelf;
        Vector3 targetPosition = _playerKnowledge.AreaSelf.PlacementPosition();

        _cardMover.MoveCard(drawnCard, targetArea, targetPosition, PlacementFacing.Up, _playerKnowledge.TableDirection);

        return drawnCard;
    }

    public void PutFromHandToPlay(Card cardToPlay)
    {
        Transform targetParent = _playerKnowledge.AreaSelf.transform;
        Vector3 targetPosition = _playerKnowledge.AreaSelf.PlacementPosition();
        Vector3 targetRotation = targetParent.rotation.eulerAngles;

        _cardMover.MoveCard(cardToPlay, _playerKnowledge.AreaSelf, targetPosition, PlacementFacing.Down, _playerKnowledge.TableDirection);

        //_cardMover.MoveCard(cardToPlay, targetParent, targetPosition, PlacementFacing.Up);
    }

    public void FlipCardOver(Card card)
    {

    }


    public void PutBackAtDeckBottom(Card card)
    {
        _cardMover.MoveCard(card, card.StartingDeck, card.StartingDeck.transform.position, PlacementFacing.Down, _playerKnowledge.TableDirection);
    }
}
