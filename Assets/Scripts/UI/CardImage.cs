using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CardImage : MonoBehaviour
{
    public Image ImageComponent;

    public Button ImageButton;
    public int ImageIndex;

    public event Action<int> OnButtonPressed;

    public void Initialize(Sprite image, int index)
    {
        ImageComponent.sprite = image;
        ImageIndex = index;
    }

    public void ButtonPressed()
    {
        OnButtonPressed?.Invoke(ImageIndex);
    }

}
