using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardDisplayer : MonoBehaviour
{
    private CardSO _cardObject;

    [SerializeField] private MeshRenderer frontFace;
    [SerializeField] private MeshRenderer backFace;

    public bool IsVisible { get; private set; }

    public void Initialize(CardSO cardSO)
    {
        _cardObject = cardSO;

        frontFace.material.SetTexture("_BaseMap", _cardObject.faceImage);
        backFace.material.SetTexture("_BaseMap", _cardObject.backImage);
    }


    public void DisplayCard()
    {
        frontFace.enabled = true;
        backFace.enabled = true;

        IsVisible = true;
    }

    public void HideCard()
    {
        frontFace.enabled = false;
        backFace.enabled = false;

        IsVisible = false;
    }

}
