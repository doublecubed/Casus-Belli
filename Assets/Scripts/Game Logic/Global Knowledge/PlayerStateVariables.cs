using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;

public class PlayerStateVariables : MonoBehaviour
{
    #region STATES
    public int CannotAffectDeck {  get; private set; }
    public int SetArmiesToOne { get; private set; }
    public int DrawTwiceCards { get; private set; }

    #endregion

    #region REFERENCES

    [SerializeField] private EndState _endState;
    private GlobalKnowledge _globalKnowledge;

    #endregion

    #region VARIABLES



    private Dictionary<PlayerStateVariable, PropertyInfo> _propertyDictionary;
    private Dictionary<PlayerStateVariable, int> _previousValues;

    [field: SerializeField] public Affiliation Faction { get; private set; }
    [field: SerializeField] public int MaxCardsToDraw { get; private set; }

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        CompileStatesAndGenerateDictionary();
        _endState.OnTurnEnded += TurnEnded;
    }

    private void OnEnable()
    {
        _globalKnowledge = GlobalKnowledge.Instance;
    }

    #endregion

    #region GENERAL METHODS

    #region Initialization

    private void CompileStatesAndGenerateDictionary()
    {
        _propertyDictionary = new Dictionary<PlayerStateVariable, PropertyInfo>();
        _previousValues = new Dictionary<PlayerStateVariable, int>();

        _propertyDictionary[PlayerStateVariable.CannotAffectDeck] = typeof(PlayerStateVariables).GetProperty("CannotAffectDeck");
        _propertyDictionary[PlayerStateVariable.SetArmiesToOne] = typeof(PlayerStateVariables).GetProperty("SetArmiesToOne");
        _propertyDictionary[PlayerStateVariable.DrawTwiceCards] = typeof(PlayerStateVariables).GetProperty("DrawTwiceCards");
    }

    #endregion

    #region State Manipulation

    public void UpdateState(PlayerStateVariable state, int amount, bool stackable = false)
    {
        PropertyInfo property = _propertyDictionary[state];
        int currentPropertyValue = (int)property.GetValue(this);

        currentPropertyValue = !stackable ? amount : currentPropertyValue + amount;

        property.SetValue(this, currentPropertyValue);

    }

    public bool StateActive(PlayerStateVariable state)
    {
        int previousValue = _previousValues[state];
        int currentValue = (int)_propertyDictionary[state].GetValue(this);

        return !(previousValue == 0 && currentValue != 0) || currentValue == 0;
    }

    public void DisableState(PlayerStateVariable state)
    {
        _propertyDictionary[state].SetValue(this, 0);
    }

    #endregion

    #region Turn End

    private void TurnEnded()
    {
        UpdatePreviousValues();
        DecrementStateValues();
    }

    private void DecrementStateValues()
    {
        foreach (KeyValuePair<PlayerStateVariable, PropertyInfo> variable in _propertyDictionary)
        {
            int propertyValue = (int)_propertyDictionary[variable.Key].GetValue(this);
            propertyValue--;
            propertyValue = Mathf.Max(propertyValue, 0);
            _propertyDictionary[variable.Key].SetValue(this, propertyValue);
        }
    }

    private void UpdatePreviousValues()
    {
        foreach (KeyValuePair<PlayerStateVariable, PropertyInfo> variable in _propertyDictionary) 
        {
            _previousValues[variable.Key] = (int)_propertyDictionary[variable.Key].GetValue(this);
        }
    }

    #endregion

    #endregion

    #region STATE METHODS

    public void CheckTurnStartStates()
    {
        CheckDrawTwiceCards();
    }

    public void CheckResolveStartStates()
    {
        CheckSetArmiesToOne();
    }

    private void CheckDrawTwiceCards()
    {

    }

    private void CheckSetArmiesToOne()
    {
        if (!StateActive(PlayerStateVariable.SetArmiesToOne)) return;

        List<Card> armyCardsInPlay = _globalKnowledge.PlayArea(Faction).CardsInPlay.Where(x => x.CardType == CardType.Army).ToList();

        foreach (Card card in armyCardsInPlay)
        {
            card.SetPower(1);
        }
    }

    


    #endregion
}

public enum PlayerStateVariable
{
    CannotAffectDeck,
    SetArmiesToOne,
    DrawTwiceCards
}
