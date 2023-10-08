using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerKnowledge : MonoBehaviour
{
    public Deck ArmyDeckSelf;
    public Deck SupportDeckSelf;
    public Deck ArmyTrashSelf;
    public Deck SupportTrashSelf;
    [Space(10)]
    public Deck ArmyDeckOpponent;
    public Deck SupportDeckOpponent;
    public Deck ArmyTrashOpponent;
    public Deck SupportTrashOpponent;
    [Space(10)]
    public PlayArea AreaSelf;
    public PlayArea AreaOpponent;
    [Space(10)]
    public Hand HandSelf;
    public Hand HandOpponent;

    [Space(10)]
    public Vector3 TableDirection;

}
