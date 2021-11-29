using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class FailPanel : MonoBehaviour
{
    [SerializeField] private Button _restartButton;
    [SerializeField] private Image _transitionPanel;
    [SerializeField] private GameObject _mainPanel;


    private void Start()
    {
        _restartButton.onClick.AddListener(HandleOnRestartButton);
    }

    private void OnEnable()
    {
        _transitionPanel.color = new Color(0f, 0f, 0f, 0f);
        _mainPanel.SetActive(true);
        _restartButton.interactable = true;
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
}
