using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.Linq;

public class CardDisplayer : MonoBehaviour
{
    private CardSO _cardObject;
    
    [SerializeField] private SpriteRenderer _backFace;
    [SerializeField] private SpriteRenderer _cardImage;
    [SerializeField] private SpriteRenderer _cardType;
    [SerializeField] private SpriteRenderer _cardPriority;
    [SerializeField] private SpriteRenderer _firstAbility;
    [SerializeField] private SpriteRenderer _secondAbility;
    [SerializeField] private SpriteRenderer _border;
    [SerializeField] private TextMeshPro _powerText;

    [SerializeField] private GameObject _priorityObject;
    [SerializeField] private GameObject _firstAbilityObject;
    [SerializeField] private GameObject _secondAbilityObject;
    [SerializeField] private GameObject _powerObject;

    public bool IsVisible { get; private set; }

    public void Initialize(CardSO cardSO)
    {
        _cardObject = cardSO;

        _backFace.sprite = cardSO.backSprite;

        _cardImage.sprite = cardSO.cardImage;

        _border.color = cardSO.faction == Affiliation.Red ? GlobalKnowledge.Instance.RedFactionColor : GlobalKnowledge.Instance.GreenFactionColor;

        _cardType.sprite = cardSO.cardType == CardType.Army ? GlobalKnowledge.Instance.ArmyCardSprite : GlobalKnowledge.Instance.SupportCardSprite;
        
        if (cardSO.priority == CardPriority.VeryFast)
        {
            _cardPriority.sprite = GlobalKnowledge.Instance.VeryFastSprite;
        } else if (cardSO.priority == CardPriority.Fast)
        {
            _cardPriority.sprite = GlobalKnowledge.Instance.FastSprite;
        } else if (cardSO.priority == CardPriority.Slow)
        {
            _cardPriority.sprite = GlobalKnowledge.Instance.SlowSprite;
        } else
        {
            _priorityObject.SetActive(false);
        }

        switch (cardSO.abilities.Length)        
        {
            case 0:
                _firstAbilityObject.SetActive(false);
                _secondAbilityObject.SetActive(false);
                break;
            case 1:
                _firstAbility.sprite = cardSO.abilities[0].GetComponent<AbilityBase>().AbilityImage;
                _firstAbilityObject.SetActive(true);
                _secondAbilityObject.SetActive(false);
                break;
            case 2:
                _firstAbility.sprite = cardSO.abilities[0].GetComponent<AbilityBase>().AbilityImage;
                _secondAbility.sprite = cardSO.abilities[1].GetComponent<AbilityBase>().AbilityImage;
                _firstAbilityObject.SetActive(true);
                _secondAbilityObject.SetActive(true);
                break;
            default:
                _firstAbilityObject.SetActive(false);
                _secondAbilityObject.SetActive(false);
                break;
        }

        if (cardSO.power != 0)
        {
            _powerObject.SetActive(true);
            _powerText.text = cardSO.power.ToString();
        } else
        {
            _powerObject.SetActive(false);
        }

    }

}
