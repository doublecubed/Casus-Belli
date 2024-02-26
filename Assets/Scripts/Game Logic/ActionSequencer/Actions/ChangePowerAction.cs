using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using TMPro;
using UnityEngine;

public class ChangePowerAction : GameAction
{
    private Card _card;
    private int _newPower;
    private TextMeshPro _powerText;

    public ChangePowerAction(Card card, int newPower)
    {
        _card = card;
        _newPower = newPower;
        _powerText = _card.GetComponent<CardDisplayer>().PowerText;
    }


    public override async UniTask ExecuteAction()
    {
        _powerText.text = _newPower.ToString();
    }
}
