using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "New Card", menuName = "Casus Belli/Card")]
public class CardSO : ScriptableObject
{
    public string cardName;
    public CardType cardType;
    public Affiliation faction;

    public Texture faceImage;
    public Texture backImage;

    public CardPriority priority;
    public int power;

    public GameObject[] abilities;
 
}
