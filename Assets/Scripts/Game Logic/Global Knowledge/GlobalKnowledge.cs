using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GlobalKnowledge : MonoBehaviour
{
    public static GlobalKnowledge Instance;

    public AbilityPlayPhase AbilityPhase;
    public PlayerInput PlayerInput;

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
    [SerializeField] GameObject _redPlayer;
    [SerializeField] GameObject _greenPlayer;

    [SerializeField] PlayerBehaviour _greenBehaviour;
    [SerializeField] PlayerBehaviour _redBehaviour;

    [SerializeField] PlayerStateVariables _greenStates;
    [SerializeField] PlayerStateVariables _redStates;

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
        }

        CreateDictionaries();
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
        return _movers[faction];
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
}
