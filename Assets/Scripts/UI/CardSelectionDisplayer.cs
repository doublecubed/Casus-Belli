using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class CardSelectionDisplayer : MonoBehaviour
{
    #region REFERENCES

    private UIManager _uiManager;



    [SerializeField] private Transform _displayParent;

    [SerializeField] private GameObject _cardPrefab;
    [SerializeField] private GameObject _tokenPrefab;

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }


    #endregion

    #region METHODS

    public void DisplaySelection(List<Card> cardsToDisplay, IButtonClickReceiver receiver)
    {
        List<Sprite> sprites = cardsToDisplay.Select(card => card.CardImage).ToList();

        DisplaySelection(sprites, receiver);
    }

    public void DisplaySelection(List<Sprite> spritesToDisplay, IButtonClickReceiver receiver)
    {
        ClearSelectionCards();

        for (int i = 0; i < spritesToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            cardImage.Initialize(spritesToDisplay[i], i);
            cardImage.OnButtonPressed += receiver.ButtonClicked;

            // TODO: These events have to be unsubscribed in some way after selection is done.
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
