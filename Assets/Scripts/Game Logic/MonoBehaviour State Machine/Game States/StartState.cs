using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartState : GameStateBase
{
    [SerializeField] private Deck _playerArmyDeck;
    [SerializeField] private Deck _playerSupportDeck;
    [SerializeField] private Deck _opponentArmyDeck;
    [SerializeField] private Deck _opponentSupportDeck;


    protected override void OnEnable()
    {
        base.OnEnable();

        

        _playerArmyDeck.DealDeck();
        _playerArmyDeck.Shuffle();
        _playerSupportDeck.DealDeck();
        _playerSupportDeck.Shuffle();

        _opponentArmyDeck.DealDeck();
        _opponentArmyDeck.Shuffle();
        _opponentSupportDeck.DealDeck();
        _opponentSupportDeck.Shuffle();

        base._isDone = true;
    }

    
}
