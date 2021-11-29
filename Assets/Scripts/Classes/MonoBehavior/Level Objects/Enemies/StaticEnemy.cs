using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticEnemy : PlarformAttachment
{
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Transform _sprite;
    [SerializeField] private float _animationSpeed;
    [SerializeField, Range(0f, 1f)] private float _animationScale;

    private float _animationOffset;
    private Vector3 _defaultScale;


    private void Start()
    {
        _defaultScale = _sprite.localScale;
        _animationOffset = Random.Range(0f, 1000f);
    }

    private void FixedUpdate()
    {
        Vector3 targetScale = _defaultScale;
        targetScale.y += Mathf.Sin((Time.time + _animationOffset) * _animationSpeed) * _animationScale;
        _sprite.localScale = targetScale;
    }

    public override void AnchorToPlatform(float surfacePosition)
    {
        boundsWidth = _collider.bounds.max.x - _collider.bounds.min.x;
        base.AnchorToPlatform(surfacePosition);
    }
}
