using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ShopPanel : MonoBehaviour
{
    [SerializeField] private Panel _panel;
    [SerializeField] private Button _backButton;
    [SerializeField] private ScrollRect _scroll;
    [SerializeField] private CharacterShopButton _buttonPrefab;
    [SerializeField] private CharacterHolder _characterHolder;


    private void Awake()
    {
        _panel.onPanelShow += HandleOnPanelShow;
    }

    private void Start()
    {
        Build();
        _backButton.onClick.AddListener(HandleOnBackButton);
    }


    private void Build() 
    {
        int rowCount = Mathf.CeilToInt(_characterHolder.Count / 2f);
        float padding = (720f - ((RectTransform)_buttonPrefab.transform).sizeDelta.y * 2f) / 3f;
        float contentHeight = rowCount * ((RectTransform)_buttonPrefab.transform).sizeDelta.y;
        contentHeight += padding * (rowCount + 1);
        _scroll.content.sizeDelta = Vector2.up * contentHeight;

        for (int i = 0; i < _characterHolder.Count; i++) 
        {
            CharacterShopButton button = Instantiate(_buttonPrefab);
            button.transform.SetParent(_scroll.content);
            Vector2 position = Vector2.zero;
            position.x = ((((RectTransform)button.transform).sizeDelta.x / 2f) + padding / 2f);
            position.x *= (i % 2 == 0 ? -1f : 1f);
            position.y = -((RectTransform)button.transform).sizeDelta.y * Mathf.FloorToInt(i / 2f);
            position.y -= ((RectTransform)button.transform).sizeDelta.y / 2f;
            position.y -= padding * (Mathf.FloorToInt(i / 2f) + 1);
            ((RectTransform)button.transform).anchoredPosition = position;
            ((RectTransform)button.transform).localScale = Vector3.one;
            button.Init(_characterHolder, i);
        }
    }

    private void HandleOnPanelShow()
    {

    }

    private void HandleOnBackButton() 
    {
        LevelManager.Default.Restart();
        UIManager.Default.CurentState = UIManager.State.Start;
    }
}
