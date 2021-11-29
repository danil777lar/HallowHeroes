using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

[RequireComponent(typeof(PlatformerGravity))]
public class PlayerAnimation : MonoBehaviour
{
    [Header("Cloak")]
    [SerializeField] private Transform _cloakTransform;
    [SerializeField] private float _fullAnimSpeed;
    [SerializeField] private float _deformScaleCloak;
    [SerializeField] private float _deformSpeedCloak;
    [Header("Legs")]
    [SerializeField] private Transform _legsTransform;
    [SerializeField] private float _legsPositionGrounded;
    [SerializeField] private float _legsPositionNonGrounded;
    [Header("Main")]
    [SerializeField] private Transform _bodyTransform;
    [SerializeField, Range(0f, 1f)] private float _deformScaleBody;
    [SerializeField] private float _deformSpeedBody;

    private bool _failAnimPlay;
    private float _deformSinValueBody;
    private float _deformSinValueCloak;
    private Vector2 _fallVelocity;
    private Camera _cam;
    private PlatformerGravity _gravity;


    private void Start()
    {
        _gravity = GetComponent<PlatformerGravity>();
        _cam = Camera.main;
    }

    private void Update()
    {
        if (_failAnimPlay)
        {
            FailAnimation();
        }
        else
        {
            CloakAnimation();
            LegsAnimation();
            BodyAnimation();
        }
    }


    public void PlayFailAnim(Vector2 fallDirection) 
    {
        _failAnimPlay = true;
        _fallVelocity = fallDirection.normalized * 7f;
        _fallVelocity.y = Mathf.Max(_fallVelocity.y, 0f);
    }


    private void CloakAnimation()
    {
        Vector3 targetScale = _cloakTransform.localScale;
        targetScale.y = 1f + (Mathf.Lerp(0f, 0.35f, Mathf.Abs(_gravity.VelocityY) / _fullAnimSpeed) * (_gravity.VelocityY < 0f ? -1f : 1f));
        targetScale.y -= Mathf.Sin(_deformSinValueCloak) * _deformScaleCloak;

        _cloakTransform.localScale = Vector3.Lerp(_cloakTransform.localScale, targetScale, Time.deltaTime * 10f);
        _deformSinValueCloak += (_gravity.IsGrounded ? _deformSpeedCloak : _deformSpeedCloak / 10f) * Time.deltaTime;
    }

    private void LegsAnimation() 
    {
        Vector3 targetPosition = Vector3.up * (_gravity.IsGrounded ? _legsPositionGrounded : _legsPositionNonGrounded);
        _legsTransform.localPosition = Vector3.Lerp(_legsTransform.localPosition, targetPosition, Time.deltaTime * 25f);
    }

    private void BodyAnimation()
    {
        if (_gravity.IsGrounded)
        {
            Vector3 targetScale = _bodyTransform.localScale;
            targetScale.y = 1f + Mathf.Sin(_deformSinValueBody) * _deformScaleBody;
            _bodyTransform.localScale = targetScale;
            _deformSinValueBody += _deformSpeedBody * Time.deltaTime;
        }
    }

    private void FailAnimation() 
    {
        if (transform.position.y > _cam.ViewportToWorldPoint(Vector3.zero).y - 2f)
        {
            Vector3 targetPosition = (Vector2)transform.position + (_fallVelocity * Time.deltaTime);
            targetPosition.z = transform.position.z;
            Vector2 direction = targetPosition - transform.position;
            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
            Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);
            transform.position = targetPosition;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, Time.deltaTime * 10f);

            _fallVelocity.y -= 9.8f * 2f * Time.deltaTime;
            _fallVelocity.x *= 1f - Time.deltaTime; 
        }
    }
}
