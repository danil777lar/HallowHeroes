using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class Platform : MonoBehaviour, ILandableSurface
{
    [SerializeField] private BoxCollider2D _platformBody;
    [SerializeField] private SpriteRenderer _topSprite;
    [SerializeField] private SpriteRenderer _bottomSprite;
    [SerializeField] private TextMeshProUGUI _numText;

    private bool _startPlatform;
    private bool _landed;
    private float _defaultHeight;
    private Tween _anim;

    public Action<Platform> OnLanded;


    private void Start()
    {
        _defaultHeight = transform.position.y;
        _numText.transform.parent.localScale = Vector3.one;
    }


    public void Init(LevelStage stage, bool startPlatform)
    {
        _startPlatform = startPlatform;
        Vector3 targetScale = _platformBody.transform.localScale;
        targetScale.x = (Camera.main.ViewportToWorldPoint(Vector3.right) - Camera.main.ViewportToWorldPoint(Vector3.zero)).x;
        _platformBody.transform.localScale = targetScale;

        if (!startPlatform && stage.enemyPrefabs.Count > 0 && stage.enemySpawnChance >= UnityEngine.Random.Range(0f, 1f)) 
        {
            PlatformAttachment instance = Instantiate(stage.enemyPrefabs[UnityEngine.Random.Range(0, stage.enemyPrefabs.Count)]);
            instance.transform.SetParent(transform);
            instance.AnchorToPlatform(_platformBody.bounds.max.y);
        }

        if (stage.environmentPrefabs != null && stage.environmentPrefabs.Count > 0)
        {
            for (int i = 0; i < UnityEngine.Random.Range(1, 5); i++)
            {
                PlatformAttachment instance = Instantiate(stage.environmentPrefabs[UnityEngine.Random.Range(0, stage.environmentPrefabs.Count)]);
                instance.transform.SetParent(transform);
                instance.AnchorToPlatform(_platformBody.bounds.max.y);
            }
        }

        if (!startPlatform && stage.moneyPrefab && stage.moneySpawnChance >= UnityEngine.Random.Range(0f, 1f))
        {
            PlatformAttachment instance = Instantiate(stage.moneyPrefab);
            instance.transform.SetParent(transform);
            instance.AnchorToPlatform(_platformBody.bounds.max.y);
        }

        _numText.transform.parent.localPosition = new Vector3(-(_platformBody.transform.localScale.x / 2f) + 0.5f, 0f, 0f);
    }

    public void SetColor(PlatformColors colors, float duration) 
    {
        _topSprite.DOColor(colors.topColor, duration);
        _bottomSprite.DOColor(colors.bottomColor, duration);
    }

    public void SetNumText(int num) 
    {
        _numText.transform.localScale = Vector3.zero;
        _numText.text = $"{num}";
        _numText.transform.DOScale(1f, 0.3f)
            .SetEase(Ease.OutBack);
    }

    void ILandableSurface.LandOn()
    {
        if (!_startPlatform)
        {
            if (!_landed) 
            {
                _landed = true;
                OnLanded?.Invoke(this);
            }

            _anim?.Kill();
            _anim = transform.DOMoveY(_defaultHeight - 0.25f, 0.15f)
                        .OnComplete(() =>
                        {
                            _anim = transform.DOMoveY(_defaultHeight, 2f)
                                        .SetEase(Ease.OutElastic);
                        });
            SoundHolder.Default.PlayFromSoundPack("Platform");
        }
    }
}
