using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KralDoubleCards : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;

    private PlayerStateVariables _selfStates;

    private Affiliation _faction;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        _selfStates = _knowledge.PlayerStates(_selfCard.Faction);

        base._abilityPhase.Add(SetUpDoubleCards);

        base.Initialize();
    }

    private void SetUpDoubleCards()
    {
        _selfStates.UpdateState(PlayerStateVariable.DrawTwiceCards, 2, true);
        base.AbilityCompleted();
    }

}
