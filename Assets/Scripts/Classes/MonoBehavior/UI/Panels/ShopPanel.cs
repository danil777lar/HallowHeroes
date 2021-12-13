using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Button _backButton;

    private void Start()
    {
        _backButton.onClick.AddListener(HandleOnBackButton);
    }

    private void HandleOnBackButton() 
    {
        LevelManager.Default.Restart();
        UIManager.Default.CurentState = UIManager.State.Start;
    }
}
