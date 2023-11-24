using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour, IButtonClickReceiver
{
    #region REFERENCES

    private CardSO _cardObject;

    private GlobalKnowledge _knowledge;
    private EndState _endState;
    private PlayerStateVariables _selfStates;
    private PlayerBehaviour _selfBehaviour;

    #endregion

    #region VARIABLES

    #region Card Properties

    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeField] public string CardName { get; private set; }
    [field: SerializeField] public CardType CardType { get; private set; }
    [field: SerializeField] public Affiliation Faction { get; set; }
    [field: SerializeField] public CardPriority Priority { get; private set; }
    [field: SerializeField] public int Power { get; private set; }
    [field: SerializeField] public bool AffectsDeck { get; private set; }
    [field: SerializeField] public GameObject[] Abilities { get; private set; }
    

    [field: SerializeField] public Deck StartingDeck { get; private set; }



    private AbilityBase[] _abilityScripts;

    private int _defaultPower;

    #endregion

    #endregion

    #region EVENTS

    public event Action OnCardResolutionCompleted;

    #endregion

    #region METHODS

    #region Initialization

    public void Initialize(CardSO cardSO)
    {
        _cardObject = cardSO;

        CardImage = cardSO.faceSprite;
        CardName = _cardObject.cardName;
        CardType = _cardObject.cardType;
        Faction = _cardObject.faction;
        Priority = _cardObject.priority;
        Power = _cardObject.power;
        _defaultPower = _cardObject.power;
        Abilities = _cardObject.abilities;
        StartingDeck = transform.parent.GetComponentInParent<Deck>();
        AffectsDeck = _cardObject.affectsDeck;

        _knowledge = GlobalKnowledge.Instance;
        _endState = _knowledge.EndState;
        _endState.OnTurnEnded += ResetPower;
        _selfStates = _knowledge.PlayerStates(Faction);
        _selfBehaviour = _knowledge.Behaviour(Faction);

        _abilityScripts = new AbilityBase[Abilities.Length];

        for (int i = 0; i < Abilities.Length; i++)
        {
            GameObject abilityObject = Instantiate(Abilities[i], transform);
            _abilityScripts[i] = abilityObject.GetComponent<AbilityBase>();
            _abilityScripts[i].Initialize();

            _abilityScripts[i].OnAbilityExecutionCompleted += AbilityResolved;
        }
    }


    #endregion

    #region Ability Usage

    public void StartAbilityUse()
    {
        if (AffectsDeck && _selfStates.StateActive(PlayerStateVariable.CannotAffectDeck))
        {
            _selfStates.UpdateState(PlayerStateVariable.CannotAffectDeck, 0);
            OnCardResolutionCompleted?.Invoke();
            return;
        }

        if (Abilities.Length == 0) OnCardResolutionCompleted?.Invoke();

        if (Abilities.Length == 1) _abilityScripts[0].UseAbility();

        if (Abilities.Length >= 2) 
        {
            Debug.Log("The ability selection runs");
            _selfBehaviour.SelectAbility(this, _abilityScripts);
        }
    }



    #endregion

    #region Ability Manipulation

    public void BlockAbility(int abilityNo)
    {

    }

    public void BlockAbilities()
    {

    }

    #endregion

    #region Stat Manipulation

    public void SetPower(int power)
    {
        Power = power;
    }

    private void ResetPower()
    {
        Power = _defaultPower;
    }

    #endregion

    #region Internal

    private void AbilityResolved()
    {
        OnCardResolutionCompleted?.Invoke();
    }

    public void ButtonClicked(int index)
    {
        _abilityScripts[index].UseAbility();

        Debug.Log($"Button no {index} is clicked");
    }

    #endregion

    #endregion
}

public enum Affiliation
{
    Red,
    Green
}

public enum CardType
{
    Army,
    Support
}

public enum CardPriority
{
    VerySlow = 0,
    Normal = 10,
    Slow = 20,
    Fast = 30,
    VeryFast = 40
}