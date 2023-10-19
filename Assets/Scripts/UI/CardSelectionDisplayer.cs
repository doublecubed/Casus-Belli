using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardSelectionDisplayer : MonoBehaviour
{
    #region REFERENCES

    private UIManager _uiManager;

    [SerializeField] private Transform _displayParent;

    [SerializeField] private GameObject _cardPrefab;

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }


    #endregion

    #region METHODS

    public void DisplaySelectionCards(List<Card> cardsToDisplay, IButtonClickReceiver receiver)
    {
        for (int i = 0; i < cardsToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            cardImage.Initialize(cardsToDisplay[i].CardImage, i);
            cardImage.OnButtonPressed += receiver.ButtonClicked;
        }

        _uiManager.DisplayCardSelectionUI(true);
    }

    public void ClearSelectionCards()
    {



        int numberOfChildren = _displayParent.childCount;

        for (int i = 0; i < numberOfChildren;i++)
        {
            Destroy(_displayParent.GetChild(i).gameObject);
        }
    }


    #endregion

}
