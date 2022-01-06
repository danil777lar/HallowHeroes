using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using DG.Tweening;
using Vector3 = UnityEngine.Vector3;

public class Money : PlatformAttachment
{
    [SerializeField] private int _value;
    [SerializeField] private SpriteRenderer _coinSprite;
    [SerializeField] private ParticleSystem _partsPrefab;
    [Header("Animation")] 
    [SerializeField] private float _animSpeed;
    [SerializeField] private float _animScale;
    private int _seed;
    private Vector3 _defaultPosition;
    

    private void FixedUpdate()
    {
        Vector3 offset = Vector3.up * (Mathf.Sin((Time.time + _seed) * _animSpeed) * _animScale);
        transform.position = _defaultPosition + offset;
    }


    public override void AnchorToPlatform(float surfacePosition)
    {
        Vector3 targetPosition = Vector3.up * surfacePosition;
        float leftScreenSide = Camera.main.ViewportToWorldPoint(Vector3.zero).x + 1f;
        float rightScreenSide = Camera.main.ViewportToWorldPoint(Vector3.right).x - 1f;
        float rand = Random.Range(0f, 1f);
        targetPosition.x = Mathf.Lerp(leftScreenSide, rightScreenSide, rand);
        if (rand < 0.5f)
        {
            Vector3 targetScale = transform.localScale;
            targetScale.x *= -1f;
            transform.localScale = targetScale;
        }
        transform.localPosition = targetPosition;
        _defaultPosition = transform.position;
        _seed = Random.Range(0, 1000);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer == LayerMask.NameToLayer("Player")) 
        {
            ProcessPanel.Default.PlayCoinAnim(_value, _coinSprite.transform.position);
            ParticleSystem instance = Instantiate(_partsPrefab);
            instance.transform.position = _coinSprite.transform.position;
            Destroy(instance, instance.main.duration);
            _coinSprite.DOColor(new Color(1f, 1f, 1f, 0f), 0.25f)
                .OnComplete(() => Destroy(gameObject));
        }
    }
}
