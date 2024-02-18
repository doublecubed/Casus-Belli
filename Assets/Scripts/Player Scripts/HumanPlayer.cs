using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HumanPlayer : MonoBehaviour
{
    [SerializeField] private MonoBehaviourStateMachine _stateMachine;

    private PlayerStateVariables _playerVariables;
    private PlayerBehaviour _playerBehaviour;
    [SerializeField] private GlobalKnowledge _globalKnowledge;
    private CardPicker _cardPicker;

    public bool DoneDrawing {  get; private set; }
    public bool DonePlaying { get; private set; }

    private void Awake()
    {
        _playerBehaviour = GetComponent<PlayerBehaviour>();
        _playerVariables = GetComponent<PlayerStateVariables>();
        _cardPicker = GetComponent<CardPicker>();
    }

    public void ClickedOnCard(Card clickedCard)
    {

        if (clickedCard.transform.parent = _globalKnowledge.Hand(_globalKnowledge.HumanFaction()).transform)
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

        if (clickedDeck == _globalKnowledge.ArmyDeck(_globalKnowledge.HumanFaction()) || clickedDeck == _globalKnowledge.SupportDeck(_globalKnowledge.HumanFaction()))
        {
            if (_globalKnowledge.Hand(_globalKnowledge.HumanFaction()).CardsInHand.Count < _playerVariables.CardsToDraw)
            {
                _playerBehaviour.DrawFromDeckToHand(clickedDeck);
            }
        }

    }

    public void SendUnPlayedCardsToDecks()
    {
        int numberOfCards = _globalKnowledge.Hand(_globalKnowledge.HumanFaction()).CardsInHand.Count;

        for (int i = 0; i < numberOfCards; i++)
        {
            _playerBehaviour.PutBackAtDeckBottom(_globalKnowledge.Hand(_globalKnowledge.HumanFaction()).CardsInHand[0]);
        }
    }

    public void ResetPlayedFlag()
    {
        DonePlaying = false;
    }

}
