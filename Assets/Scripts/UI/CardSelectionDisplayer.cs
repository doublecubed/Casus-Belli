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

    private IButtonClickReceiver _currentReceiver;

    private bool _cardSelected;

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
        Debug.Log($"First display selection overload running, with {receiver} as the receiver");

        ClearSelectionCards();

        List<Sprite> spritesToDisplay = cardsToDisplay.Select(card => card.CardImage).ToList();

        _currentReceiver = receiver;

        _cardImageComponents = new List<CardImage>();

        for (int i = 0; i < spritesToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            _cardImageComponents.Add(cardImage);
            cardImage.Initialize(spritesToDisplay[i], i, definitions[i]);
            cardImage.OnButtonPressed += ButtonClickedFirst;
            cardImage.OnButtonPressed += _currentReceiver.ButtonClicked;


            // TODO: These events have to be unsubscribed in some way after selection is done.
        }

        _uiManager.DisplayCardSelectionUI(true);
    }

    public void DisplaySelection(List<Card> cardsToDisplay, IButtonClickReceiver receiver)
    {
        Debug.Log($"Second display selection overload running, with {receiver} as the receiver");

        _cardSelected = false;

        List<Sprite> sprites = cardsToDisplay.Select(card => card.CardImage).ToList();

        DisplaySelection(sprites, receiver);
    }

    public void DisplaySelection(List<Sprite> spritesToDisplay, IButtonClickReceiver receiver)
    {
        Debug.Log($"Third display selection overload running, with {receiver} as the receiver");

        ClearSelectionCards();

        _cardImageComponents = new List<CardImage>();

        for (int i = 0; i < spritesToDisplay.Count; i++)
        {
            GameObject selection = Instantiate(_cardPrefab, _displayParent);
            CardImage cardImage = selection.GetComponent<CardImage>();
            _cardImageComponents.Add(cardImage);
            cardImage.Initialize(spritesToDisplay[i], i);
            cardImage.OnButtonPressed += receiver.ButtonClicked;
            cardImage.OnButtonPressed += ButtonClickedSecond;

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

    private void ButtonClickedFirst(int index)
    {
        Debug.Log($"Button {index} clicked in first card selection displayer");
        for (int i = 0; i < _cardImageComponents.Count; i++)
        {
            _cardImageComponents[i].OnButtonPressed -= _currentReceiver.ButtonClicked;
            _cardImageComponents[i].OnButtonPressed -= ButtonClickedFirst;
        }

        _uiManager.DisplayCardSelectionUI(false); 
    }

    private void ButtonClickedSecond(int index)
    {
        Debug.Log($"Button {index} clicked in second card selection displayer");
        for (int i = 0; i < _cardImageComponents.Count; i++)
        {
            _cardImageComponents[i].OnButtonPressed -= _currentReceiver.ButtonClicked;
            _cardImageComponents[i].OnButtonPressed -= ButtonClickedSecond;
        }

        _uiManager.DisplayCardSelectionUI(false);
    }

    #endregion

}
