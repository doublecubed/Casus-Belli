using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

public class PlayerStateVariables : MonoBehaviour, IButtonClickReceiver
{
    #region STATES
    public int CannotAffectDeck {  get; private set; }
    public int SetArmiesToOne { get; private set; }
    public int DrawTwiceCards { get; private set; }
    public int PickACardFromHand { get; private set; }
    public int CantPlaySupportCards { get; private set; }
    public int TakeSupportFromTrash { get; private set; }
    public int PlayHandOpen { get; private set; }
    public int TakeKingInHand { get; private set; }
    public int TakePrinceInHand { get; private set; }
    public int ReturnPlayedSupportsToDeck { get; private set; }
    public int ReturnSupportsBuyucu {  get; private set; }

    #endregion

    #region REFERENCES

    [SerializeField] private EndState _endState;
    private GlobalKnowledge _knowledge;
    private Affiliation _opponentFaction;
    [SerializeField] private CardSelectionDisplayer _cardSelectionDisplayer;
    private CardMover _mover;

    private Hand _selfHand;
    private Hand _opponentHand;
    private PlayArea _selfPlayArea;

    private Deck _selfSupportTrash;
    private Deck _selfSupportDeck;

    private PlayerStateVariable _currentAbility;

    // Take card in hand
    private List<Card> _cardSelectionList;
    private Card _selectedCard;

    private List<Card> _saklabanCardList;

    private bool _supportCardNegated;

    public Card TargetPrince;
    public Card TargetKing;


    #endregion

    #region VARIABLES



    private Dictionary<PlayerStateVariable, PropertyInfo> _propertyDictionary;

    public int CardsToDraw { get; private set; }

    [field: SerializeField] public bool AIPlayer { get; private set; }

    [field: SerializeField] public Affiliation Faction { get; private set; }
    [field: SerializeField] public int DefaultCardsToDraw { get; private set; }
    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        CompileStatesAndGenerateDictionary();
        _endState.OnTurnEnded += TurnEnded;

        AIPlayer = GetComponent<AIPlayer>();
    }

    private void OnEnable()
    {
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(Faction);
        _opponentFaction = _knowledge.OpponentFaction(Faction);
        _selfHand = _knowledge.Hand(Faction);
        _opponentHand = _knowledge.Hand(_opponentFaction);
        _selfSupportTrash = _knowledge.SupportTrash(Faction);
        _selfPlayArea = _knowledge.PlayArea(Faction);
        _selfSupportDeck = _knowledge.SupportDeck(Faction);

        CardsToDraw = DefaultCardsToDraw;
    }

    #endregion

    #region GENERAL METHODS

    #region Initialization

    private void CompileStatesAndGenerateDictionary()
    {
        _propertyDictionary = new Dictionary<PlayerStateVariable, PropertyInfo>();

        _propertyDictionary[PlayerStateVariable.CannotAffectDeck] = typeof(PlayerStateVariables).GetProperty("CannotAffectDeck");
        _propertyDictionary[PlayerStateVariable.SetArmiesToOne] = typeof(PlayerStateVariables).GetProperty("SetArmiesToOne");
        _propertyDictionary[PlayerStateVariable.DrawTwiceCards] = typeof(PlayerStateVariables).GetProperty("DrawTwiceCards");
        _propertyDictionary[PlayerStateVariable.PickACardFromHand] = typeof(PlayerStateVariables).GetProperty("PickACardFromHand");
        _propertyDictionary[PlayerStateVariable.CantPlaySupportCards] = typeof(PlayerStateVariables).GetProperty("CantPlaySupportCards");
        _propertyDictionary[PlayerStateVariable.TakeSupportFromTrash] = typeof(PlayerStateVariables).GetProperty("TakeSupportFromTrash");
        _propertyDictionary[PlayerStateVariable.PlayHandOpen] = typeof(PlayerStateVariables).GetProperty("PlayHandOpen");
        _propertyDictionary[PlayerStateVariable.TakeKingInHand] = typeof(PlayerStateVariables).GetProperty("TakeKingInHand");
        _propertyDictionary[PlayerStateVariable.TakePrinceInHand] = typeof(PlayerStateVariables).GetProperty("TakePrinceInHand");
        _propertyDictionary[PlayerStateVariable.ReturnPlayedSupportsToDeck] = typeof(PlayerStateVariables).GetProperty("ReturnPlayedSupportsToDeck");
        _propertyDictionary[PlayerStateVariable.ReturnSupportsBuyucu] = typeof(PlayerStateVariables).GetProperty("ReturnSupportsBuyucu");
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
        int currentValue = (int)_propertyDictionary[state].GetValue(this);

        return currentValue > 0;
    }

    public void DisableState(PlayerStateVariable state)
    {
        _propertyDictionary[state].SetValue(this, 0);
    }

    #endregion

    #region Turn End

    private void TurnEnded()
    {
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


    #endregion

    #endregion

    #region STATE METHODS

    #region State Checks

    public void CheckState(GameStateBase state)
    {
        switch (state)
        {
            case StartState startstate when state is StartState:                
                break;
            case DrawState drawState when state is DrawState:
                CheckDrawStartStates();
                break;
            case PlayState playState when state is PlayState:
                CheckPlayStartStates();
                break;
            case ResolveState resolveState when state is ResolveState:
                CheckResolveStartStates();
                break;
            case EndState endState when state is EndState:
                break;
        }
    }

    public void CheckDrawStartStates()
    {
        CheckTakeKingAndPrince();
        CheckDrawTwiceCards();
        CheckTakeSupportFromTrash();
        CheckPlayHandOpen();
    }

    public void CheckPlayStartStates()
    {
        CheckPickACardFromHand();
    }

    public void CheckResolveStartStates()
    {
        CheckReturnPlayedSupportsToDeck();
        CheckSetArmiesToOne();
    }

    #endregion

    #region State-Specific Methods

    private void CheckDrawTwiceCards()
    {
        bool drawTwice = StateActive(PlayerStateVariable.DrawTwiceCards);

        CardsToDraw = DefaultCardsToDraw * (drawTwice ? 2 : 1);
    }

    private void CheckSetArmiesToOne()
    {
        if (!StateActive(PlayerStateVariable.SetArmiesToOne)) return;

        List<Card> armyCardsInPlay = _knowledge.PlayArea(Faction).CardsInPlay.Where(x => x.CardType == CardType.Army).ToList();

        foreach (Card card in armyCardsInPlay)
        {
            card.SetPower(1);
        }
    }

    private void CheckPickACardFromHand()
    {
        if (!StateActive(PlayerStateVariable.PickACardFromHand)) return;

        _currentAbility = PlayerStateVariable.PickACardFromHand;
        _cardSelectionList = _opponentHand.CardsInHand;

        if (AIPlayer)
        {
            ButtonClicked(0);
        } else
        {
            _cardSelectionDisplayer.DisplaySelection(_cardSelectionList, this);
        }
    }

    private void CheckPlayHandOpen()
    {

    }

    private void CheckTakeSupportFromTrash()
    {
        if (TakeSupportFromTrash <= 0) return;

        _currentAbility = PlayerStateVariable.TakeSupportFromTrash;
        _cardSelectionList = _selfSupportTrash.LookAtCards();

        if (AIPlayer)
        {
            ButtonClicked(0);
        }
        else
        {
            _cardSelectionDisplayer.DisplaySelection(_cardSelectionList, this);
        }
    }

    private void CheckTakeKingAndPrince()
    {
        if (StateActive(PlayerStateVariable.TakePrinceInHand))
        {
            _mover.MoveCard(TargetPrince, _selfHand, _selfHand.PlacementPosition(), PlacementFacing.ToCamera, _knowledge.LookDirection(Faction));   
        } 

        if (StateActive(PlayerStateVariable.TakeKingInHand))
        {
            _mover.MoveCard(TargetKing, _selfHand, _selfHand.PlacementPosition(), PlacementFacing.ToCamera, _knowledge.LookDirection(Faction));
        }
    }

    private void CheckReturnPlayedSupportsToDeck()
    {
        if (ReturnPlayedSupportsToDeck <= 1) return;

        _saklabanCardList = _selfPlayArea.CardsInPlay.Where(x => x.CardType == CardType.Support).ToList();

        _endState.OnTurnEnded += ResolveReturnPlayedSupportsToDeck;
    }

    private void ResolvePickACardFromHand()
    {
        _mover.MoveCard(_selectedCard, _selfHand, _selfHand.PlacementPosition(), PlacementFacing.ToCamera, _knowledge.LookDirection(Faction));
    }

    private void ResolveTakeSupportFromTrash()
    {
        ResolvePickACardFromHand();
    }

    private void ResolveReturnPlayedSupportsToDeck()
    {
        _endState.OnTurnEnded -= ResolveReturnPlayedSupportsToDeck;
        
        foreach (Card card in _saklabanCardList)
        {
            _mover.MoveCard(card, _selfSupportDeck, _selfSupportDeck.transform.position, PlacementFacing.Down, DeckSide.Bottom, _knowledge.LookDirection(Faction));
        }
    }

    #endregion

    #region Non-Ability Methods

    public void ButtonClicked(int index)
    {
        _selectedCard = _cardSelectionList[index];
        ResolveAbility(_currentAbility);
    }

    private void ResolveAbility(PlayerStateVariable ability)
    {
        switch (ability)
        {
            case PlayerStateVariable.PickACardFromHand:
                ResolvePickACardFromHand();
                break;
            case PlayerStateVariable.TakeSupportFromTrash:
                ResolveTakeSupportFromTrash();
                break;
        }
    }

    #endregion

    #endregion
}

public enum PlayerStateVariable
{
    CannotAffectDeck,
    SetArmiesToOne,
    DrawTwiceCards,
    PickACardFromHand,
    CantPlaySupportCards,
    TakeSupportFromTrash,
    PlayHandOpen,
    TakeKingInHand,
    TakePrinceInHand,
    ReturnPlayedSupportsToDeck,
    ReturnSupportsBuyucu
}
