using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using System;
using static UnityEngine.GraphicsBuffer;

public class CardMover : MonoBehaviour
{
    private Camera _camera;

    [SerializeField] private float _defaultMoveDuration;
    [SerializeField] private float _defaultRiseDistance;

    public event Action<Card> OnCardMovementCompleted;
    public event Action<Card> OnCardMovementStarted;

    private void OnEnable()
    {
        _camera = Camera.main;
    }

    public void MoveCard(Card card, ICardContainer container, Vector3 position, PlacementFacing facing, DeckSide order, Vector3 lookDirection)
    {
        OnCardMovementStarted?.Invoke(card);

        card.transform.DOMove(position, _defaultMoveDuration).OnComplete(() => 
        { 
            container.AddCard(card, order); 
            OnCardMovementCompleted?.Invoke(card); 
        });
        card.transform.DORotateQuaternion(CardRotation(facing, lookDirection), _defaultMoveDuration);
    }

    public void MoveCard(Card card, ICardContainer target, Vector3 position, PlacementFacing facing, Vector3 lookDirection)
    {
        MoveCard(card, target, position, facing, DeckSide.Bottom, lookDirection);
    }


    public Quaternion CardRotation(PlacementFacing facing, Vector3 lookDirection)
    {
        if (facing == PlacementFacing.Up) return Quaternion.LookRotation(Vector3.up, lookDirection);

        if (facing == PlacementFacing.Down) return Quaternion.LookRotation(Vector3.down, lookDirection);

        if (facing == PlacementFacing.ToCamera) return Quaternion.LookRotation(-_camera.transform.forward, Vector3.up);

        if (facing == PlacementFacing.FromCamera) return Quaternion.LookRotation(_camera.transform.forward, Vector3.up);

        return Quaternion.identity;
    }

    public void FlipCardUp(Card card, Vector3 lookRotation)
    {
        Quaternion newRotation = CardRotation(PlacementFacing.Up, lookRotation);
        card.transform.DORotateQuaternion(newRotation, _defaultMoveDuration);
    }

    public void RiseInPlace(Card card)
    {
        OnCardMovementStarted?.Invoke(card);

        card.transform.DOMove(Vector3.up * _defaultRiseDistance, _defaultMoveDuration).SetRelative(true).OnComplete(() => 
        { 
            OnCardMovementCompleted?.Invoke(card);
        });
    }

    public void LowerInPlace(Card card)
    {
        OnCardMovementStarted?.Invoke(card);

        card.transform.DOMove(Vector3.down * _defaultRiseDistance, _defaultMoveDuration).SetRelative(true).OnComplete(() =>
        {
            OnCardMovementCompleted?.Invoke(card);
        });
    }

}

public enum PlacementFacing
{
    Up,
    Down,
    ToCamera,
    FromCamera
}
