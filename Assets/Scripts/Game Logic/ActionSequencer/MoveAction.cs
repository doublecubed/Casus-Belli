using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEngine;

public class MoveAction : GameAction
{
    private Card _card;
    private CardMover _mover;
    private ICardContainer _targetContainer;
    private Vector3 _containerPosition;
    private PlacementFacing _facing;
    private DeckSide _deckSide;
    private Vector3 _lookDirection;

    public MoveAction(Card card, CardMover mover, ICardContainer targetContainer, Vector3 position, PlacementFacing facing, DeckSide order, Vector3 lookDirection)
    {
        _card = card;
        _mover = mover;
        _targetContainer = targetContainer;
        _containerPosition = position;
        _facing = facing;
        _deckSide = order;
        _lookDirection = lookDirection;
    }

    public override async UniTask ExecuteAction(CancellationToken token)
    {

    }

}
