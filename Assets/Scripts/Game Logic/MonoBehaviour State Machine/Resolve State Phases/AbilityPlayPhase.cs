using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using UnityEngine;

public class AbilityPlayPhase : GameStateBase, IButtonClickReceiver
{
    #region REFERENCES

    [SerializeField] private PlayArea _playerArea;
    [SerializeField] private PlayArea _opponentArea;

    private PlayerStateVariables _greenStates;
    private PlayerStateVariables _redStates;

    #endregion

    #region VARIABLES

    [SerializeField] private List<Card> _cardsInPlay;
    [SerializeField] private List<Card> _cardResolveOrder;

    private int _cardsToPlay;
    private int _cardResolveIndex;

    [SerializeField] private Card _cardInEffect;

    #endregion

    #region MONOBEHAVIOUR

    protected override void Start()
    {
        base.Start();
        _greenStates = base._knowledge.PlayerStates(Affiliation.Green);
        _redStates = base._knowledge.PlayerStates(Affiliation.Red);
    }

    protected async override void OnEnable()
    {
        base.OnEnable();

        SortCardOrder();
        await StartCardResolution();
        //ResolveNextCard();

        //EditorApplication.hierarchyChanged += CardsInPlayUpdated;
    }

    protected override void Update()
    {
        base.Update();
    }

    private void OnDisable()
    {
        //EditorApplication.hierarchyChanged -= CardsInPlayUpdated;
    }


    #endregion


    #region METHODS

    public void ButtonClicked(int index)
    {

    }


    private void SortCardOrder()
    {
        GetCardsInPlay();
        SortCardsInPlay();

        _cardsToPlay = _cardResolveOrder.Count;
        _cardResolveIndex = 0;
    }

    private async UniTask StartCardResolution()
    {
        int numberOfCards = _cardResolveOrder.Count;



        var ct = this.GetCancellationTokenOnDestroy();

        while (_cardResolveOrder.Count > 0)
        {
            await ResolveCard(_cardResolveOrder[0], ct);
            _cardResolveOrder.RemoveAt(0);
        }

        base._isDone = true;
    }

    private async UniTask ResolveCard(Card card, CancellationToken cancellationToken)
    {
        Debug.Log(card.CardName);
        Debug.Log("Starting ability use for " + card.CardName);
        card.StartAbilityUse();
    }

    private void ResolveNextCard()
    {
        if (_cardResolveIndex >= _cardResolveOrder.Count)
        {
            base._isDone = true;
            return;
        }

        _cardInEffect = _cardResolveOrder[_cardResolveIndex];
        _cardInEffect.OnCardResolutionCompleted += CardResolved;

        Debug.Log("Starting ability use for " + _cardInEffect.CardName);
        _cardInEffect.StartAbilityUse();
    }

    private void CardResolved()
    {
        _cardInEffect.OnCardResolutionCompleted -= CardResolved;
        _cardResolveIndex++;
        ResolveNextCard();
    }

    private void GetCardsInPlay()
    {
        _cardsInPlay = new List<Card>();
        _cardsInPlay.AddRange(_opponentArea.CardsInPlay);
        _cardsInPlay.AddRange(_playerArea.CardsInPlay);
    }

    private void SortCardsInPlay()
    {
        _cardResolveOrder = new List<Card>();

        List<Card> veryFastCards = _cardsInPlay.Where(x => x.Priority == CardPriority.VeryFast).ToList();
        _cardResolveOrder.AddRange(veryFastCards);


        List<Card> fastCards = _cardsInPlay.Where(x => x.Priority == CardPriority.Fast).ToList();
        _cardResolveOrder.AddRange(fastCards);


        List<Card> slowCards = _cardsInPlay.Where(x => x.Priority == CardPriority.Slow).ToList();
        _cardResolveOrder.AddRange(slowCards);


        List<Card> verySlowSupportCards = _cardsInPlay.Where(x => x.Priority == CardPriority.VerySlow && x.CardType == CardType.Support).ToList();
        _cardResolveOrder.AddRange(verySlowSupportCards);


        List<Card> verySlowArmyCards = _cardsInPlay.Where(x => x.Priority == CardPriority.VerySlow && x.CardType == CardType.Army).ToList();
        verySlowArmyCards = verySlowArmyCards.OrderBy(x => x.Power).ToList();
        _cardResolveOrder.AddRange(verySlowArmyCards);

    }

    private void CardsInPlayUpdated()
    {
        GetCardsInPlay();
        SortCardsInPlay();
    }

    public void RemoveCardFromStack(Card card)
    {
        if (_cardResolveOrder.Contains(card))
        {
            int cardIndex = _cardResolveOrder.IndexOf(card);
            _cardResolveOrder.RemoveAt(cardIndex);
            if (cardIndex <= _cardResolveIndex) _cardResolveIndex--;
        }
    }

    public void AddCardToStack(Card card)
    {
        _cardResolveOrder.Add(card);
    }

    #endregion
}
