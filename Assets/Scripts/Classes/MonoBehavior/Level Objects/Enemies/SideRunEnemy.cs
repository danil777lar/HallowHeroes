using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class SideRunEnemy : PlatformAttachment
{
    [Header("Links")]
    [SerializeField] private BoxCollider2D _collider;
    [SerializeField] private Transform _body;
    [SerializeField] private Canvas _runWarningUI;
    [Header("Run Options")] 
    [SerializeField] private float _runDuration;
    [SerializeField] private float _runDelay;
    [SerializeField] private float _warningDelay;

    private bool _leftSide;
    private Tween _warnScale;
    
    
    public override void AnchorToPlatform(float surfacePosition)
    {
        _leftSide = Random.Range(0, 2) == 0;
        _boundsWidth = _collider.bounds.max.x - _collider.bounds.min.x;
        Vector3 targetPosition = Vector3.up * surfacePosition;
        targetPosition.x = GetBodyPosition(_leftSide);
        transform.localPosition = targetPosition;
        if (!_leftSide) _body.localScale = new Vector3(-_body.localScale.x, _body.localScale.y, _body.localScale.z);

        Vector3 warnPosition = _runWarningUI.transform.position;
        warnPosition.x = GetWarningPosition(_leftSide);
        _runWarningUI.transform.position = warnPosition;
        
        StartCoroutine(RunCycle());
    }


    private float GetBodyPosition(bool leftSide)
    {
        float leftPosition = Camera.main.ViewportToWorldPoint(Vector3.zero).x - _boundsWidth * 2f;
        float rightPosition = Camera.main.ViewportToWorldPoint(Vector3.right).x + _boundsWidth * 2f;
        return leftSide ? leftPosition : rightPosition;
    }

    private float GetWarningPosition(bool leftSide)
    {
        float leftPosition = Camera.main.ViewportToWorldPoint(Vector3.zero).x + 0.5f;
        float rightPosition = Camera.main.ViewportToWorldPoint(Vector3.right).x - 0.5f;
        return leftSide ? leftPosition : rightPosition;
    }


    private void SetActiveWaringUI(bool arg)
    {
        _warnScale?.Kill();
        _warnScale = _runWarningUI.transform.DOScale(arg ? 1f : 0f, 0.25f)
            .SetEase(arg ? Ease.OutBack : Ease.InBack);
    }

    private IEnumerator RunCycle()
    {
        yield return new WaitForSeconds(_runDelay - _warningDelay);
        SetActiveWaringUI(true);
        yield return new WaitForSeconds(_warningDelay);
        SetActiveWaringUI(false);
        _body.transform.DOMoveX(GetBodyPosition(!_leftSide), _runDuration);
        yield return new WaitForSeconds(_runDuration);
        Vector3 defaultPosition = _body.transform.position;
        defaultPosition.x = GetBodyPosition(_leftSide);
        _body.transform.position = defaultPosition;
        StartCoroutine(RunCycle());
    }
}
