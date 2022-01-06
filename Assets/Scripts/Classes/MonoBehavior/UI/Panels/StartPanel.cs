using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class StartPanel : MonoBehaviour, IPointerClickHandler
{
    [SerializeField] private Button _shopButton;
    [SerializeField] private Button _rateButton;
    [SerializeField] private Button _soundButton;

    [Header("Sound sprites")] 
    [SerializeField] private Image _soundIcon;
    [SerializeField] private Sprite _soundEnableSprite;
    [SerializeField] private Sprite _soundDisableSprite;
    

    private void Start()
    {
        _shopButton.onClick.AddListener(HandleOnShopButtonClick);
        _rateButton.onClick.AddListener(HandleOnRateButtonClick);
        _soundButton.onClick.AddListener(HandleOnSoundButtonClick);
    }

    private void HandleOnShopButtonClick() 
    {
        UIManager.Default.CurentState = UIManager.State.Shop;
    }

    private void HandleOnRateButtonClick() 
    {

    }

    private void HandleOnSoundButtonClick()
    {
        SoundHolder.Default.soundActive = !SoundHolder.Default.soundActive;
        _soundIcon.sprite = SoundHolder.Default.soundActive ? _soundEnableSprite : _soundDisableSprite;
    }

    void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
    {
        UIManager.Default.CurentState = UIManager.State.Process;
    }
}
