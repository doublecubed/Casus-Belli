using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KraliceTakeCardInHand : AbilityBase, IButtonClickReceiver
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;
    private CardMover _mover;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;
        _mover = _knowledge.Mover(_selfCard.Faction);

        base.Initialize();
    }



    public void ButtonClicked(int index)
    {

    }
}
