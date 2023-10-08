using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardPicker : MonoBehaviour
{
    [SerializeField] private Camera _camera;

    public event Action<Card> OnCardClicked;
    public event Action<Deck> OnDeckClicked;

    private void Start()
    {
        _camera = Camera.main;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray pickerRay = _camera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(pickerRay, out RaycastHit hitInfo))
            {
                if (hitInfo.transform.CompareTag("Card")) OnCardClicked?.Invoke(hitInfo.transform.GetComponent<Card>());

                if (hitInfo.transform.CompareTag("Deck")) OnDeckClicked?.Invoke(hitInfo.transform.GetComponent<Deck>());
            }
        }
    }
}
