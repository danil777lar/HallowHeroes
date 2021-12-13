using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using DG.Tweening;

public class FailPanel : MonoBehaviour
{
    [SerializeField] private Image _transitionPanel;
    [SerializeField] private GameObject _mainPanel;
    [Header("Buttons")]
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _restartButton;
    [SerializeField] private Button _homeButton;


    private void Start()
    {
        _shopButton.onClick.AddListener(HandleOnShopButton);
        _restartButton.onClick.AddListener(HandleOnRestartButton);
        _homeButton.onClick.AddListener(HandleOnHomeButton);
    }

    private void OnEnable()
    {
        _transitionPanel.color = new Color(0f, 0f, 0f, 0f);
        _mainPanel.SetActive(true);
        _restartButton.interactable = true;
    }


    private void HandleOnShopButton()
    {
        UIManager.Default.CurentState = UIManager.State.Shop;
    }

    private void HandleOnRestartButton() 
    {
        _restartButton.interactable = false;
        _transitionPanel.DOColor(new Color(0f, 0f, 0f, 1f), 0.25f)
            .OnComplete(() => 
            {
                _mainPanel.SetActive(false);
                LevelManager.Default.Restart();
                UIManager.Default.CurentState = UIManager.State.Process;
            });
    }

    private void HandleOnHomeButton() 
    {
        _restartButton.interactable = false;
        _transitionPanel.DOColor(new Color(0f, 0f, 0f, 1f), 0.25f)
            .OnComplete(() =>
            {
                _mainPanel.SetActive(false);
                LevelManager.Default.Restart();
                UIManager.Default.CurentState = UIManager.State.Start;
            });
    }
}
