using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;
using System.Threading;
using UnityEditor;

public static class CardActions
{
    private const float riseDuration = 0.5f;
    private const float riseDistance = 1f;

    private const float highlightDuration = 1f;
    private const float highlightScale = 1.5f;

    private const float cancelAbilityScale = 1.5f;
    private const float cancelAbilityDuration = 0.5f;



    public static async UniTask RiseCard(Card card, CancellationToken cancellationToken, ActionSequencer sequencer)
    {
        RiseAction action = new RiseAction(card, riseDistance, riseDuration, true, cancellationToken);
        await sequencer.InsertAction(action);
    }

    public static async UniTask LowerCard(Card card, CancellationToken ct, ActionSequencer sequencer)
    {
        RiseAction action = new RiseAction(card, riseDistance, riseDuration, false, ct);
        await sequencer.InsertAction(action);
    }


    public static async UniTask FlipCard(Card card, CancellationToken ct, ActionSequencer sequencer)
    {

    }

    public static async UniTask HighlightAbility(Card card, Transform targetTransform, CancellationToken ct, ActionSequencer sequencer)
    {
        HighlightAction action = new HighlightAction(card, targetTransform, highlightScale, highlightDuration, ct);
        await sequencer.InsertAction(action);
    }

    public static async UniTask CancelAbilities(Card card, CancellationToken ct, ActionSequencer sequencer)
    {
        int numberOfAbilities = card.Abilities.Length;

        UniTask[] tasks = new UniTask[numberOfAbilities];

        for (int i = 0; i < numberOfAbilities; i++)
        {
            CancelAbilityAction action = new CancelAbilityAction(card, i, cancelAbilityScale, cancelAbilityDuration, ct);
            card.Abilities[i].GetComponent<AbilityBase>().CancelAbility();
            tasks[i] = sequencer.InsertAction(action);
        }

        await UniTask.WhenAll(tasks);
    }

    public static async UniTask ChangePower(Card card, int newPower, CancellationToken ct, ActionSequencer sequencer)
    {
        ChangePowerAction changePower = new ChangePowerAction(card, newPower);
        await sequencer.InsertAction(changePower);
    }
}
