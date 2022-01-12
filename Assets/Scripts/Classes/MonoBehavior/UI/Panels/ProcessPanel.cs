using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using TMPro;

public class ProcessPanel : MonoBehaviour, IPointerDownHandler
{
    #region Singleton
    static private ProcessPanel _default;
    static public ProcessPanel Default => _default;
    #endregion

    [SerializeField] private TextMeshProUGUI _curentScore;
    [SerializeField] private TextMeshProUGUI _bestScore;
    [SerializeField] private TextMeshProUGUI _curentCoins;
    [SerializeField] private TextMeshProUGUI _totalCoins;

    private int _coins;
    private MoneyAnimation _moneyAnim;

    public Action OnTap;


    private void Awake()
    {
        _default = this;
    }

    private void Start()
    {
        GetComponent<Panel>().onPanelShow += HandleOnPanelShow;
        _moneyAnim = GetComponent<MoneyAnimation>();
        _coins = 0;
        UpdateMoneyUI();
    }


    public void UpdateScore(int score) 
    {
        _curentScore.text = "" + score;
    }

    public void PlayCoinAnim(int count, Vector3 position) 
    {
        for (int i = 0; i < count; i++) 
        {
            _moneyAnim.PlayMoneyUpAnim(position, () => 
            {
                _coins++;
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money", 0) + 1);
                UpdateMoneyUI();
            }, false);
        }
    }


    private void HandleOnPanelShow() 
    {
        _coins = 0;
        _curentScore.text = "0";
        _bestScore.text = $"best {PlayerPrefs.GetInt("BestScore", 0)}";
        UpdateMoneyUI();
    }

    private void UpdateMoneyUI() 
    {
        _curentCoins.text = $"{_coins}<sprite index=0>";
        _totalCoins.text = $"total {PlayerPrefs.GetInt("Money", 0)}";
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnTap?.Invoke();
    }
}
