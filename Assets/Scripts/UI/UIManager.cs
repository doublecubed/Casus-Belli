using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    #region REFERENCES

    public static UIManager Instance;

    [SerializeField] private GameObject _mainMenu;
    [SerializeField] private GameObject _startButton;
    [SerializeField] private GameObject _doneButton;
    [SerializeField] private GameObject _cardSelectionPanel;
    [SerializeField] private GameObject _gameEndPanel;
    [SerializeField] private GameObject _gameStartPanel;
    [SerializeField] private TextMeshProUGUI _winnerText;

    #endregion

    #region MONOBEHAVIOUR

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        } else
        {
            Instance = this;
        }

    }


    private void OnEnable()
    {
        DisplayMainMenu(true);
        DisplayDoneButton(false);
        DisplayGameEndPanel(false);
        DisplayCardSelectionUI(false);
    }


    #endregion

    #region METHODS

    #region Display UI Elements

    public void DisplayDoneButton(bool condition)
    {
        _doneButton.SetActive(condition);
    }


    public void DisplayStartButton(bool condition)
    {
        _startButton.SetActive(condition);
    }

    public void DisplayMainMenu(bool condition)
    {
        _mainMenu.SetActive(condition);
    }

    public void DisplayCardSelectionUI(bool condition)
    {
        _cardSelectionPanel.SetActive(condition);
    }

    #endregion

    #region Game end

    public void SetWinningPlayer(string name)
    {
        _winnerText.text = name;
    }

    public void DisplayGameEndPanel(bool condition)
    {
        _gameEndPanel.SetActive(condition);
    }

    #endregion
   

    #endregion
}
