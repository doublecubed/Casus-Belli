using Cysharp.Threading.Tasks;
using System.Threading;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionSequencer : MonoBehaviour
{
    private GlobalKnowledge _knowledge;
    private CardMover _mover;

    private List<Card> _cardOrder;

    private Queue<GameAction> _actionsQueue;

    private CancellationToken ct;

    public bool isProcessing;

    private void Start()
    {
        _knowledge = GetComponent<GlobalKnowledge>();
        _mover = GetComponent<CardMover>();

        ct = this.GetCancellationTokenOnDestroy();
        _actionsQueue = new Queue<GameAction>();
    }

    public void GetCardOrder(List<Card> cards)
    {
        _cardOrder = cards;
    }

    public void InsertAction(GameAction action)
    {
        _actionsQueue.Enqueue(action);
        ProcessActions();
    }


    public async void ProcessActions()
    {
        if (isProcessing) return;

        isProcessing = true;

        while (_actionsQueue.Count > 0)
        {
            GameAction currentAction = _actionsQueue.Dequeue();
            await currentAction.ExecuteAction();
        }

        isProcessing = false;
    }

}
