using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrensesTakeKingAndPrince : AbilityBase
{
    private Card _selfCard;
    private GlobalKnowledge _knowledge;

    public override void Initialize()
    {
        _selfCard = GetComponentInParent<Card>();
        _knowledge = GlobalKnowledge.Instance;

        base.Initialize();
    }

}
