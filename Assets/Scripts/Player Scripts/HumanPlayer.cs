using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour
{
    [SerializeField] private MonoBehaviourStateMachine _stateMachine;

    private PlayerBehaviour _playerBehaviour;
    private PlayerKnowledge _playerKnowledge;
    private PlayerStats _playerStats;
    private CardPicker _cardPicker;

    public bool DoneDrawing {  get; private set; }
    public bool DonePlaying { get; private set; }

    private void Awake()
    {
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _playerKnowledge = GetComponent<PlayerKnowledge>();
        _playerStats = GetComponent<PlayerStats>();
        _cardPicker = GetComponent<CardPicker>();
    }

    public void ClickedOnCard(Card clickedCard)
    {

        if (clickedCard.transform.parent = _playerKnowledge.HandSelf.transform)
        {
            _playerBehaviour.PutFromHandToPlay(clickedCard);
        }

        UIManager.Instance.DisplayDoneButton(true);
    }


    public void PlayerIsDonePlaying()
    {
        DonePlaying = true;
    }

    public void ClickedOnDeck(Deck clickedDeck)
    {

        if (clickedDeck == _playerKnowledge.ArmyDeckSelf || clickedDeck == _playerKnowledge.SupportDeckSelf)
        {
            if (_playerKnowledge.HandSelf.CardsInHand.Count < _playerStats.MaxCardsPerRound)
            {
                _playerBehaviour.DrawFromDeckToHand(clickedDeck);
            }
        }

    }

    public void SendUnPlayedCardsToDecks()
    {
        int numberOfCards = _playerKnowledge.HandSelf.CardsInHand.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            _playerBehaviour.PutBackAtDeckBottom(_playerKnowledge.HandSelf.CardsInHand[0]);
        }
    }

    public void ResetPlayedFlag()
    {
        DonePlaying = false;
    }

}
