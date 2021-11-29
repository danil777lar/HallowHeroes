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

    public Action OnTap;


    private void Awake()
    {
        _default = this;
    }

    private void Start()
    {
        GetComponent<Panel>().onPanelShow += HandleOnPanelShow;
    }


    public void UpdateScore(int score) 
    {
        _curentScore.text = "" + score;
    }


    private void HandleOnPanelShow() 
    {
        _curentScore.text = "0";
        _bestScore.text = $"best {PlayerPrefs.GetInt("BestScore", 0)}";
    }

    void IPointerDownHandler.OnPointerDown(PointerEventData eventData)
    {
        OnTap?.Invoke();
    }
}
