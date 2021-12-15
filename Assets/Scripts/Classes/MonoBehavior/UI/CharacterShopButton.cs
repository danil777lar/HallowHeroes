using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class CharacterShopButton : MonoBehaviour
{
    [Header("Unlocked")]
    [SerializeField] private RectTransform _unlockTemplate;
    [SerializeField] private RectTransform _characterSpriteRoot;
    [Header("Locked")]
    [SerializeField] private RectTransform _lockedTemplate;
    [SerializeField] private TextMeshProUGUI _costText;

    private CharacterHolder _holder;
    private CharacterHolder.Character _character;
    private GameObject _model;


    public void Init(CharacterHolder holder, int id) 
    {
        GetComponentInParent<Panel>().onPanelShow += () => ToggleCharacterAlpha(true);
        GetComponentInParent<Panel>().onPanelHide += () => ToggleCharacterAlpha(false);

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

    private void ToggleCharacterAlpha(bool arg) 
    {
        foreach (SpriteRenderer rend in _model.GetComponentsInChildren<SpriteRenderer>())
        {
            rend.color = ChangeAlpha(rend.color, arg ? 0f : 1f);
            rend.DOColor(ChangeAlpha(rend.color, arg ? 1f : 0f), 0.25f);
        }
    }

    private Color ChangeAlpha(Color color, float a) 
    {
        color.a = a;
        return color;
    }
}
