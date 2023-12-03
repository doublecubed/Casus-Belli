using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CardImage : MonoBehaviour
{
    public Image ImageComponent;

    public Button ImageButton;
    public int ImageIndex;

    public GameObject TextCradle;
    public TextMeshProUGUI AbilityDescription;

    public event Action<int> OnButtonPressed;

    public void Initialize(Sprite image, int index)
    {
        ImageComponent.sprite = image;
        ImageIndex = index;
        TextCradle.SetActive(false);
    }

    public void Initialize(Sprite image, int index, string abilityDescription)
    {
        Initialize(image, index);
        AbilityDescription.text = abilityDescription;
        TextCradle.SetActive(true);
    }

    public void ButtonPressed()
    {
        OnButtonPressed?.Invoke(ImageIndex);
    }

}
