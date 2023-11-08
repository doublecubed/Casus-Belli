using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

public class PlayerStateVariables : MonoBehaviour
{
    #region STATES
    public int CannotAffectDeck {  get; private set; }



    #endregion

    #region REFERENCES

    [SerializeField] private EndState _endState;

    #endregion

    #region VARIABLES

    private Dictionary<PlayerStateVariable, PropertyInfo> _propertyDictionary;
    private Dictionary<PlayerStateVariable, int> _previousValues;


    #endregion

    #region MONOBEHAVIOUR

    private void OnEnable()
    {
        CompileStatesAndGenerateDictionary();
        _endState.OnTurnEnded += TurnEnded;
    }

    #endregion

    #region METHODS

    private void CompileStatesAndGenerateDictionary()
    {
        _propertyDictionary = new Dictionary<PlayerStateVariable, PropertyInfo>();
        _previousValues = new Dictionary<PlayerStateVariable, int>();

        PropertyInfo[] properties = typeof(PlayerStateVariables).GetProperties();
        
        foreach(PropertyInfo property in properties)
        {
            string propertyName = property.Name;
            
            if (Enum.TryParse(propertyName, out PlayerStateVariable variable))
            {
                _propertyDictionary[variable] = property;
                _previousValues[variable] = 0;
            } else
            {
                Debug.LogWarning("Property name does not match an enum value");
            }
        }

    }

    public void IncrementState(PlayerStateVariable state, int amount)
    {
        PropertyInfo property = _propertyDictionary[state];
        int currentPropertyValue = (int)property.GetValue(this);

        currentPropertyValue += amount;
        property.SetValue(this, currentPropertyValue);

    }

    public bool StateActive(PlayerStateVariable state)
    {
        return !(_previousValues[state] == 0 && (int)_propertyDictionary[state].GetValue(this) != 0);
    }

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
}

public enum PlayerStateVariable
{
    CannotAffectDeck
}
