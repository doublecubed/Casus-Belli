using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class CardArranger : MonoBehaviour
{
    [SerializeField] private CardArrangeType _arrangeType;

    [SerializeField] private float _linearCardInterval;
    [SerializeField] private float _movementDuration;

    public Vector3 PlacementPosition(List<Card> cardList)
    {
        float startingOffset = cardList.Count * (_linearCardInterval * 0.5f);
        
        Vector3 localPlacementPosition = -transform.right * startingOffset;


        for (int i = 0; i < cardList.Count; i++)
        {
            cardList[i].transform.DOLocalMove(localPlacementPosition, _movementDuration);
            localPlacementPosition += transform.right * _linearCardInterval;
        }

        return transform.position + localPlacementPosition;
    }

    public enum CardArrangeType
    {
        Linear,
        Radial
    }
}


