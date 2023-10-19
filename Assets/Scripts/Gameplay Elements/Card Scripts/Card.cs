using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Card : MonoBehaviour, IButtonClickReceiver
{
    #region REFERENCES

    private CardSO _cardObject;

    #endregion

    #region VARIABLES

    #region Card Properties

    [field: SerializeField] public Sprite CardImage { get; private set; }
    [field: SerializeField] public string CardName { get; private set; }
    [field: SerializeField] public CardType CardType { get; private set; }
    [field: SerializeField] public Affiliation Faction { get; private set; }
    [field: SerializeField] public CardPriority Priority { get; private set; }
    [field: SerializeField] public int Power { get; private set; }
    [field: SerializeField] public GameObject[] Abilities { get; private set; }

    [field: SerializeField] public Deck StartingDeck { get; private set; }

    private AbilityBase[] _abilityScripts;

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
        Abilities = _cardObject.abilities;
        StartingDeck = transform.parent.GetComponentInParent<Deck>();

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
        if (Abilities.Length == 0) OnCardResolutionCompleted?.Invoke();

        if (Abilities.Length == 1) _abilityScripts[0].UseAbility();

        if (Abilities.Length >= 2) 
        {
            List<Card> cardList = new List<Card>();
            
            for (int i = 0; i < Abilities.Length; i++)
            {
                cardList.Add(this);
            }

            UIManager.Instance.DisplaySelectionCards(cardList, this);
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

    #endregion

    #region Internal

    private void AbilityResolved()
    {
        OnCardResolutionCompleted?.Invoke();
    }

    public void ButtonClicked(int index)
    {
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