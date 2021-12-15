using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CharacterShopButton : MonoBehaviour
{
    [Header("Unlocked")]
    [SerializeField] private CanvasGroup _unlockTemplate;
    [SerializeField] private RectTransform _characterSpriteRoot;
    [SerializeField] private Image _chooseImage;
    [Header("Locked")]
    [SerializeField] private CanvasGroup _lockedTemplate;
    [SerializeField] private TextMeshProUGUI _costText;

    private CharacterHolder _holder;
    private CharacterHolder.Character _character;
    private GameObject _model;


    private void Update()
    {
        if (_holder != null)
        {
            _chooseImage.color = Color.Lerp(_chooseImage.color, ChangeAlpha(_chooseImage.color, _holder.Current == _character ? 1f : 0f), Time.deltaTime * 5f);
        }
    }


    public void Init(CharacterHolder holder, int id) 
    {
        GetComponentInParent<Panel>().onPanelShow += () => ToggleCharacterAlpha(true, 0.25f);
        GetComponentInParent<Panel>().onPanelHide += () => ToggleCharacterAlpha(false, 0.25f);
        GetComponentInChildren<Button>().onClick.AddListener(HandleOnButtonClicked);

        _holder = holder;
        _character = holder.GetCharacter(id);

        CreateUnlockedTemplate();
        CreateLockedTemplate();

        if (_holder.IsUnlocked(_character)) _lockedTemplate.gameObject.SetActive(false);
        else _unlockTemplate.gameObject.SetActive(false);
    }

    private void CreateUnlockedTemplate() 
    {
        SpawnCharacterModel();
    }

    private void CreateLockedTemplate() 
    {
        _costText.text = $"{_character.characterCost}<sprite index=0>";
    }

    private void SpawnCharacterModel() 
    {
        _model = Instantiate(_character.characterPrefab);
        _model.transform.SetParent(_characterSpriteRoot);
        _model.transform.localPosition = Vector3.zero;
        _model.transform.localScale = Vector3.one * 200f;
        foreach (SpriteRenderer rend in _model.GetComponentsInChildren<SpriteRenderer>()) 
        {
            rend.sortingOrder = 200;
        }
    }

    private void ToggleCharacterAlpha(bool arg, float delay) 
    {
        foreach (SpriteRenderer rend in _model.GetComponentsInChildren<SpriteRenderer>())
        {
            rend.color = ChangeAlpha(rend.color, arg ? 0f : 1f);
            rend.DOColor(ChangeAlpha(rend.color, arg ? 1f : 0f), delay);
        }
    }

    private void HandleOnButtonClicked()
    {
        if (_holder.IsUnlocked(_character))
        {
            _holder.Current = _character;
            this.DOKill();
            _unlockTemplate.transform.DOScale(0.9f, 0.25f).SetTarget(this)
                .OnComplete(() => _unlockTemplate.transform.DOScale(1f, 0.25f).SetTarget(this));
        }
        else 
        {
            if (PlayerPrefs.GetInt("Money", 0) >= _character.characterCost)
            {
                PlayerPrefs.SetInt("Money", PlayerPrefs.GetInt("Money") - _character.characterCost);
                _holder.Unlock(_character);
                _holder.Current = _character;

                _lockedTemplate.DOFade(0f, 0.5f);
                _lockedTemplate.transform.DOScale(1.5f, 0.5f)
                    .OnComplete(() => _lockedTemplate.gameObject.SetActive(false));
                _unlockTemplate.gameObject.SetActive(true);
                ToggleCharacterAlpha(true, 0.5f);
                GetComponentInParent<ShopPanel>().UpdateMoneyUI();
            }
            else 
            {
                this.DOKill();
                _lockedTemplate.transform.DOScale(1.1f, 0.25f).SetTarget(this)
                    .OnComplete(() => _lockedTemplate.transform.DOScale(1f, 0.25f).SetTarget(this));
            }
        }
    }

    private Color ChangeAlpha(Color color, float a) 
    {
        color.a = a;
        return color;
    }
}
