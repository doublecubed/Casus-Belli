using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralDoubleCards : AbilityBase
{
    private Card _selfCard;
    private CardMover _mover;

    private PlayerStateVariables _selfStates;

    private Affiliation _faction;

    public override void Initialize(GlobalKnowledge knowledge)
    {
        _selfCard = GetComponentInParent<Card>();
        _mover = knowledge.Mover(_selfCard.Faction);

        _selfStates = knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpDoubleCards);

        base.Initialize(knowledge);
    }

    private void SetUpDoubleCards()
    {
        _selfStates.UpdateState(PlayerStateVariable.DrawTwiceCards, 2, true);
        base.AbilityCompleted();
    }

}
