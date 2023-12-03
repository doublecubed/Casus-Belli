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

    private List<CardImage> _cardImageComponents;

    private List<string> _dedfaultDefinitions = new List<string>();

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        _uiManager = GetComponent<UIManager>();
    }


    #endregion

    #region METHODS

    public void DisplaySelection(List<Card> cardsToDisplay, IButtonClickReceiver receiver, List<string> definitions)
    {
        ClearSelectionCards();

        List<Sprite> spritesToDisplay = cardsToDisplay.Select(card => card.CardImage).ToList();

        _cardImageComponents = new List<CardImage>();

        for (int i = 0; i < spritesToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            _cardImageComponents.Add(cardImage);
            cardImage.Initialize(spritesToDisplay[i], i, definitions[i]);
            cardImage.OnButtonPressed += receiver.ButtonClicked;
            cardImage.OnButtonPressed += ButtonClicked;

            // TODO: These events have to be unsubscribed in some way after selection is done.
        }

        _uiManager.DisplayCardSelectionUI(true);
    }

    public void DisplaySelection(List<Card> cardsToDisplay, IButtonClickReceiver receiver)
    {
        List<Sprite> sprites = cardsToDisplay.Select(card => card.CardImage).ToList();

        DisplaySelection(sprites, receiver);
    }

    public void DisplaySelection(List<Sprite> spritesToDisplay, IButtonClickReceiver receiver)
    {
        ClearSelectionCards();

        _cardImageComponents = new List<CardImage>();

        for (int i = 0; i < spritesToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            _cardImageComponents.Add(cardImage);
            cardImage.Initialize(spritesToDisplay[i], i);
            cardImage.OnButtonPressed += receiver.ButtonClicked;
            cardImage.OnButtonPressed += ButtonClicked;

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

    private void ButtonClicked(int index)
    {
        for (int i = 0; i < _cardImageComponents.Count; i++)
        {
            _cardImageComponents[i].OnButtonPressed -= ButtonClicked;
        }

        _uiManager.DisplayCardSelectionUI(false); 
    }


    #endregion

}
