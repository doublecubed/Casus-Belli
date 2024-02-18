using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalKnowledge : MonoBehaviour
{
    public static GlobalKnowledge Instance;


    public AbilityPlayPhase AbilityPhase;
    public EndState EndState;

    public PlayerInput PlayerInput;

    private CardMover _defaultMover;

    [Header("Draw Decks")]
    [SerializeField] private Deck _redArmyDeck;
    [SerializeField] private Deck _greenArmyDeck;

    [SerializeField] private Deck _redSupportDeck;
    [SerializeField] private Deck _greenSupportDeck;

    [Header("Trash Decks")]
    [SerializeField] private Deck _redArmyTrash;
    [SerializeField] private Deck _greenArmyTrash;

    [SerializeField] private Deck _redSupportTrash;
    [SerializeField] private Deck _greenSupportTrash;

    [Header("Hands")]
    [SerializeField] private Hand _redHand;
    [SerializeField] private Hand _greenHand;

    [Header("Play Areas")]
    [SerializeField] private PlayArea _redPlayArea;
    [SerializeField] private PlayArea _greenPlayArea;

    [Header("Players")]
    [SerializeField] private GameObject _redPlayer;
    [SerializeField] private GameObject _greenPlayer;

    [SerializeField] private PlayerBehaviour _greenBehaviour;
    [SerializeField] private PlayerBehaviour _redBehaviour;

    [SerializeField] private PlayerStateVariables _greenStates;
    [SerializeField] private PlayerStateVariables _redStates;

    [Header("Factions")]
    [SerializeField] private Affiliation _humanFaction;
    [SerializeField] private Affiliation _computerFaction;

    [Space(20)]
    [Header("Colors & Icons")]

    public Color RedFactionColor;
    public Color GreenFactionColor;

    [Space(5)]
    public Sprite SlowSprite;
    public Sprite FastSprite;
    public Sprite VeryFastSprite;

    [Space(5)]
    public Sprite ArmyCardSprite;
    public Sprite SupportCardSprite;

    


    private Dictionary<Affiliation, Vector3> _lookDirection;

    private Dictionary<Affiliation, Deck> _armyDecks;
    private Dictionary<Affiliation, Deck> _supportDecks;
    private Dictionary<Affiliation, Deck> _armyTrashes;
    private Dictionary<Affiliation, Deck> _supportTrashes;
    private Dictionary<Affiliation, Hand> _hands;
    private Dictionary<Affiliation, PlayArea> _playAreas;

    private Dictionary<Affiliation, CardMover> _movers;
    private Dictionary<Affiliation, PlayerBehaviour> _behaviours;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
            Debug.Log("GlobalKnowledge instance created");
        }

        CreateDictionaries();

        _defaultMover = GetComponent<CardMover>();
    }

    private void CreateDictionaries()
    {
        _armyDecks = new Dictionary<Affiliation, Deck>();
        _armyDecks.Add(Affiliation.Red, _redArmyDeck);
        _armyDecks.Add(Affiliation.Green, _greenArmyDeck);

        _supportDecks = new Dictionary<Affiliation, Deck>();
        _supportDecks.Add(Affiliation.Red, _redSupportDeck);
        _supportDecks.Add(Affiliation.Green, _greenSupportDeck);

        _armyTrashes = new Dictionary<Affiliation, Deck>();
        _armyTrashes.Add(Affiliation.Red, _redArmyTrash);
        _armyTrashes.Add(Affiliation.Green, _greenArmyTrash);

        _supportTrashes = new Dictionary<Affiliation, Deck>();
        _supportTrashes.Add(Affiliation.Red, _redSupportTrash);
        _supportTrashes.Add(Affiliation.Green, _greenSupportTrash);

        _hands = new Dictionary<Affiliation, Hand>();
        _hands.Add(Affiliation.Red, _redHand);
        _hands.Add(Affiliation.Green, _greenHand);

        _playAreas = new Dictionary<Affiliation, PlayArea>();
        _playAreas.Add(Affiliation.Red, _redPlayArea);
        _playAreas.Add(Affiliation.Green, _greenPlayArea);

        _movers = new Dictionary<Affiliation, CardMover>();
        _movers.Add(Affiliation.Red, _redPlayer.GetComponent<CardMover>());
        _movers.Add(Affiliation.Green, _greenPlayer.GetComponent<CardMover>());

        _lookDirection = new Dictionary<Affiliation, Vector3>();
        _lookDirection.Add(Affiliation.Red, Vector3.back);
        _lookDirection.Add(Affiliation.Green, Vector3.forward);

        _behaviours = new Dictionary<Affiliation, PlayerBehaviour>();
        _behaviours.Add(Affiliation.Red, _redBehaviour);
        _behaviours.Add(Affiliation.Green, _greenBehaviour);
    }


    public Deck ArmyDeck(Affiliation faction)
    {
        return _armyDecks[faction];
    }

    public Deck SupportDeck(Affiliation faction)
    {
        return _supportDecks[faction];
    }

    public Deck ArmyTrash(Affiliation faction)
    {
        return _armyTrashes[faction];
    }

    public Deck SupportTrash(Affiliation faction)
    {
        return _supportTrashes[faction];
    }

    public Hand Hand(Affiliation faction)
    {
        return _hands[faction];
    }

    public PlayArea PlayArea(Affiliation faction)
    {
        return _playAreas[faction];
    }

    public CardMover Mover(Affiliation faction)
    {
        return _defaultMover;
        //return _movers[faction];
    }

    public CardMover Mover()
    {
        return _defaultMover;
    }

    public Vector3 LookDirection(Affiliation faction)
    {
        return _lookDirection[faction];
    }

    public PlayerBehaviour Behaviour(Affiliation faction)
    {
        return _behaviours[faction];
    }

    public bool HumanPlayer(PlayerBehaviour behaviour)
    {
        return behaviour.GetComponent<HumanPlayer>();
    }

    public Affiliation OpponentFaction(Affiliation faction)
    {
        return faction == Affiliation.Red ? Affiliation.Green : Affiliation.Red;
    }

    public PlayerStateVariables PlayerStates(Affiliation faction)
    {
        return faction == Affiliation.Red ? _redStates : _greenStates;
    }

    public bool AIPLayer(Affiliation faction)
    {
        return Behaviour(faction).TryGetComponent(out AIPlayer aiPlayer);
    }

    public Affiliation HumanFaction()
    {
        return _humanFaction;
    }

    public Affiliation ComputerFaction()
    {
        return _computerFaction;
    }
}
